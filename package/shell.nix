{
  pkgs,
  ...
}:
pkgs.mkShell rec {
  dotnetPkg = (with pkgs.dotnetCorePackages; combinePackages [
    sdk_8_0
  ]);

  deps = [
  ];

  packages = [
    dotnetPkg
  ];

  NIX_LD_LIBRARY_PATH = pkgs.lib.makeLibraryPath ([
    pkgs.stdenv.cc.cc
  ] ++ deps );
  NIX_LD = "${pkgs.stdenv.cc.libc_bin}/bin/ld.so";
  nativeBuildInputs = [
  ] ++ deps;

  shellHook = ''
    export DOTNET_ROOT="${dotnetPkg}"
    export PATH="$PATH:$HOME/.dotnet/tools"
  '';
}
