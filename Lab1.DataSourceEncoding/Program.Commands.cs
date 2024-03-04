namespace Lab1.DataSourceEncoding;

public partial class Program {
    private const string DefaultInputFilePath = "./InputData/file.webp";
    private const string DefaultOutputFilePath = "./OutputData/archive.32a";
    private const string DefaultDecodedFilePath = "./DecodedData/file";

    private static readonly CommandDefinition[] CommandDefinitions = [
        new("1") {
            Description = "Archive",
            Action = Archive
        },
        new("2") {
            Description = "Unarchive",
            Action = Unarchive
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

    private static void Archive() {
        var archiver = new Archiver();
        archiver.Archive(DefaultInputFilePath, DefaultOutputFilePath);
        Console.WriteLine("Archive completed.");
    }

    private static void Unarchive() {
        var archiver = new Archiver();
        archiver.Unarchive(DefaultOutputFilePath, DefaultDecodedFilePath);
        Console.WriteLine("Unarchive completed.");
    }

    private sealed class CommandDefinition(params string[] codes) {
        public string[] Codes { get; } = codes;
        public string Description { get; set; } = string.Empty;
        public Action? Action { get; set; }
    }
}
