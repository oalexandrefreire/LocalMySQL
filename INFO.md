### unlock files - powershell
Get-ChildItem -Path "C:\caminho\do\projeto" -Recurse | Unblock-File

### remove bin and obj folders
Get-ChildItem -Path "C:\caminho\do\projeto" -Include bin,obj -Recurse -Directory | Remove-Item -Recurse -Force
