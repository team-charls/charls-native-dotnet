msbuild -t:restore -p:Configuration=Release
msbuild -t:clean -p:Configuration=Release
msbuild -t:pack -p:Configuration=Release -p:ContinuousIntegrationBuild=true -p:CertificateThumbprint=%1 -p:CertificateThumbprint256=%2 -p:TimestampUrl=%3
