using System;

public class EventIDQuery
{
    public string Script { get; }

    public EventIDQuery(string computerName, int[] eventIDs, string logName, DateTime startDate, DateTime endDate, string eventType, string eventSource)
    {
        // Escape PowerShell strings
        string psComputerName = EscapePowerShellString(computerName);
        string psLogName = EscapePowerShellString(logName);
        string psEventType = EscapePowerShellString(eventType);
        string psEventSource = EscapePowerShellString(eventSource);

        // Format dates
        string psStartDate = startDate.ToString("o");
        string psEndDate = endDate.ToString("o");

        // Prepare EventID array
        string eventIDFilter = "";
        if (eventIDs != null && eventIDs.Length > 0)
        {
            string joinedIDs = string.Join(", ", eventIDs);
            eventIDFilter = $"@({joinedIDs})";
        }

        Script = $@"
$ErrorActionPreference = 'Stop'
$computer = '{psComputerName}'
$logName = '{psLogName}'
$startDate = [datetime]'{psStartDate}'
$endDate = [datetime]'{psEndDate}'
$eventType = '{psEventType}'
$eventSource = '{psEventSource}'

try {{
    $filterHash = @{{ LogName = $logName; StartTime = $startDate; EndTime = $endDate }}
    {(string.IsNullOrEmpty(eventIDFilter) ? "" : "$filterHash['Id'] = " + eventIDFilter)}

    $events = Get-WinEvent -ComputerName $computer -FilterHashtable $filterHash

    if ($eventType -ne '') {{
        $events = $events | Where-Object {{ $_.LevelDisplayName -eq $eventType }}
    }}

    if ($eventSource -ne '') {{
        $events = $events | Where-Object {{ $_.ProviderName -eq $eventSource }}
    }}

    $results = @()
    foreach ($e in $events) {{
        $results += [PSCustomObject]@{{
            TimeCreated = $e.TimeCreated
            Id = $e.Id
            LevelDisplayName = $e.LevelDisplayName
            ProviderName = $e.ProviderName
            Message = $e.Message
            MachineName = $e.MachineName
        }}
    }}

    if ($results.Count -eq 0) {{
        Write-Output '[]'
    }} else {{
        $results | ConvertTo-Json -Depth 3
    }}
}}
catch {{
    Write-Error $_.Exception.Message
    exit 1
}}

exit 0
";
    }

    private string EscapePowerShellString(string input)
    {
        return string.IsNullOrEmpty(input) ? "" : input.Replace("'", "''");
    }
}
