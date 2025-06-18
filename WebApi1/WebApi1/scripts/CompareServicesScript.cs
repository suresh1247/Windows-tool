using System;

public class CompareServicesScript
{
    public string script;

    public CompareServicesScript(string serverName, string outputFolder)
    {
        this.script = $@"
function Compare-Services {{
    param (
        [string]$Computer,
        [string]$OutputFolderPath
    )

    $XmlNameBefore = ""$Computer-Before.xml""
    $XmlNameAfter = ""$Computer-After.xml""
    $BeforeExists = Test-Path ""$OutputFolderPath\$XmlNameBefore""
    $AfterExists = Test-Path ""$OutputFolderPath\$XmlNameAfter""

    if (-not $BeforeExists) {{
        Get-WmiObject win32_service -ComputerName $Computer -ErrorAction Ignore |
            Select SystemName, Name, DisplayName, State, StartMode |
            Sort StartMode |
            Export-Clixml ""$OutputFolderPath\$XmlNameBefore""
    }}

    if ($BeforeExists -or $AfterExists) {{
        Get-WmiObject win32_service -ComputerName $Computer -ErrorAction Ignore |
            Select SystemName, Name, DisplayName, State, StartMode |
            Sort StartMode |
            Export-Clixml ""$OutputFolderPath\$XmlNameAfter""
    }}

    if ((Test-Path ""$OutputFolderPath\$XmlNameBefore"") -and (Test-Path ""$OutputFolderPath\$XmlNameAfter"")) {{
        $xmlBefore = Import-Clixml -Path ""$OutputFolderPath\$XmlNameBefore""
        $xmlAfter = Import-Clixml -Path ""$OutputFolderPath\$XmlNameAfter""

        $obj = @()

        foreach ($line1 in $xmlBefore) {{
            foreach ($line2 in $xmlAfter) {{
                if ($line1.Name -eq $line2.Name) {{
                    if (($line1.StartMode -ne $line2.StartMode) -or ($line1.State -ne $line2.State)) {{
                        $obj += [PSCustomObject]@{{
                            SystemName = $line1.SystemName
                            Name = $line1.Name
                            DisplayName = $line1.DisplayName
                            StartMode_Before = $line1.StartMode
                            StartMode_After = $line2.StartMode
                            State_Before = $line1.State
                            State_After = $line2.State
                        }}
                    }}
                }}
            }}
        }}

        $obj | ConvertTo-Json -Depth 5
    }}
}}

Compare-Services -Computer '{serverName}' -OutputFolderPath '{outputFolder}'
";
    }
}
