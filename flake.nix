{
  description = "SDBB Gui Helper";

  inputs = {
    nixpkgs = {
      url = "nixpkgs/nixos-unstable";
    };

    treefmt-nix = {
      url = "github:numtide/treefmt-nix";
      inputs.nixpkgs.follows = "nixpkgs";
    };
  };

  outputs = 
  {
    self,
    nixpkgs,
    treefmt-nix,
    ...
  }:
  let
    pkgs = nixpkgs.legacyPackages.x86_64-linux;
    treefmtEval = treefmt-nix.lib.evalModule pkgs ./treefmt.nix;
  in
  {
    packages.x86_64-linux = rec {
      sdbb-gui = pkgs.callPackage ./package/sdbb-gui.nix { };
      default = sdbb-gui;
    };

    devShells.x86_64-linux.default = import ./package/shell.nix { inherit pkgs; };

    formatter.x86_64-linux = treefmtEval.config.build.wrapper;

    checks.x86_64-linux = {
      formatting = treefmtEval.config.build.check self;
    };
  };
}
