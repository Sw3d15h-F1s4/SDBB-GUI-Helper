{
  buildDotnetModule,
  dotnetCorePackages,
}:

buildDotnetModule {

  pname = "sdbb-gui";
  version = "4.2.0";

  src = ../src;

  projectFile = "sdbb-gui.csproj";

  dotnet-sdk = dotnetCorePackages.sdk_8_0;
  dotnet-runtime = dotnetCorePackages.runtime_8_0;

  executables = [ "sdbb-gui" ];
}
