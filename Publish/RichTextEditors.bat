call update_version.vbs

nuget restore ..\Kooboo.Toolkits-Release.sln
"C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe" ..\Kooboo.Toolkits-Release.sln /t:rebuild /l:FileLogger,Microsoft.Build.Engine;logfile=Publish.log; /p:VisualStudioVersion=14.0 /p:Configuration=Release

del *.nupkg

nuget pack ..\Kooboo.CMS.Toolkit.Controls.RichTextEditors\Kooboo.CMS.Toolkit.Controls.RichTextEditors.csproj -Prop Configuration=Release

@pause