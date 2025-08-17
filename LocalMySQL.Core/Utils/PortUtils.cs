using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace LocalMySQL.Core.Utils
{
    public static class PortUtils
    {
        // Estrutura que representa uma linha da tabela TCP IPv4
        [StructLayout(LayoutKind.Sequential)]
        public struct MIB_TCPROW_OWNER_PID
        {
            public uint state;
            public uint localAddr;
            public byte localPort1;
            public byte localPort2;
            public byte localPort3;
            public byte localPort4;
            public byte reserved; // para completar 4 bytes da porta
            public uint owningPid;

            public int Port
            {
                get
                {
                    // Porta está armazenada em bytes invertidos
                    return (localPort1 << 8) + localPort2;
                }
            }
        }

        // Estrutura da tabela TCP
        [StructLayout(LayoutKind.Sequential)]
        public struct MIB_TCPTABLE_OWNER_PID
        {
            public uint dwNumEntries;
            public MIB_TCPROW_OWNER_PID table;
        }

        public enum TcpTableClass
        {
            TCP_TABLE_OWNER_PID_ALL = 5,
        }

        [DllImport("iphlpapi.dll", SetLastError = true)]
        private static extern uint GetExtendedTcpTable(
            nint pTcpTable,
            ref int dwOutBufLen,
            bool sort,
            int ipVersion,
            TcpTableClass tblClass,
            uint reserved = 0);

        public static int GetProcessIdByPort(int port)
        {
            int AF_INET = 2; // IPv4
            int buffSize = 0;

            // Chamada para obter tamanho do buffer necessário
            uint res = GetExtendedTcpTable(nint.Zero, ref buffSize, true, AF_INET, TcpTableClass.TCP_TABLE_OWNER_PID_ALL);
            if (res != 0 && res != 122) // 122 = ERROR_INSUFFICIENT_BUFFER
            {
                return -1;
            }

            nint tcpTablePtr = Marshal.AllocHGlobal(buffSize);
            try
            {
                res = GetExtendedTcpTable(tcpTablePtr, ref buffSize, true, AF_INET, TcpTableClass.TCP_TABLE_OWNER_PID_ALL);
                if (res != 0)
                {
                    return -1;
                }

                int numEntries = Marshal.ReadInt32(tcpTablePtr);

                // Offset para o início da tabela
                nint rowPtr = nint.Add(tcpTablePtr, 4);

                int rowSize = Marshal.SizeOf(typeof(MIB_TCPROW_OWNER_PID));

                for (int i = 0; i < numEntries; i++)
                {
                    var row = Marshal.PtrToStructure<MIB_TCPROW_OWNER_PID>(rowPtr);

                    // Porta local está na estrutura, mas em formato big-endian nos bytes
                    int localPort = row.localPort1 << 8 | row.localPort2;
                    if (localPort == port)
                    {
                        return (int)row.owningPid;
                    }

                    rowPtr = nint.Add(rowPtr, rowSize);
                }
            }
            finally
            {
                Marshal.FreeHGlobal(tcpTablePtr);
            }

            return -1;
        }

        public static bool IsPortOpen(int port)
        {
            try
            {
                using var client = new TcpClient();
                var result = client.BeginConnect(IPAddress.Loopback, port, null, null);
                var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(100));
                return success && client.Connected;
            }
            catch
            {
                return false;
            }
        }

        public static int FindAvailablePort(string basePath, int startPort = 3306, int maxPort = 3399)
        {
            var usedPorts = new HashSet<int>();

            foreach (var dir in Directory.GetDirectories(basePath))
            {
                var iniPath = Path.Combine(dir, "my.ini");
                if (File.Exists(iniPath))
                {
                    var lines = File.ReadAllLines(iniPath);
                    var portLine = lines.FirstOrDefault(l => l.TrimStart().StartsWith("port"));
                    if (portLine != null && int.TryParse(portLine.Split('=')[1].Trim(), out var p))
                    {
                        usedPorts.Add(p);
                    }
                }
            }

            for (int port = startPort; port <= maxPort; port++)
            {
                if (!usedPorts.Contains(port) && !IsPortOpen(port))
                {
                    return port;
                }
            }

            throw new Exception("Nenhuma porta livre disponível entre 3306 e 3399.");
        }
    }
}

