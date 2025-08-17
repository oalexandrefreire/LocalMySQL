using LocalMySQL.Core.dto;
using LocalMySQL.Core.Utils;
using System.Diagnostics;
using System.IO.Compression;
using System.Net;
using System.Text.Json;

namespace LocalMySQL.Core;

public class MysqlManager
{
    private readonly string _basePath;

    public MysqlManager(string basePath)
    {
        _basePath = basePath;
        if (!Directory.Exists(_basePath))
            Directory.CreateDirectory(_basePath);
    }

    public List<MysqlInstanceDto> ListInstances()
    {
        var list = new List<MysqlInstanceDto>();

        if (!Directory.Exists(_basePath))
            return list;

        foreach (var dir in Directory.GetDirectories(_basePath))
        {
            var version = Path.GetFileName(dir);
            var dataDir = Path.Combine(dir, "data");
            var runningFile = Path.Combine(dataDir, "mysqld.pid");
            var iniPath = Path.Combine(dir, "my.ini");

            int? port = null;
            if (File.Exists(iniPath))
            {
                var lines = File.ReadAllLines(iniPath);
                var portLine = lines.FirstOrDefault(l => l.StartsWith("port"));
                if (portLine != null && int.TryParse(portLine.Split('=')[1].Trim(), out var parsedPort))
                {
                    port = parsedPort;
                }
            }

            bool running = false;
            if (File.Exists(runningFile))
            {
                if (int.TryParse(File.ReadAllText(runningFile), out var pid))
                {
                    running = IsProcessRunning(pid);
                    if (!running)
                    {
                        File.Delete(runningFile);
                    }
                }
            }

            var instance = new MysqlInstanceDto
            {
                Version = version,
                Path = dir,
                Running = running,
                Port = port ?? 3306
            };

            list.Add(instance);
        }

        return list;
    }

