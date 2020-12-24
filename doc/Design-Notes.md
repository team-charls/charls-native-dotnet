# Design Notes

## NuGet Package

There are a couple of things that need to be done, that make creating a NuGet package not standard:

- Code signing of all DLLs before the package is created. The private key is stored on a smart card and
signing cannot be done in the CI pipeline.

- Native DLLs need to be put into the NuGet package.

There are 3 methods to create a NuGet package:

1. Using dotnet.exe and property settings in the .csproj file
   This method cannot be used as dotnet.exe cannot build the .csproj file as the C++ DLL also needs to be build.

1. Using nuget.exe and a .nuspec file
   This method works, but information is duplicated and an additional .cmd or other script is needed to execute all
   the commands.

1. Using MSBuild -t:pack.
   This seems to be the best option. Research is needed to see if all required steps can be implemented.
