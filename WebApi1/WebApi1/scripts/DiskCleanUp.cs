public class DiskCleanup
{
    public string Script { get; }

    public DiskCleanup(string serverName)
    {
        // The script will run on the remote server directly (so no remoting inside)
        // It will clean temp and recycle bin for all fixed drives
        this.Script = $@"
$logContent = @()
$logContent += '--- Cleaning Disk on server: {serverName} ---`r`n'

try {{
    # Get all fixed drives
    $drives = Get-PSDrive -PSProvider FileSystem | Where-Object {{ $_.Free -ne $null }}

    foreach ($drive in $drives) {{
        $driveRoot = $drive.Root
        $logContent += ""Cleaning drive: $driveRoot`r`n""

        # Clean TEMP folder on the drive (if exists)
        $tempPath = Join-Path $driveRoot 'Temp'
        if (Test-Path $tempPath) {{
            $tempFiles = Get-ChildItem -Path $tempPath -Recurse -Force -ErrorAction SilentlyContinue
            $deletedTempCount = 0
            foreach ($file in $tempFiles) {{
                try {{
                    Remove-Item $file.FullName -Force -Recurse -ErrorAction SilentlyContinue
                    $deletedTempCount++
                }} catch {{}}
            }}
            $logContent += ""Deleted $deletedTempCount temp files from $tempPath`r`n""
        }} else {{
            $logContent += ""Temp folder does not exist at $tempPath`r`n""
        }}

        # Clean Recycle Bin folder for the drive
        $recycleBinPath = Join-Path $driveRoot '$Recycle.Bin'
        if (Test-Path $recycleBinPath) {{
            try {{
                # Remove all contents of recycle bin folder
                Get-ChildItem -Path $recycleBinPath -Force -Recurse -ErrorAction SilentlyContinue | Remove-Item -Force -Recurse -ErrorAction SilentlyContinue
                $logContent += 'Recycle Bin cleared successfully on drive ' + $driveRoot + '`r`n'
            }} catch {{
                $logContent += 'Failed to clear Recycle Bin on drive ' + $driveRoot + ': ' + $_.Exception.Message + '`r`n'
            }}
        }} else {{
            $logContent += ""Recycle Bin folder does not exist at $recycleBinPath`r`n""
        }}
    }}
}} catch {{
    $logContent += 'Exception during cleanup: ' + $_.Exception.Message + '`r`n'
}}

# Output log
$logContent -join ""`r`n""
";
    }
}
