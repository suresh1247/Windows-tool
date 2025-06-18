using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using WebApi1.Services;
using System.Management;
[ApiController]
[Route("[controller]")]
public class ServerController : ControllerBase

{

    private readonly ApplicationContext db;
    public ServerController(ApplicationContext _db)
    {
        db = _db;
    }
    [HttpPost]
    [Route("ping")]
    public IActionResult PingServer([FromBody] string serverName)
    {
        string tempFilePath = Path.GetTempFileName() + ".ps1";

        try
        {
            string scriptContent = $"Test-Connection -ComputerName {serverName} -Count 2 -Quiet";
            System.IO.File.WriteAllText(tempFilePath, scriptContent);

            ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName = "powershell.exe",
                Arguments = $"-File \"{tempFilePath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(psi))
            {
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (!string.IsNullOrEmpty(error))
                {
                    return BadRequest(new { res = "No", error });
                }

                bool isReachable = output.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
                return Ok(new { res = isReachable ? "Yes" : "No" });
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { res = "No", error = ex.Message });
        }
        finally
        {
            if (System.IO.File.Exists(tempFilePath))
            {
                System.IO.File.Delete(tempFilePath);
            }
        }
    }

    [HttpPost]
    [Route("healthCheck")]
    public IActionResult HealthCheck([FromBody] string serverName)
    {
        string scriptPath = Path.Combine(Path.GetTempPath(), serverName+"ServerHealthCheck.ps1");
        string scriptContent = new HealthCheck(serverName).script;

        System.IO.File.WriteAllText(scriptPath, scriptContent);

        try
        {
            ServerHealth serverHealth = ExecutePowerShellScript(scriptPath);
            serverHealth.dateTime = DateTime.Now;

            db.Add(serverHealth);
            db.SaveChanges();

            return Ok(serverHealth);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
        finally
        {
            System.IO.File.Delete(scriptPath);
        }
    }

    private ServerHealth ExecutePowerShellScript(string scriptPath)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "powershell.exe",
            Arguments = $"-File \"{scriptPath}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (var process = Process.Start(startInfo))
        {
            var output = process.StandardOutput.ReadToEnd();
            var error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (!string.IsNullOrEmpty(error))
            {
                throw new InvalidOperationException($"Error executing PowerShell script: {error}");
            }

            return Newtonsoft.Json.JsonConvert.DeserializeObject<ServerHealth>(output);
        }
    }


public class EventLogRequest
{
    public string ComputerName { get; set; }
    public int[] EventIDs { get; set; }
    public string LogName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string EventType { get; set; }
    public string EventSource { get; set; }
}

