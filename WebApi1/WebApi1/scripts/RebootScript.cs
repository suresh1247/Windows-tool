class RebootScript
{
    public string script;

    public RebootScript(string serverName)
    {
        this.script = $@"
            $server = '{serverName}'

            try {{
                Restart-Computer -ComputerName $server -Force -ErrorAction Stop
                Write-Output ""${{server}}: Reboot completed successfully""
            }} catch {{
                Write-Output ""${{server}}: Failed to reboot - $($_.Exception.Message)""
            }}
        ";
    }
}
