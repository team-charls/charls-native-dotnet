rmdir /s /q artifacts
rmdir /s /q extern\charls\build
nuget restore
msbuild -t:restore -p:Configuration=Release -p:TargetFramework=net48
msbuild -t:restore -p:Configuration=Release -p:TargetFramework=net11.0
msbuild -t:clean -p:Configuration=Release
msbuild -t:pack -p:Configuration=Release -p:ContinuousIntegrationBuild=true -p:CertificateThumbprint=%1 -p:CertificateThumbprint256=%2 -p:TimestampUrl=%3
