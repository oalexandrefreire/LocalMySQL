namespace LocalMySQL.Core.dto;

public class MysqlInstanceDto
{
    public string Version { get; set; } = "";
    public string Path { get; set; } = "";
    public int Port { get; set; }
    public bool Running { get; set; }
}
