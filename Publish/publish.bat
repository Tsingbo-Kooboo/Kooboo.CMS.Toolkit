rd Released /Q /S
md Release
md Release\Kooboo.CMS.Toolkit\lib\
md Release\Kooboo.CMS.Membership.China\lib\
md Release\Kooboo.CMS.Toolkit.Controls\lib\
md Release\Kooboo.Core\lib\
md Release\Kooboo.ModuleDevelopment.Binaries\content\
md Release\Kooboo.CMS.Content.UserKeyGenerator.Chinese\lib\
md packages
del *.log /Q /S

copy "..\Kooboo.CMS.Toolkit\Kooboo.CMS.Toolkit.nuspec" "..\Publish\Release\Kooboo.CMS.Toolkit\Kooboo.CMS.Toolkit.nuspec"
copy "..\Kooboo.CMS.Toolkit.Controls\Kooboo.CMS.Toolkit.Controls.nuspec" "..\Publish\Release\Kooboo.CMS.Toolkit.Controls\Kooboo.CMS.Toolkit.Controls.nuspec"
copy "..\lib\Kooboo.Core.nuspec" "..\Publish\Release\Kooboo.Core\Kooboo.Core.nuspec"
copy "..\Kooboo.ModuleDevelopment.Binaries\Kooboo.ModuleDevelopment.Binaries.nuspec" "..\Publish\Release\Kooboo.ModuleDevelopment.Binaries\Kooboo.ModuleDevelopment.Binaries.nuspec"
copy "..\Kooboo.CMS.Content.UserKeyGenerator.Chinese\Kooboo.CMS.Content.UserKeyGenerator.Chinese.nuspec" "..\Publish\Release\Kooboo.CMS.Content.UserKeyGenerator.Chinese\Kooboo.CMS.Content.UserKeyGenerator.Chinese.nuspec"
copy "..\Kooboo.CMS.Membership.China\Kooboo.CMS.Membership.China.nuspec" "..\Publish\Release\Kooboo.CMS.Membership.China\Kooboo.CMS.Membership.China.nuspec"
call update_version.vbs

cd..

"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild" Kooboo.Toolkits.sln /t:rebuild /l:FileLogger,Microsoft.Build.Engine;logfile=Publish\Publish.log;

"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild" Kooboo.CMS.Toolkit\Kooboo.CMS.Toolkit.csproj /t:ResolveReferences;Compile /p:Configuration=Release logfile=Publish\Publish.log;
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild" Kooboo.CMS.Toolkit.Controls\Kooboo.CMS.Toolkit.Controls.csproj /t:ResolveReferences;Compile /p:Configuration=Release logfile=Publish\Publish.log;

cd Kooboo.CMS.Toolkit
copy "bin\Release\Kooboo.CMS.Toolkit.dll" "..\Publish\Release\Kooboo.CMS.Toolkit\lib\Kooboo.CMS.Toolkit.dll"
cd ..

cd Kooboo.CMS.Toolkit.Controls
copy "bin\Release\Kooboo.CMS.Toolkit.Controls.dll" "..\Publish\Release\Kooboo.CMS.Toolkit.Controls\lib\Kooboo.CMS.Toolkit.Controls.dll"
cd ..

cd lib 
copy "Kooboo.dll" "..\Publish\Release\Kooboo.Core\lib\Kooboo.dll"
copy "Kooboo.*.dll" "..\Publish\Release\Kooboo.Core\lib\"
cd ..

cd Kooboo.ModuleDevelopment.Binaries
xcopy content\*.* ..\Publish\Release\Kooboo.ModuleDevelopment.Binaries\content\*.* /S /E /Y /H
cd ..

cd Kooboo.CMS.Content.UserKeyGenerator.Chinese
copy "bin\Release\Kooboo.CMS.Content.UserKeyGenerator.Chinese.dll" "..\Publish\Release\Kooboo.CMS.Content.UserKeyGenerator.Chinese\lib\Kooboo.CMS.Content.UserKeyGenerator.Chinese.dll"
cd ..

cd Kooboo.CMS.Membership.China
copy "bin\Release\Kooboo.CMS.Membership.China.dll" "..\Publish\Release\Kooboo.CMS.Membership.China\lib\Kooboo.CMS.Membership.China.dll"
cd ..
cd Publish

nuget pack Release\Kooboo.CMS.Toolkit\Kooboo.CMS.Toolkit.nuspec -OutputDirectory packages
nuget setApiKey 12487df2-6ae4-449f-a648-4237aba653b6

nuget pack Release\Kooboo.CMS.Toolkit.Controls\Kooboo.CMS.Toolkit.Controls.nuspec -OutputDirectory packages
nuget setApiKey 12487df2-6ae4-449f-a648-4237aba653b6

nuget pack Release\Kooboo.Core\Kooboo.Core.nuspec -OutputDirectory packages
nuget setApiKey 12487df2-6ae4-449f-a648-4237aba653b6

nuget pack Release\Kooboo.ModuleDevelopment.Binaries\Kooboo.ModuleDevelopment.Binaries.nuspec -OutputDirectory packages
nuget setApiKey 12487df2-6ae4-449f-a648-4237aba653b6

nuget pack Release\Kooboo.CMS.Content.UserKeyGenerator.Chinese\Kooboo.CMS.Content.UserKeyGenerator.Chinese.nuspec -OutputDirectory packages
nuget setApiKey 12487df2-6ae4-449f-a648-4237aba653b6

nuget pack Release\Kooboo.CMS.Membership.China\Kooboo.CMS.Membership.China.nuspec -OutputDirectory packages
nuget setApiKey 12487df2-6ae4-449f-a648-4237aba653b6
