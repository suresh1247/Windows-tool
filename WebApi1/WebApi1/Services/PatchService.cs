using System.Management;

public class PatchService
{
    public List<PatchResult> GetLast5Patches(string serverName)
    {
        var results = new List<PatchResult>();
        try
        {
            var searcher = new ManagementObjectSearcher(
                $"\\\\{serverName}\\root\\cimv2",
                "SELECT * FROM Win32_QuickFixEngineering");

            var patches = searcher.Get()
                .Cast<ManagementObject>()
                .Select(mo => new
                {
                    KB = mo["HotFixID"]?.ToString(),
                    DateStr = mo["InstalledOn"]?.ToString(),
                    Object = mo
                })
                .Select(patch => new
                {
                    patch.KB,
                    InstalledOn = patch.DateStr,
                    patch.Object
                })
                .OrderByDescending(p => p.InstalledOn)
                .Take(5);

            foreach (var patch in patches)
            {
                results.Add(new PatchResult
                {
                    ServerName = serverName,
                    KBNumber = patch.KB,
                    InstalledOn = patch.InstalledOn.ToString(),
                    Status = "Installed"
                });
            }
            return results;
        }
        catch (Exception ex)
        {
            results.Add(new PatchResult
            {
                ServerName = serverName,
                KBNumber = null,
                InstalledOn = null,
                Status = $"Error: {ex.Message}"
            });
        }
        return results;

}

private DateTime? TryParseDate(string dateStr)
{
    if (DateTime.TryParse(dateStr, out var date))
    {
        return date;
    }

    return null; // Return null if parsing fails
}

    public PatchResult CheckPatch(string serverName, string kbNumber)
    {
        try
        {
            var searcher = new ManagementObjectSearcher(
                $"\\\\{serverName}\\root\\cimv2",
                $"SELECT * FROM Win32_QuickFixEngineering WHERE HotFixID = '{kbNumber}'");

            var result = searcher.Get().Cast<ManagementObject>().FirstOrDefault();
            
            if (result != null)
            {
                Console.WriteLine(result.ToString());
                return new PatchResult
                {
                    ServerName = serverName,
                    KBNumber = kbNumber,
                    InstalledOn = result["InstalledOn"]?.ToString(),
                    Status = "Installed"
                };
            }

            return new PatchResult
            {
                ServerName = serverName,
                KBNumber = kbNumber,
                InstalledOn = null,
                Status = "Not Installed"
            };
        }
        catch (Exception ex)
        {
            return new PatchResult
            {
                ServerName = serverName,
                KBNumber = kbNumber,
                InstalledOn = null,
                Status = $"Error: {ex.Message}"
            };
        
        }
    }
}
