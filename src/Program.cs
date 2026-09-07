using System.Reflection;
using Reader;
using Logging;

class MainProgram
{
    static int Main(string[] args)
    {
        if (args.Length == 0)
        {
            PrintUsage();
            return 1;
        }

        if (args.Contains("-h"))
        {
            PrintHelp();
            return 0;
        }

        if (args.Contains("-v"))
        {
            PrintVersion();
            return 0;
        }

        if (args.Contains("-V"))
        {
          Logger.Instance.VerboseMode = true;
        }

        string infile_str;
        string outpath_str;

        int infile_index = Array.IndexOf(args, "-i");
        if (infile_index != -1 && args.Length >= infile_index + 2)
        {
            infile_str = args[infile_index + 1];
        }
        else
        {
            PrintUsage();
            return 1;
        }
        ;

        int outpath_index = Array.IndexOf(args, "-o");
        if (outpath_index != -1 && args.Length >= outpath_index + 2)
        {
            outpath_str = args[outpath_index + 1];
        }
        else
        {
            PrintUsage();
            return 1;
        }
        ;

        if (Path.Exists(infile_str)) { }
        else
        {
            Console.Error.WriteLine("ERR: Can not find input file: {0}", infile_str);
            return 1;
        }

        if (Path.Exists(outpath_str)) { }
        else
        {
            Console.Error.WriteLine("ERR: Can not find output path: {0}", outpath_str);
            return 1;
        }

        ReadSheet(infile_str, outpath_str);

        Console.WriteLine("Success. Ouptut located at: {0}", Path.GetFullPath(outpath_str));

        return 0;
    }

    static void PrintUsage()
    {
        Console.WriteLine("usage: sdbb-gui [-h][-V][-v] -i input_file -o output_directory");
    }

    static void PrintHelp()
    {
        PrintUsage();

        Console.WriteLine("");
        Console.WriteLine("-i input_file      \t\tInput character spreadsheet");
        Console.WriteLine("-o output_directory\t\tOutput GUI menu path");
        Console.WriteLine("-V                 \t\tVerbose output");
        Console.WriteLine("-v                 \t\tPrint version");
        Console.WriteLine("");
        Console.WriteLine("Report bugs to Sw3d15h-F1s4");
    }

    static void PrintVersion()
    {
        string? version = Assembly.GetExecutingAssembly()
                                 .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                                 .InformationalVersion;
        if (version != null) {
          Console.WriteLine("sdbb-gui {0}", version);
        } else {
          Console.WriteLine("sdbb-gui debug");
        }

    }

    static void ReadSheet(string infile, string outdir)
    {
        var sheetreader = new SheetReader(new(Path.GetFullPath(infile)));

        sheetreader.ReadCharSkinSheet();

        sheetreader.PrintMenu(Path.GetFullPath(outdir));
    }
}
