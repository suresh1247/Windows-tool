class DiskInfo
{
    public string script;
    public DiskInfo(string ServerNames)
    {
     this.script= @$"
        $servers = '{ServerNames}' -split ','
        $logContent = @()

        foreach ($server in $servers) {{
            $logContent += 'Attempting to clean up on server ' + $server + ':`r`n'
            
            try {{
                $pingResult = Test-Connection -ComputerName $server -Count 1 -Quiet
                if (-not $pingResult) {{
                    throw 'Unable to reach server ' + $server + '. Skipping cleanup.'
                }}

                $before = Invoke-Command -ComputerName $server -ScriptBlock {{
                    Get-WmiObject Win32_LogicalDisk | Where-Object {{ $_.DeviceID -eq 'C:' }} | Select-Object @{{
                        Name = 'FreeSpace'
                        Expression = {{ '{0:N1}' -f ($_.FreeSpace / 1GB) }}
                    }}
                }}
                $beforeFreeSpace = $before.FreeSpace
                $logContent += ' - Free space before cleanup: ' + $beforeFreeSpace + ' GB`r`n'

                Invoke-Command -ComputerName $server -ScriptBlock {{
                    Stop-Service -Name wuauserv -Force -ErrorAction SilentlyContinue
                }}

                $softwareDistPath = 'C:\Windows\SoftwareDistribution'
                if (Test-Path -Path $softwareDistPath) {{
                    Invoke-Command -ComputerName $server -ScriptBlock {{
                        Get-ChildItem 'C:\Windows\SoftwareDistribution\*' -Recurse -Force -ErrorAction SilentlyContinue | Remove-Item -Force -Recurse -ErrorAction SilentlyContinue
                    }}
                }}

                $windowsTempPath = 'C:\Windows\Temp'
                if (Test-Path -Path $windowsTempPath) {{
                    Invoke-Command -ComputerName $server -ScriptBlock {{
                        Get-ChildItem 'C:\Windows\Temp\*' -Recurse -Force -ErrorAction SilentlyContinue | Remove-Item -Force -Recurse -ErrorAction SilentlyContinue
                    }}
                }}

                $ccmCachePath = 'C:\Windows\CCMCache'
                if (Test-Path -Path $ccmCachePath) {{
                    Invoke-Command -ComputerName $server -ScriptBlock {{
                        Get-ChildItem 'C:\Windows\CCMCache\*' -Recurse -Force -ErrorAction SilentlyContinue | Remove-Item -Force -Recurse -ErrorAction SilentlyContinue
                    }}
                }}

                Invoke-Command -ComputerName $server -ScriptBlock {{
                    $shell = New-Object -ComObject Shell.Application
                    $bin = $shell.Namespace(0xA)
                    $bin.Items() | ForEach-Object {{ Remove-Item $_.Path -Force -ErrorAction SilentlyContinue }}
                }}

                Invoke-Command -ComputerName $server -ScriptBlock {{
                    Start-Service -Name wuauserv -ErrorAction SilentlyContinue
                }}

                $after = Invoke-Command -ComputerName $server -ScriptBlock {{
                    Get-WmiObject Win32_LogicalDisk | Where-Object {{ $_.DeviceID -eq 'C:' }} | Select-Object @{{
                        Name = 'FreeSpace'
                        Expression = {{ '{0:N1}' -f ($_.FreeSpace / 1GB) }}
                    }}
                }}
                $afterFreeSpace = $after.FreeSpace
                $logContent += ' - Free space after cleanup: ' + $afterFreeSpace + ' GB`r`n'
            }} catch {{
                $logContent += 'Failed to clean up on server ' + $server + ': ' + $_ + '`r`n'
            }}
        }}

        $logContent | ConvertTo-Json
        ";   
    }
}