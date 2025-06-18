class RemoteDiskInfoScript
{
    public string script;
    public RemoteDiskInfoScript(string serverName)
    {
        this.script = $@"
            $server = '{serverName}'
            $Disks = @()
            try {{
                $Disks = Get-WmiObject win32_logicaldisk -ComputerName $server -Filter ""Drivetype=3"" -ErrorAction Stop |
                    Select-Object @{{
                        Name = ""ServerName""
                        Expression = {{ $_.SystemName }}
                    }},
                    @{{
                        Name = ""DriveLetter""
                        Expression = {{ $_.DeviceID }}
                    }},
                    @{{
                        Name = ""TotalCapacityGB""
                        Expression = {{ ""{{0:N1}}"" -f ($_.Size / 1GB) }}
                    }},
                    @{{
                        Name = ""UsedSpaceGB""
                        Expression = {{ [math]::Round(($_.Size - $_.FreeSpace) / 1GB, 2) }}
                    }},
                    @{{
                        Name = ""FreeSpaceGB""
                        Expression = {{ ""{{0:N1}}"" -f ($_.FreeSpace / 1GB) }}
                    }},
                    @{{
                        Name = ""FreeSpacePercent""
                        Expression = {{ ""{{0:P0}}"" -f ($_.FreeSpace / $_.Size) }}
                    }}
            }} catch {{
                $Disks = @(
                    [PSCustomObject]@{{
                        'ServerName' = $server
                        'DriveLetter' = 'N/A'
                        'TotalCapacityGB' = 'N/A'
                        'UsedSpaceGB' = 'N/A'
                        'FreeSpaceGB' = 'N/A'
                        'FreeSpacePercent' = 'N/A'
                    }}
                )
            }}

            # Ensure the output is always an array
            if ($Disks -isnot [System.Collections.IEnumerable] -or $Disks -is [string]) {{
                $Disks = @($Disks)
            }}

            $Disks | ConvertTo-Json -Depth 3
        ";
    }
}
