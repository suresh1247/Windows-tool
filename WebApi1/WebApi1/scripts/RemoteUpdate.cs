class RemoteUpdate
{
    public string script;
    public RemoteUpdate(string ServerName)
    {
        this.script = $@"
            $server = '{ServerName}'
            try {{
                $os = Get-WmiObject -Class Win32_OperatingSystem -ComputerName $server -ErrorAction Stop
                $uptime = (Get-Date) - $os.ConvertToDateTime($os.LastBootUpTime)
                $uptimeString = ""{{0}} days, {{1}} hours, {{2}} minutes"" -f $uptime.Days, $uptime.Hours, $uptime.Minutes
                
                $result = @{{ ServerName = $server; Uptime = $uptimeString }}
                $result | ConvertTo-Json
            }} catch {{
                $result = @{{ ServerName = $server; Uptime = 'Failed to retrieve uptime' }}
                $result | ConvertTo-Json
            }}
        ";
    }
    
}