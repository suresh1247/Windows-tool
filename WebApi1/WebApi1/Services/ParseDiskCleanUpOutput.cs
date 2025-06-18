using System;
using System.Collections.Generic;

namespace WebApi1.Services
{
    public class DiskCleanupParser
    {
        public List<DriveCleanupFlatResult> ParseDiskCleanupOutput(string output)
        {
            var results = new List<DriveCleanupFlatResult>();

            var lines = output.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            string currentServer = null;
            DriveCleanupFlatResult currentDrive = null;

            foreach (var line in lines)
            {
                if (line.StartsWith("--- Cleaning Disk on server: "))
                {
                    currentServer = line.Substring("--- Cleaning Disk on server: ".Length)
                                        .Replace("---", "")
                                        .Replace("\r", "")
                                        .Replace("\n", "")
                                        .Trim();
                }
                else if (line.StartsWith("Cleaning drive: "))
                {
                    if (currentDrive != null)
                    {
                        results.Add(currentDrive);
                    }

                    currentDrive = new DriveCleanupFlatResult
                    {
                        Server = currentServer,
                        Drive = line.Substring("Cleaning drive: ".Length).Trim(),
                        Messages = new List<string>()
                    };
                }
                else
                {
                    if (currentDrive != null)
                    {
                        var cleanedMessage = line.Replace("\r", "").Replace("\n", "").Trim();
                        if (!string.IsNullOrWhiteSpace(cleanedMessage))
                        {
                            currentDrive.Messages.Add(cleanedMessage);
                        }
                    }
                }
            }

            if (currentDrive != null)
            {
                results.Add(currentDrive);
            }

            return results;
        }
    }

    public class DriveCleanupFlatResult
    {
        public string Server { get; set; }
        public string Drive { get; set; }
        public List<string> Messages { get; set; }
    }
}
