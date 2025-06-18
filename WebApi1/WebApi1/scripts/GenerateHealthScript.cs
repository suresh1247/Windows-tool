class GenerateHealthScript
{
    public string script;
    public GenerateHealthScript(String serverName)
    {
        this.script=$@"
$result = @{{
    Server = '{serverName}'
    ConnectionTest = 'Unknown'
    DNSResolutionStatus = 'Failure'
    ResolvedIP = 'N/A'
    NSLookupStatus = 'Failure'
    ResolvedName = 'N/A'
    NSLookupOutput = 'N/A'
}}
 
# Connection Test
try {{
    $pingResult = Test-Connection -ComputerName '{serverName}' -Count 1 -Quiet
    $result.ConnectionTest = if ($pingResult) {{ 'Reachable' }} else {{ 'Unreachable' }}
}} catch {{
    $result.ConnectionTest = 'Error - ' + $_.Exception.Message
}}
 
# DNS Resolution
try {{
    $ipAddress = [System.Net.Dns]::GetHostAddresses('{serverName}') | Where-Object {{ $_.AddressFamily -eq [System.Net.Sockets.AddressFamily]::InterNetwork }}
    if ($ipAddress) {{
        $result.DNSResolutionStatus = 'Success'
        $result.ResolvedIP = $ipAddress[0].IPAddressToString
    }}
}} catch {{
    $result.DNSResolutionStatus = 'Failure'
    $result.ResolvedIP = 'N/A'
}}
 
# NS Lookup
try {{
    $nslookupResult = nslookup '{serverName}' 2>&1
    $nslookupText = $nslookupResult -join '`r`n'
    $nameMatch = $nslookupText -match 'Name:\s+(.+)'
    if ($nameMatch) {{
        $result.NSLookupStatus = 'Success'
        $result.ResolvedName = $matches[1]
    }}
    $result.NSLookupOutput = $nslookupText
}} catch {{
    $result.NSLookupStatus = 'Error'
    $result.ResolvedName = 'N/A'
    $result.NSLookupOutput = $_.Exception.Message
}}
 
$result | ConvertTo-Json
";
    }
    
}