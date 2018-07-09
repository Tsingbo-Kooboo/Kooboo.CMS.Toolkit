@echo off & setlocal EnableDelayedExpansion
set MsBuildPath="C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\MsBuild.exe"
if not exist %MsBuildPath% (
  echo %MsBuildPath%
  echo MSBuild path not found,publish failed!
  pause
  exit
)
rd Released /Q /S
md Release
md Release\Kooboo.CMS.Toolkit\lib\
md Release\Kooboo.CMS.Membership.China\lib\
md Release\Kooboo.CMS.Toolkit.Controls\lib\
md Release\Kooboo.CMS.Content.UserKeyGenerator.Chinese\lib\
md packages
del *.log /Q /S

copy "..\Kooboo.CMS.Toolkit\Kooboo.CMS.Toolkit.nuspec" "..\Publish\Release\Kooboo.CMS.Toolkit\Kooboo.CMS.Toolkit.nuspec"
copy "..\Kooboo.CMS.Toolkit.Controls\Kooboo.CMS.Toolkit.Controls.nuspec" "..\Publish\Release\Kooboo.CMS.Toolkit.Controls\Kooboo.CMS.Toolkit.Controls.nuspec"
copy "..\Kooboo.CMS.Content.UserKeyGenerator.Chinese\Kooboo.CMS.Content.UserKeyGenerator.Chinese.nuspec" "..\Publish\Release\Kooboo.CMS.Content.UserKeyGenerator.Chinese\Kooboo.CMS.Content.UserKeyGenerator.Chinese.nuspec"
copy "..\Kooboo.CMS.Membership.China\Kooboo.CMS.Membership.China.nuspec" "..\Publish\Release\Kooboo.CMS.Membership.China\Kooboo.CMS.Membership.China.nuspec"
call update_version.vbs

cd..

%MsBuildPath% Kooboo.Toolkits.sln /t:Rebuild /l:FileLogger,Microsoft.Build.Engine;logfile=Publish\Publish.log; /p:VisualStudioVersion=15.0;Configuration=Release

%MsBuildPath% Kooboo.CMS.Toolkit\Kooboo.CMS.Toolkit.csproj /t:ResolveReferences;Compile /p:Configuration=Release;
%MsBuildPath% Kooboo.CMS.Toolkit.Controls\Kooboo.CMS.Toolkit.Controls.csproj /t:ResolveReferences;Compile /p:Configuration=Release;

cd Kooboo.CMS.Toolkit
copy "bin\Release\Kooboo.CMS.Toolkit.dll" "..\Publish\Release\Kooboo.CMS.Toolkit\lib\Kooboo.CMS.Toolkit.dll"
cd ..

cd Kooboo.CMS.Toolkit.Controls
copy "bin\Release\Kooboo.CMS.Toolkit.Controls.dll" "..\Publish\Release\Kooboo.CMS.Toolkit.Controls\lib\Kooboo.CMS.Toolkit.Controls.dll"
cd ..

cd Kooboo.CMS.Content.UserKeyGenerator.Chinese
copy "bin\Release\Kooboo.CMS.Content.UserKeyGenerator.Chinese.dll" "..\Publish\Release\Kooboo.CMS.Content.UserKeyGenerator.Chinese\lib\Kooboo.CMS.Content.UserKeyGenerator.Chinese.dll"
cd ..

cd Kooboo.CMS.Membership.China
copy "bin\Release\Kooboo.CMS.Membership.China.dll" "..\Publish\Release\Kooboo.CMS.Membership.China\lib\Kooboo.CMS.Membership.China.dll"
cd ..
cd Publish

nuget pack Release\Kooboo.CMS.Toolkit\Kooboo.CMS.Toolkit.nuspec -OutputDirectory packages

nuget pack Release\Kooboo.CMS.Toolkit.Controls\Kooboo.CMS.Toolkit.Controls.nuspec -OutputDirectory packages

nuget pack Release\Kooboo.CMS.Content.UserKeyGenerator.Chinese\Kooboo.CMS.Content.UserKeyGenerator.Chinese.nuspec -OutputDirectory packages

nuget pack Release\Kooboo.CMS.Membership.China\Kooboo.CMS.Membership.China.nuspec -OutputDirectory packages
