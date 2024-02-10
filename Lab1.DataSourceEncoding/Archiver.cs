namespace Lab1.DataSourceEncoding;

public class Archiver {
    private const ushort BlockSize = 3;
    private const uint ControlBytes = 0xFFFF;

    private bool ControlBytesAdded;
    private bool ControlBytesRead;

    public void Archive(string inputFilePath, string outputFilePath) {
        ControlBytesAdded = false;

        var dictionary = new Dictionary<byte[], ushort>();
        ushort iterator = 1;

        try {
            using var inputStream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read);
            using var outputStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write);

            var buffer = new byte[BlockSize];
            int bytesRead;

            outputStream.Write(BitConverter.GetBytes("32A"));

            while ((bytesRead = inputStream.Read(buffer, 0, BlockSize)) > 0) {
                if (iterator == 0 || bytesRead < BlockSize) {
                    WriteTailBytes(outputStream, buffer);
                    continue;
                }

                if (!dictionary.TryGetValue(buffer, out var blockValue)) {
                    dictionary.Add(buffer, iterator++);
                }

                var outputBytes = BitConverter.GetBytes(dictionary[buffer]);
                outputStream.Write(outputBytes);
            }

            outputStream.Write(BitConverter.GetBytes(dictionary.Count), 4, 2);
        } catch {
            throw;
        }
    }

    private void WriteTailBytes(FileStream stream, byte[] bytes) {
        WriteControlBytes(stream);
        stream.Write(bytes);
    }

    private void WriteControlBytes(FileStream stream) {
        if (ControlBytesAdded) {
            return;
        }

        var controlBytes = BitConverter.GetBytes(ControlBytes);
        stream.Write(controlBytes, 0, controlBytes.Length);
    }

    public void Unarchive(string inputFilePath, string outputFilePath) {
        ControlBytesRead = false;

        try {
            using var inputStream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read);
            using var outputStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write);

            var buffer = new byte[BlockSize];
            int bytesRead;

            while ((bytesRead = inputStream.Read(buffer, 0, BlockSize)) > 0) {
                // TODO: Add logic
            }
        } catch {
            throw;
        }
    }

    private ushort GetBytesLengthByState(ReadState state, ushort dictionaryLength) {
        return state switch {
            ReadState.Format => 3,
            ReadState.DictionaryLength => 2,
            ReadState.Dictionary => dictionaryLength,
            ReadState.Stream => 2,
            _ => throw new NotSupportedException($"{state} is unsupported")
        };
    }

    private enum ReadState {
        Format = 0,
        DictionaryLength = 1,
        Dictionary = 2,
        Stream = 3
    }
}