 [HttpPost]
[Route("query")]
public IActionResult QueryEventLog([FromBody] EventLogRequest request)
{
    string scriptPath = Path.Combine(Path.GetTempPath(),request.ComputerName+ "EventIDQuery.ps1");

    var eventIDQuery = new EventIDQuery(
        request.ComputerName,
        request.EventIDs,
        request.LogName,
        request.StartDate,
        request.EndDate,
        request.EventType,
        request.EventSource
    );

    string scriptContent = eventIDQuery.Script;
    System.IO.File.WriteAllText(scriptPath, scriptContent);

    try
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "powershell.exe",
            Arguments = $"-NoProfile -ExecutionPolicy Bypass -File \"{scriptPath}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (var process = Process.Start(startInfo))
        {
            var output = process.StandardOutput.ReadToEnd();
            var error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            Console.WriteLine("PowerShell Output:");
            Console.WriteLine(output);
            Console.WriteLine("PowerShell Error:");
            Console.WriteLine(error);

            if (process.ExitCode != 0 || !string.IsNullOrWhiteSpace(error))
            {
                return StatusCode(500, new
                {
                    Error = "PowerShell error",
                    StdOut = output,
                    StdErr = error
                });
            }

            if (string.IsNullOrWhiteSpace(output))
            {
                return StatusCode(500, new { Error = "No output received from PowerShell script." });
            }

            // Safely extract and wrap JSON array if needed
            string trimmedOutput = output.Trim();

            // Attempt to wrap object list if not already wrapped
            if (!trimmedOutput.StartsWith("[") && trimmedOutput.Contains("},") && !trimmedOutput.StartsWith("{"))
            {
                trimmedOutput = "[" + trimmedOutput + "]";
            }

            try
            {
                var jsonOutput = JsonConvert.DeserializeObject(trimmedOutput);
                return Ok(trimmedOutput);
            }
            catch (Exception jsonEx)
            {
                return StatusCode(500, new
                {
                    Error = "Failed to parse JSON from PowerShell output.",
                    Details = jsonEx.Message,
                    Output = trimmedOutput
                });
            }
        }
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { Error = "Unhandled exception", Details = ex.Message });
    }
    finally
    {
        if (System.IO.File.Exists(scriptPath))
            System.IO.File.Delete(scriptPath);
    }
}



    [HttpPost]
    [Route("reboot")]
    public IActionResult RemoteReboot([FromBody] string serverName)
    {
        string scriptPath = Path.Combine(Path.GetTempPath(), serverName+"RemoteReboot.ps1");
        string comment = "";

        RebootScript rs = new RebootScript(serverName);
        string scriptContent = rs.script;

        // Write the PowerShell script content to a temporary file
        System.IO.File.WriteAllText(scriptPath, scriptContent);

        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = $"-File \"{scriptPath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(startInfo))
            {
                var output = process.StandardOutput.ReadToEnd();
                var error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (!string.IsNullOrEmpty(error))
                {
                    return Ok(new { message = $"{serverName}: {error}" });
                }
                return Ok(new { message = output.Trim() });
            }
        }
        catch (Exception ex)
        {
            return Ok(new { message = $"{serverName}: Failed to reboot" });
        }
        finally
        {
            // Clean up the script file
            System.IO.File.Delete(scriptPath);
        }
    }

[HttpPost]
[Route("uptime")]
public IActionResult RemoteUptime([FromBody] string serverName)
{
    try
    {
        var uptime = GetRemoteUptime(serverName);
        return Ok(new { ServerName = serverName, Uptime = uptime });
    }
    catch (Exception ex)
    {
        return Ok(new { ServerName = serverName, Uptime = "Failed to retrieve uptime", Error = ex.Message });
    }
}

private string GetRemoteUptime(string serverName)
{
    var scope = new ManagementScope($@"\\{serverName}\root\cimv2");
    scope.Connect();

    var query = new ObjectQuery("SELECT LastBootUpTime FROM Win32_OperatingSystem");
    var searcher = new ManagementObjectSearcher(scope, query);

    foreach (ManagementObject os in searcher.Get())
    {
        var lastBootUp = ManagementDateTimeConverter.ToDateTime(os["LastBootUpTime"].ToString());
        var uptime = DateTime.Now - lastBootUp;
        return $"{(int)uptime.TotalDays} days {uptime.Hours} hrs {uptime.Minutes} min";
    }

    return "Unknown";
}



    [HttpPost]
    [Route("diskinfo")]
    public IActionResult RemoteDiskInfo([FromBody] string serverName)
    {
        string scriptPath = Path.Combine(Path.GetTempPath(),serverName+ "RemoteDiskInfo.ps1");

        string scriptContent = new RemoteDiskInfoScript(serverName).script;

        System.IO.File.WriteAllText(scriptPath, scriptContent);

        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = $"-File \"{scriptPath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(startInfo))
            {
                var output = process.StandardOutput.ReadToEnd();
                var error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (!string.IsNullOrEmpty(error))
                {
                    throw new InvalidOperationException($"Error executing PowerShell script: {error}");
                }
                return Ok(output);
                DiskInfoModel diskInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<DiskInfoModel>(output);
                diskInfo.UsedSpaceGB = (float.Parse(diskInfo.TotalCapacityGB) - float.Parse(diskInfo.FreeSpaceGB)).ToString();
                diskInfo.dateTime = DateAndTime.Now;
                diskInfo.Server = serverName;
                db.Add(diskInfo);
                db.SaveChanges();
                return Ok(diskInfo); // Return JSON output directly
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Failed to retrieve disk information for {serverName}. Error: {ex.Message}");
        }
        finally
        {
            System.IO.File.Delete(scriptPath);
        }
    }

