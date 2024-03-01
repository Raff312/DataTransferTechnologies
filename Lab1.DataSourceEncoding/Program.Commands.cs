namespace Lab1.DataSourceEncoding;

public partial class Program {
    private const string DefaultInputFilePath = "./InputData/test.txt";
    private const string DefaultOutputFilePath = "./OutputData/file.32a";
    private const string DefaultDecodedFilePath = "./DecodedData/file";

    private static readonly CommandDefinition[] CommandDefinitions = [
        new("1") {
            Description = "Run",
            Action = Run
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

    private static void Run() {
        var archiver = new Archiver();

        archiver.Archive(DefaultInputFilePath, DefaultOutputFilePath);
        Console.WriteLine("Archive completed.");

        archiver.Unarchive(DefaultOutputFilePath, DefaultDecodedFilePath);
        Console.WriteLine("Unarchive completed.");
    }

    private sealed class CommandDefinition(params string[] codes) {
        public string[] Codes { get; } = codes;
        public string Description { get; set; } = string.Empty;
        public Action? Action { get; set; }
    }
}
