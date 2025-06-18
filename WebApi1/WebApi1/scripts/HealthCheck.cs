class HealthCheck
{
    public string script;
    public HealthCheck(string serverName)
    {
        this.script = $@"
            $serverHealth = @()
           
            $cpuUsage = Get-WmiObject Win32_Processor -ComputerName {serverName} | Measure-Object -Property LoadPercentage -Average | Select-Object -ExpandProperty Average
            $memoryInfo = Get-WmiObject Win32_OperatingSystem -ComputerName {serverName}
            $memoryUsage = ((($memoryInfo.TotalVisibleMemorySize - $memoryInfo.FreePhysicalMemory) / $memoryInfo.TotalVisibleMemorySize) * 100)
            $diskInfo = Get-WmiObject Win32_LogicalDisk -ComputerName {serverName} | Where-Object {{$_.DriveType -eq 3}}
            $diskUsage = (($diskInfo.Size - $diskInfo.FreeSpace) / $diskInfo.Size) * 100
            $pingResult = Test-Connection -ComputerName {serverName} -Count 1 -Quiet
            $pingStatus = if ($pingResult) {{'Success'}} else {{'Failure'}}
            $dnsResolution = [System.Net.Dns]::GetHostAddresses('{serverName}') | Where-Object {{ $_.AddressFamily -eq [System.Net.Sockets.AddressFamily]::InterNetwork }} | Select-Object -ExpandProperty IPAddressToString
           
            $serverHealth += [PSCustomObject]@{{
                ServerName = '{serverName}'
                CPUUsage = [math]::Round($cpuUsage, 2)
                MemoryUsage = [math]::Round($memoryUsage, 2)
                DiskUsage = [math]::Round($diskUsage, 2)
                PingStatus = $pingStatus
                DNSResolution = $dnsResolution
            }}
           
            $serverHealth | ConvertTo-Json
        ";
    }

}