[HttpPost]
[Route("diskcleanup")]
public IActionResult DiskCleanup([FromBody] string ServerNames)
{
    if (string.IsNullOrWhiteSpace(ServerNames))
    {
        return BadRequest("ServerNames input is required.");
    }

    string scriptPath = Path.Combine(Path.GetTempPath(), ServerNames+"DiskCleanup.ps1");
    string scriptContent = new DiskCleanup(ServerNames).Script;

    System.IO.File.WriteAllText(scriptPath, scriptContent);

    try
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "powershell.exe",
            Arguments = $"-File \"{scriptPath}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (var process = Process.Start(startInfo))
        {
            var output = process.StandardOutput.ReadToEnd();
            var error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (!string.IsNullOrEmpty(error))
            {
                throw new InvalidOperationException($"Error executing PowerShell script: {error}");
            }
            DiskCleanupParser d=new DiskCleanupParser();
            var parsedResults = d.ParseDiskCleanupOutput(output);
            return Ok(parsedResults);
        }
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"Failed to perform disk cleanup. Error: {ex.Message}");
    }
    finally
    {
        System.IO.File.Delete(scriptPath);
    }
}
    [HttpPost]
    [Route("compare-services")]
    public IActionResult CompareServices([FromBody] string serverName)
    {
        string scriptPath = Path.Combine(Path.GetTempPath(), serverName+"CompareServices.ps1");
        string outputFolder = @"C:\OutputFiles"; // Adjust this path as needed
        string scriptContent = new CompareServicesScript(serverName, outputFolder).script;

        System.IO.File.WriteAllText(scriptPath, scriptContent);

        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = $"-File \"{scriptPath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(startInfo))
            {
                var output = process.StandardOutput.ReadToEnd();
                var error = process.StandardError.ReadToEnd();
                Console.WriteLine("hey");

                if (!string.IsNullOrEmpty(error))
                {
                    throw new InvalidOperationException($"Error executing PowerShell script: {error}");
                }
                Console.WriteLine(output);
                string htmlReportPath = Path.Combine(outputFolder, $"Report_{serverName}.html");
                // Generate HTML report based on the output (JSON data)
                Console.WriteLine(htmlReportPath);
                GenerateHtmlReport(output, serverName, htmlReportPath);
                Process.Start(new ProcessStartInfo
                {
                    FileName = htmlReportPath,
                    UseShellExecute = true // Ensures the file opens with the default application
                });
                
                return Ok(new { message = "Check Done", htmlReportPath });
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Failed to retrieve service comparison for {serverName}. Error: {ex.Message}");
        }
        finally
        {
            System.IO.File.Delete(scriptPath);
        }
    }


    private void GenerateHtmlReport(string jsonData, string serverName, string htmlReportPath)
    {
        // Create HTML report based on JSON data
        Console.WriteLine("hello");
        var head = @"
            <Title>Service Compare</Title>
            <style>
            body { background-color:#FFFFFF; font-family:Tahoma; font-size:12pt; }
            td, th { border:1px solid black; border-collapse:collapse; }
            th { color:white; background-color:black; }
            table, tr, td, th { padding: 5px; }
            </style>";

        var body = $"<h2>Services Compare on {serverName}</h2><table><tr><th>Service Name</th><th>Display Name</th><th>Before</th><th>After</th></tr>";

        // Deserialize JSON data, assuming it is an array of objects
        dynamic data = JsonConvert.DeserializeObject(jsonData);
        foreach (var entry in data)
        {
            body += $"<tr><td>{entry.Name}</td><td>{entry.DisplayName}</td><td>({entry.State_Before}, {entry.StartMode_Before})</td><td>({entry.State_After}, {entry.StartMode_After})</td></tr>";
        }

        body += "</table>";

        // Write the HTML content to the specified file path
        System.IO.File.WriteAllText(htmlReportPath, $"<html><head>{head}</head><body>{body}</body></html>");
    }



    [HttpPost]
    [Route("cpu-memory-info")]
    public IActionResult GetCpuMemoryInfo([FromBody] string serverName)
    {
        string scriptPath = Path.Combine(Path.GetTempPath(), serverName+"GetSystemInfo.ps1");
        string scriptContent = new CpuMemoryInfoScript(serverName).script;

        System.IO.File.WriteAllText(scriptPath, scriptContent);

        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = $"-File \"{scriptPath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(startInfo))
            {
                var output = process.StandardOutput.ReadToEnd();
                var error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (!string.IsNullOrEmpty(error))
                {
                    throw new InvalidOperationException($"Error executing PowerShell script: {error}");
                }

                // Deserialize JSON output into SystemInfo object
                SystemInfo systemInfo = JsonConvert.DeserializeObject<SystemInfo>(output.Trim());

                if (systemInfo != null)
                {
                    systemInfo.ServerName = serverName;
                    systemInfo.dateTime = DateTime.Now;
                    foreach (var item in systemInfo.Top5CPUProcesses)
                    {
                        item.date = systemInfo.dateTime;
                        item.ServerName = serverName;

                    }
                    foreach (var item in systemInfo.Top5MemoryProcesses)
                    {
                        item.date = systemInfo.dateTime;
                        item.ServerName = serverName;

                    }
                    db.Add(systemInfo);
                    db.SaveChanges();
                    return Ok(new { message = "Check Done" });
                }

                return Ok(systemInfo);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, $"Failed to retrieve CPU and memory information for {serverName}. Error: {ex.Message}");
        }
        finally
        {
            System.IO.File.Delete(scriptPath);
        }
    }

    [HttpPost]
    [Route("connectiontest")]
    public IActionResult ConnectionTest([FromBody] string serverName)
    {
        string tempFilePath = Path.GetTempFileName() + ".ps1";

        try
        {
            string scriptContent = new GenerateHealthScript(serverName).script;
            System.IO.File.WriteAllText(tempFilePath, scriptContent);

            ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName = "powershell.exe",
                Arguments = $"-File \"{tempFilePath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(psi))
            {
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (!string.IsNullOrEmpty(error))
                {
                    return BadRequest(new { res = "No", error });
                }
                JsonNode? jsonData = JsonArray.Parse(output);
                if (jsonData != null)
                {
                    ConnectionTestModel res = new ConnectionTestModel(
                        (string)jsonData["ResolvedIP"], (string)jsonData["ResolvedName"], (string)jsonData["NSLookupStatus"], (string)jsonData["DNSResolutionStatus"], (string)jsonData["NSLookupOutput"], (string)jsonData["Server"], (string)jsonData["ConnectionTest"]
                    );
                    res.dateTime = DateTime.Now;
                    db.Add(res);
                    db.SaveChanges();
                    return Ok(new { message = "Check Done" });
                }
                return StatusCode(500, new { res = "No", error = "Server Error " });

            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { res = "No", error = ex.Message });
        }
        finally
        {
            if (System.IO.File.Exists(tempFilePath))
            {
                System.IO.File.Delete(tempFilePath);
            }
        }
    }
}


public class EventLogRequest
{
    public string ComputerName { get; set; }
    public IEnumerable<string> EventIDs { get; set; }
    public string LogName { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string EventType { get; set; }
    public string EventSource { get; set; }
}

public class DriveCleanupFlatResult
{
    public string Server { get; set; }
    public string Drive { get; set; }
    public List<string> Messages { get; set; } = new List<string>();
}


public class DriveCleanupResult
{
    public string Drive { get; set; }
    public List<string> Messages { get; set; } = new List<string>();
}


