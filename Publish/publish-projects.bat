set MSBuild="C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\MSBuild.exe"
set publishDir="../PublishedPackages"
if not exist %publishDir% (
  md %publishDir%
)

del *.log /Q /S

call update_version.vbs

%MSBuild% ..\Kooboo.Toolkits.sln /t:rebuild /l:FileLogger,Microsoft.Build.Engine;logfile=.\Publish.log;

%MSBuild% ..\Kooboo.CMS.Toolkit\Kooboo.CMS.Toolkit.csproj /t:ResolveReferences;Compile /p:Configuration=Release logfile=.\Publish.log;
%MSBuild% ..\Kooboo.CMS.Toolkit.Controls\Kooboo.CMS.Toolkit.Controls.csproj /t:ResolveReferences;Compile /p:Configuration=Release logfile=.\Publish.log;

nuget pack ..\Kooboo.CMS.Toolkit\Kooboo.CMS.Toolkit.csproj -OutputDirectory %publishDir%

nuget pack ..\Kooboo.CMS.Toolkit.Controls\Kooboo.CMS.Toolkit.Controls.csproj -OutputDirectory %publishDir%

nuget pack ..\Kooboo.CMS.Content.UserKeyGenerator.Chinese\Kooboo.CMS.Content.UserKeyGenerator.Chinese.csproj -OutputDirectory %publishDir%

nuget pack ..\Kooboo.CMS.Membership.China\Kooboo.CMS.Membership.China.csproj -OutputDirectory %publishDir%

pause