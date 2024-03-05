using System.Globalization;

namespace Lab3.FaultTolerance;

public partial class Program {
    private const string DefaultInputFilePath = "./InputData/graph.txt";

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
        using var streamReader = new StreamReader(DefaultInputFilePath, new FileStreamOptions() {
            Mode = FileMode.Open,
            Access = FileAccess.Read
        });

        var counter = 0;
        var values = new List<byte>();

        string? line;
        while (!string.IsNullOrWhiteSpace(line = streamReader.ReadLine())) {
            values.AddRange(line.Split(' ').Select(x => byte.Parse(x, CultureInfo.InvariantCulture)));
            counter++;
        }

        var graph = new ByteGraph(values, counter);
        if (!graph.IsSymmetric()) {
            throw new FormatException("Graph has incorrect format.");
        }

        Console.WriteLine($"The connectivity of the graph is equal to {graph.GetConnectivity()}");
    }

    private sealed class CommandDefinition(params string[] codes) {
        public string[] Codes { get; } = codes;
        public string Description { get; set; } = string.Empty;
        public Action? Action { get; set; }
    }
}
