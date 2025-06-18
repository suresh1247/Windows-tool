

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
public class ConnectionTestModel
{
    private JsonNode? jsonData;
    public ConnectionTestModel()
    {
        
    }

    public ConnectionTestModel(string _ResolvedIP, string _ResolvedName, string _NSLookupStatus, string _DNSResolutionStatus, string _NSLookupOutput, string _Server, string _connectiontest)
    {
        ResolvedIP = _ResolvedIP;
        // ConnectionTest= (string?)jsonData["ConnectionTest"];
        ResolvedName = _ResolvedName;
        NSLookupStatus = _NSLookupOutput;
        DNSResolutionStatus = _DNSResolutionStatus;
        NSLookupOutput = _NSLookupOutput;
        Server = _Server;
        ConnectionTest = _connectiontest;
    }

    [Key]   
    public DateTime dateTime{ get; set; }
    public string ResolvedIP { get; set; }
    public string ConnectionTest { get; set; }
    public string ResolvedName { get; set; }
    public string NSLookupStatus { get; set; }
    public string DNSResolutionStatus { get; set; }
    public string NSLookupOutput { get; set; }
    public string Server { get; set; }
}