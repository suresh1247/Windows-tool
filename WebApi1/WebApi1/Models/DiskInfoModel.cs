using System.ComponentModel.DataAnnotations;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class DiskInfoModel
{
    private JsonNode? jsonData;

    // Parameterless constructor
    public DiskInfoModel()
    {
    }

    // Constructor with parameters
    public DiskInfoModel(DateTime _datetime, string _servername, string _driveletter, string _Totalcapacityingb, string _usedspaceingb, string _freespaceingb, string _freespacepercent)
    {
        dateTime = _datetime;
        Server = _servername;
        DriveLetter = _driveletter;
        TotalCapacityGB = _Totalcapacityingb;
        UsedSpaceGB = _usedspaceingb;
        FreeSpaceGB = _freespaceingb;
        FreeSpacePercent = _freespacepercent;
    }

    [Key]
    public DateTime dateTime { get; set; }
    public string Server { get; set; }
    public string DriveLetter { get; set; }
    public string TotalCapacityGB { get; set; }
    public string UsedSpaceGB { get; set; }
    public string FreeSpaceGB { get; set; }
    public string FreeSpacePercent { get; set; }
}
