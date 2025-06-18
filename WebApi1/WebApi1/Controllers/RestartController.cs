using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Management;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class RestartController : ControllerBase
{
    [HttpPost("restart")]
    public IActionResult RestartService([FromBody] ServiceRestartRequest request)
    {
        StringBuilder log = new();
        Console.WriteLine(request.ComputerName);


        try
        {
            string computer = request.ComputerName;
            string serviceName = request.ServiceName;

            // Connect to the remote machine
            var scope = new ManagementScope($"\\\\{computer}\\root\\cimv2");
            scope.Connect();

            // Get the service
            var serviceQuery = new SelectQuery($"SELECT * FROM Win32_Service WHERE Name = '{serviceName}'");
            var serviceSearcher = new ManagementObjectSearcher(scope, serviceQuery);
            var service = serviceSearcher.Get().Cast<ManagementObject>().FirstOrDefault();

            if (service == null)
                return NotFound($"Service {serviceName} not found on {computer}.");

            // Stop the service
            log.AppendLine($"Stopping service '{serviceName}'...");
            var stopResult = service.InvokeMethod("StopService", null);
            log.AppendLine($"Stop result: {stopResult}");

            // Wait briefly
            Thread.Sleep(3000);

            // Start the service
            log.AppendLine($"Starting service '{serviceName}'...");
            var startResult = service.InvokeMethod("StartService", null);
            log.AppendLine($"Start result: {startResult}");
            Console.WriteLine(startResult);
            return Ok(new
            {
                Message = $"Service '{serviceName}' restarted on {computer}.",
                Log = log.ToString()
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Failed to restart service: {ex.Message}");
        }
    }
}

public class ServiceRestartRequest
{
    public string ComputerName { get; set; }
    public string ServiceName { get; set; }
}
