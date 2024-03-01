using System.Text;

using Lab1.DataSourceEncoding.Utils;

namespace Lab1.DataSourceEncoding;

public class Archiver {
    private const ushort BlockSize = 3;
    private const ushort ControlBytes = 0xFFFF;

    private bool IsControlBytesAdded;

    public void Archive(string inputFilePath, string outputFilePath) {
        IsControlBytesAdded = false;

        var dictionary = new Dictionary<string, ushort>();
        ushort iterator = 0;

        try {
            using var inputStream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read);
            using var outputStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write);

            var buffer = new byte[BlockSize];
            int bytesRead;

            var fileBytes = new List<byte>();
            fileBytes.AddRange(Encoding.ASCII.GetBytes("32A"));

            var outputFlow = new List<byte>();

            while ((bytesRead = inputStream.Read(buffer, 0, BlockSize)) > 0) {
                if (dictionary.Count >= 65535 || bytesRead < BlockSize) {
                    WriteTailBytes(outputFlow, buffer);
                    continue;
                }

                var bufferStr = BitConverter.ToString(buffer);
                if (!dictionary.TryGetValue(bufferStr, out var blockValue)) {
                    dictionary.Add(bufferStr, iterator++);
                    fileBytes.AddRange(buffer);
                }

                outputFlow.AddRange(BitConverter.GetBytes(dictionary[bufferStr]));
            }

            fileBytes.InsertRange(3, BitConverter.GetBytes((ushort)dictionary.Count));
            fileBytes.InsertRange(5 + BlockSize * dictionary.Count, outputFlow);

            outputStream.Write(fileBytes.ToArray());
        } catch {
            throw;
        }
    }

    private void WriteTailBytes(List<byte> fileBytes, byte[] bytes) {
        WriteControlBytes(fileBytes);
        fileBytes.AddRange(bytes);
    }

    private void WriteControlBytes(List<byte> fileBytes) {
        if (IsControlBytesAdded) {
            return;
        }

        var controlBytes = BitConverter.GetBytes(ControlBytes);
        fileBytes.AddRange(controlBytes);
    }

    public void Unarchive(string inputFilePath, string outputFilePath) {
        try {
            using var inputStream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read);
            using var outputStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write);

            CheckFormat(inputStream);
            var dictionary = GenerateDictionary(inputStream);

            var buffer = new byte[2];
            int bytesRead;

            var iterator = 0;

            while ((bytesRead = inputStream.Read(buffer, 0, 2)) > 0) {
                var byteValue = BitConverter.ToUInt16(buffer);
                if (byteValue == ControlBytes) {
                    break;
                }

                iterator++;

                outputStream.Write(ByteUtils.HexStringToByteArray(dictionary[byteValue]));
            }

            buffer = new byte[BlockSize];
            while ((bytesRead = inputStream.Read(buffer, 0, BlockSize)) > 0) {
                outputStream.Write(buffer);
            }
        } catch {
            throw;
        }
    }

    private static void CheckFormat(FileStream stream) {
        var formatBuffer = new byte[BlockSize];

        stream.Read(formatBuffer, 0, BlockSize);
        var format = Encoding.ASCII.GetString(formatBuffer);
        if (format != "32A") {
            throw new FormatException("Incorrect file format");
        }
    }

    private static Dictionary<ushort, string> GenerateDictionary(FileStream stream) {
        var result = new Dictionary<ushort, string>();

        var dictionaryLength = GetDictionaryLength(stream);
        var buffer = new byte[BlockSize];
        ushort iterator = 0;

        for (var i = 0; i < dictionaryLength; i++) {
            stream.Read(buffer, 0, BlockSize);

            var bufferStr = BitConverter.ToString(buffer);
            result.Add(iterator++, bufferStr);
        }

        return result;
    }

    private static ushort GetDictionaryLength(FileStream stream) {
        var lengthBuffer = new byte[2];

        stream.Read(lengthBuffer, 0, 2);
        return BitConverter.ToUInt16(lengthBuffer);
    }
}