    public async Task<(bool Success, string? ErrorMessage)> DownloadMysqlAsync(string version, IProgress<double>? progress = null)
    {
        var versionPath = Path.Combine(_basePath, version);
        if (Directory.Exists(versionPath) && Directory.Exists(Path.Combine(versionPath, "bin")))
            return (true, "already");

        try
        {
            Directory.CreateDirectory(versionPath);

            string url = $"https://downloads.mysql.com/archives/get/p/23/file/mysql-{version}-winx64.zip";
            string zipFile = Path.Combine(versionPath, "mysql.zip");

            var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            using var httpClient = new HttpClient(handler);
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
            httpClient.DefaultRequestHeaders.Accept.ParseAdd("text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            httpClient.DefaultRequestHeaders.AcceptEncoding.ParseAdd("gzip, deflate, br");
            httpClient.DefaultRequestHeaders.AcceptLanguage.ParseAdd("en-US,en;q=0.9");
            httpClient.DefaultRequestHeaders.Connection.ParseAdd("keep-alive");
            httpClient.DefaultRequestHeaders.Referrer = new Uri("https://dev.mysql.com/downloads/mysql/");

            using var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            var totalBytes = response.Content.Headers.ContentLength ?? -1L;
            var canReportProgress = totalBytes > 0 && progress != null;

            using var inputStream = await response.Content.ReadAsStreamAsync();

            using (var fileStream = File.Create(zipFile))
            {
                var buffer = new byte[81920];
                long totalRead = 0;
                int read;
                while ((read = await inputStream.ReadAsync(buffer)) > 0)
                {
                    await fileStream.WriteAsync(buffer.AsMemory(0, read));
                    totalRead += read;

                    if (canReportProgress)
                    {
                        double percent = (double)totalRead / totalBytes * 100;
                        progress!.Report(percent);
                    }
                }
            } 

            ZipFile.ExtractToDirectory(zipFile, versionPath, overwriteFiles: true);

            File.Delete(zipFile);

            var innerDir = Directory.GetDirectories(versionPath).FirstOrDefault(d => Path.GetFileName(d).StartsWith("mysql-"));
            if (innerDir != null)
            {
                foreach (var file in Directory.GetFiles(innerDir, "*", SearchOption.AllDirectories))
                {
                    var relative = Path.GetRelativePath(innerDir, file);
                    var dest = Path.Combine(versionPath, relative);
                    Directory.CreateDirectory(Path.GetDirectoryName(dest)!);
                    File.Move(file, dest, overwrite: true);
                }
                Directory.Delete(innerDir, true);
            }

            var dataDir = Path.Combine(versionPath, "data");
            Directory.CreateDirectory(dataDir);
            int port = PortUtils.FindAvailablePort(_basePath);
            var ini = $$"""
            [mysqld]
            basedir={{versionPath.Replace("\\", "/")}}
            datadir={{dataDir.Replace("\\", "/")}}
            port={{port}}
            socket=mysql.sock
            pid-file=mysqld.pid
            log-error=mysql.err
            max_allowed_packet=64M
            sql_mode=NO_ENGINE_SUBSTITUTION,STRICT_TRANS_TABLES
            """;
            await File.WriteAllTextAsync(Path.Combine(versionPath, "my.ini"), ini);

            var mysqldPath = Path.Combine(versionPath, "bin", "mysqld.exe");
            var initProc = Process.Start(new ProcessStartInfo
            {
                FileName = mysqldPath,
                Arguments = "--initialize-insecure",
                WorkingDirectory = versionPath,
                UseShellExecute = false,
                CreateNoWindow = true
            });
            if (initProc != null)
            {
                await initProc.WaitForExitAsync();
            }

            return (true, null);
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    public bool IsProcessRunning(int pid)
    {
        try
        {
            var proc = Process.GetProcessById(pid);
            return !proc.HasExited;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> StartInstanceAsync(string version)
    {
        var versionPath = Path.Combine(_basePath, version);
        var mysqldPath = Path.Combine(versionPath, "bin", "mysqld.exe");
        var iniPath = Path.Combine(versionPath, "my.ini");
        var runningFile = Path.Combine(versionPath, "data", "mysqld.pid");

        if (!File.Exists(mysqldPath)) return false;

        var port = 3306;
        if (File.Exists(iniPath))
        {
            var lines = File.ReadAllLines(iniPath);
            var portLine = lines.FirstOrDefault(l => l.StartsWith("port"));
            if (portLine != null && int.TryParse(portLine.Split('=')[1].Trim(), out var parsedPort))
            {
                port = parsedPort;
            }
        }

        var proc = Process.Start(new ProcessStartInfo
        {
            FileName = mysqldPath,
            Arguments = $"--defaults-file=\"{iniPath}\"",
            WorkingDirectory = versionPath,
            UseShellExecute = false,
            CreateNoWindow = true
        });

        if (proc == null) return false;

        var timeout = TimeSpan.FromSeconds(3);
        var sw = Stopwatch.StartNew();
        bool isRunning = false;

        while (sw.Elapsed < timeout)
        {
            int pidOnPort = PortUtils.GetProcessIdByPort(port);
            if (pidOnPort == proc.Id)
            {
                isRunning = true;
                break;
            }
            await Task.Delay(500);
        }

        if (!isRunning)
        {
            try { proc.Kill(); } catch { }
            return false;
        }

        File.WriteAllText(runningFile, proc.Id.ToString());

        return true;
    }

    public List<string> GetVersions() => JsonSerializer.Deserialize<List<string>>(File.ReadAllText("versions.json"));

    public bool StopInstance(string version)
    {
        var versionPath = Path.Combine(_basePath, version);
        var pidFile = Path.Combine(versionPath, "data", "mysqld.pid");

        if (!File.Exists(pidFile))
            return false; 

        if (!int.TryParse(File.ReadAllText(pidFile).Trim(), out int pid))
            return false;

        try
        {
            var proc = Process.GetProcessById(pid);

            if (!proc.ProcessName.Equals("mysqld", StringComparison.OrdinalIgnoreCase))
                return false;

            proc.Kill(true); 
            proc.WaitForExit(5000); 

            File.Delete(pidFile);
            return true;
        }
        catch (ArgumentException)
        {
            File.Delete(pidFile); 
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<int> GetDatabaseCountAsync(MysqlInstanceDto instance)
    {
        try
        {
            using var conn = new MySql.Data.MySqlClient.MySqlConnection($"server=127.0.0.1;port={instance.Port};user=root;");
            await conn.OpenAsync();
            var cmd = new MySql.Data.MySqlClient.MySqlCommand("SHOW DATABASES;", conn);
            using var reader = await cmd.ExecuteReaderAsync();

            int count = 0;
            while (await reader.ReadAsync())
            {
                var dbName = reader.GetString(0);
                if (dbName is not ("information_schema" or "performance_schema" or "mysql" or "sys"))
                    count++;
            }

            return count;
        }
        catch
        {
            return -1; // erro
        }
    }

    public async Task<int> GetActiveConnectionsAsync(MysqlInstanceDto instance)
    {
        try
        {
            using var conn = new MySql.Data.MySqlClient.MySqlConnection($"server=127.0.0.1;port={instance.Port};user=root;");
            await conn.OpenAsync();
            var cmd = new MySql.Data.MySqlClient.MySqlCommand("SHOW PROCESSLIST;", conn);
            using var reader = await cmd.ExecuteReaderAsync();

            int count = 0;
            while (await reader.ReadAsync()) count++;
            return count;
        }
        catch
        {
            return -1;
        }
    }

    private string GetMetaFilePath(string version) => Path.Combine(_basePath, version, "meta.json");

    public void SaveInstanceMeta(string version, InstanceMeta meta)
    {
        var path = GetMetaFilePath(version);
        var json = JsonSerializer.Serialize(meta);
        File.WriteAllText(path, json);
    }

    public InstanceMeta? LoadInstanceMeta(string version)
    {
        var path = GetMetaFilePath(version);
        if (!File.Exists(path)) return null;

        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<InstanceMeta>(json);
    }



}
