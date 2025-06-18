using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
[Microsoft.EntityFrameworkCore.PrimaryKey(nameof(dateTime), nameof(ServerName))]
public class SystemInfo
{
    [Column(Order = 0)]
    public DateTime dateTime { get; set; }
    [Column(Order = 1)]
    public string ServerName { get; set; }
    [JsonProperty("TotalCPUUsage")]
    public double TotalCPUUsage { get; set; }

    [JsonProperty("MemoryUsagePercentage")]
    public double MemoryUsagePercentage { get; set; }

    [JsonProperty("Top5CPUProcesses")]
    public List<CpuProcessInfo> Top5CPUProcesses { get; set; }

    [JsonProperty("Top5MemoryProcesses")]
    public List<MemoryProcessInfo> Top5MemoryProcesses { get; set; }
}