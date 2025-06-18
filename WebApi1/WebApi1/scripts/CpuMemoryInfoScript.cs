class CpuMemoryInfoScript
{
    public string script;
    public CpuMemoryInfoScript(string serverName)
    {
        this.script=$@"
        function Get-SystemInfo {{
            param (
                [string]$Server
            )
            $SystemInfo = [PSCustomObject]@{{
                TotalCPUUsage = ""N/A""
                MemoryUsagePercentage = ""N/A""
                Top5CPUProcesses = @()
                Top5MemoryProcesses = @()
            }}
            try {{
                $totalCpuUsageCounter = Get-Counter ""\\$Server\Processor(_Total)\% Processor Time"" -ErrorAction SilentlyContinue
                if ($totalCpuUsageCounter) {{
                    $SystemInfo.TotalCPUUsage = [math]::Round($totalCpuUsageCounter.CounterSamples.CookedValue, 2)
                }}
                $logicalProcessors = (Get-WmiObject -ComputerName $Server Win32_ComputerSystem -ErrorAction Stop).NumberOfLogicalProcessors
                $memoryInfo = Get-WmiObject -ComputerName $Server Win32_OperatingSystem -ErrorAction Stop
                if ($memoryInfo) {{
                    $totalMemory = $memoryInfo.TotalVisibleMemorySize
                    $freeMemory = $memoryInfo.FreePhysicalMemory
                    if ($totalMemory -ne 0) {{
                        $SystemInfo.MemoryUsagePercentage = [math]::Round(($totalMemory - $freeMemory) / $totalMemory * 100, 2)
                    }}
                }}
                $Top5cpuProcesses = Get-WmiObject -ComputerName $Server -Class Win32_PerfFormattedData_PerfProc_Process -ErrorAction Stop |
                                Sort-Object -Property PercentProcessorTime -Descending |
                                Where-Object {{ $_.Name -ne ""_Total"" -and $_.IDProcess -ne 0 }} |
                                Select-Object -First 5 |
                                ForEach-Object {{
                                    $processName = $_.Name
                                    $cpuUsage = [math]::Round($_.PercentProcessorTime / $logicalProcessors, 2)
                                    [PSCustomObject]@{{
                                        PID = $_.IDProcess
                                        Name = $processName
                                        CPUUsagePercentage = $cpuUsage
                                    }}
                                }}
                $SystemInfo.Top5CPUProcesses = $Top5cpuProcesses
                $Top5memoryProcesses = Get-WmiObject -ComputerName $Server -Class Win32_PerfFormattedData_PerfProc_Process -ErrorAction Stop |
                                    Sort-Object -Property WorkingSetPrivate -Descending |
                                    Where-Object {{ $_.Name -ne ""_Total"" -and $_.IDProcess -ne 0 }} |
                                    Select-Object -First 5 |
                                    ForEach-Object {{
                                        $processName = $_.Name
                                        $memoryUsage = [math]::Round($_.WorkingSetPrivate / 1MB, 2)
                                        [PSCustomObject]@{{
                                            PID = $_.IDProcess
                                            Name = $processName
                                            MemoryUsageMB = $memoryUsage
                                        }}
                                    }}
                $SystemInfo.Top5MemoryProcesses = $Top5memoryProcesses
            }} catch {{
                # Suppress error
            }}
            return $SystemInfo | ConvertTo-Json
        }}
        Get-SystemInfo -Server '{serverName}'
    ";
    }
    
}