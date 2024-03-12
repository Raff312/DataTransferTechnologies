using Lab4.TCPIP.Utils;

namespace Lab4.TCPIP;

public partial class Program {
    private const string DefaultInputFilePath = "./InputData/file.webp";
    private const string PackagesDirectory = "./Packages/";
    private const string OutputDataDirectory = "./OutputData/";

    private static readonly CommandDefinition[] CommandDefinitions = [
        new("1") {
            Description = "Pack",
            Action = Pack
        },
        new("2") {
            Description = "Unpack",
            Action = Unpack
        },
        new("0", "exit") {
            Description = "Exit from program",
            Action = null
        }
    ];

    private static CommandDefinition? GetCommandDefinitionByCode(string code) {
        code = code.ToLowerInvariant();
        return CommandDefinitions.FirstOrDefault(x => x.Codes.Contains(code));
    }

    private static void Pack() {
        var path = ConsoleUtils.GetValueFromUser<string>("Enter file path to pack: ");
        if (string.IsNullOrWhiteSpace(path)) {
            path = DefaultInputFilePath;
        }

        ClearPackagesDirectory();

        PackageManager.Pack(path, PackagesDirectory);

        Console.WriteLine("\nThe file is packed.");
    }

    private static void ClearPackagesDirectory() {
        var outputDirectory = new DirectoryInfo(PackagesDirectory);
        foreach (var file in outputDirectory.EnumerateFiles()) {
            file.Delete();
        }
        foreach (var dir in outputDirectory.EnumerateDirectories()) {
            dir.Delete(true);
        }
    }

    private static void Unpack() {
        PackageManager.Unpack(PackagesDirectory, OutputDataDirectory);

        Console.WriteLine("The packages are unpacked.");
    }

    private sealed class CommandDefinition(params string[] codes) {
        public string[] Codes { get; } = codes;
        public string Description { get; set; } = string.Empty;
        public Action? Action { get; set; }
    }
}
