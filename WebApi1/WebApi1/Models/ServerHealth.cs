using System.ComponentModel.DataAnnotations;

public class ServerHealth
{
    ServerHealth()
    {
        

    }
    [Key]
     public DateTime dateTime { get; set; }
    public string ServerName { get; set; }
    public double CPUUsage { get; set; }
    public double MemoryUsage { get; set; }
    public double DiskUsage { get; set; }
    public string PingStatus { get; set; }
    public string DNSResolution { get; set; }
}