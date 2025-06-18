using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class CpuProcessInfo
{
    [Key]
    public int PID { get; set; }

    [JsonProperty("Name")]
    public string Name { get; set; }

    [JsonProperty("CPUUsagePercentage", NullValueHandling = NullValueHandling.Ignore)]
    public double CPUUsagePercentage { get; set; }

    [JsonProperty("MemoryUsageMB", NullValueHandling = NullValueHandling.Ignore)]
    public double MemoryUsageMB { get; set; }

    // Navigation property
    public required DateTime date { get; set; }
    public required string ServerName { get; set; }
    [ForeignKey("date, ServerName")]
    public required SystemInfo systemInfo { get; set; }
}