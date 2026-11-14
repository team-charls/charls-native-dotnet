rmdir /s /q artifacts
rmdir /s /q extern\charls\build
dotnet restore -p:IncludeNet48=true
msbuild -t:clean -p:Configuration=Release -p:IncludeNet48=true
msbuild -t:build -p:Configuration=Release -p:IncludeNet48=true
msbuild -t:pack -p:Configuration=Release -p:IncludeNet48=true -p:ContinuousIntegrationBuild=true -p:CertificateThumbprint=%1 -p:CertificateThumbprint256=%2 -p:TimestampUrl=%3
