using System.Buffers.Binary;
using System.Text;

using Lab1.DataSourceEncoding.Utils;

namespace Lab1.DataSourceEncoding;

public class Archiver {
    private const ushort BlockSize = 3;
    private const ushort ControlBytes = 0xFFFF;

    public void Archive(string inputFilePath) {
        var dictionary = new Dictionary<string, ushort>();
        ushort iterator = 0;

        using var inputStream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read);
        using var outputStream = new FileStream(GetEnsuredArchiveFilePath(inputStream), FileMode.Create, FileAccess.Write);

        var buffer = new byte[BlockSize];
        int bytesRead;

        var fileBytes = new List<byte>();
        fileBytes.AddRange(Encoding.ASCII.GetBytes("32A"));

        var outputFlow = new List<byte>();

        while ((bytesRead = inputStream.Read(buffer, 0, BlockSize)) > 0) {
            if (dictionary.Count >= 65535 || bytesRead < BlockSize) {
                inputStream.Seek(-bytesRead, SeekOrigin.Current);

                var tailBuffer = new byte[inputStream.Length - inputStream.Position];
                inputStream.Read(tailBuffer, 0, tailBuffer.Length);

                WriteTailBytes(outputFlow, tailBuffer);
                break;
            }

            var bufferStr = BitConverter.ToString(buffer);
            if (!dictionary.TryGetValue(bufferStr, out var blockValue)) {
                dictionary.Add(bufferStr, iterator++);
                fileBytes.AddRange(buffer);
            }

            outputFlow.AddRange(dictionary[bufferStr].ToBytes());
        }

        fileBytes.InsertRange(3, ((ushort)dictionary.Count).ToBytes());
        fileBytes.AddRange(outputFlow);

        outputStream.Write(fileBytes.ToArray());
    }

    private static string GetEnsuredArchiveFilePath(FileStream stream) {
        var result = Path.Combine("./OutputData", Path.GetFileName(stream.Name) + ".32a");
        if (!Directory.Exists(result)) {
            Directory.CreateDirectory("./OutputData");
        }

        return result;
    }

    private static void WriteTailBytes(List<byte> fileBytes, byte[] tailBytes) {
        var controlBytes = ControlBytes.ToBytes();
        fileBytes.AddRange(controlBytes.Concat(tailBytes));
    }

    public void Unarchive(string inputFilePath) {
        using var inputStream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read);
        using var outputStream = new FileStream(GetEnsuredUnarchiveFilePath(inputStream), FileMode.Create, FileAccess.Write);

        CheckFormat(inputStream);
        var dictionary = GenerateDictionary(inputStream);

        var buffer = new byte[2];
        int bytesRead;

        var iterator = 0;

        while ((bytesRead = inputStream.Read(buffer, 0, 2)) > 0) {
            var byteValue = BinaryPrimitives.ReadUInt16BigEndian(buffer);
            if (byteValue == ControlBytes) {
                break;
            }

            iterator++;

            outputStream.Write(ByteUtils.HexStringToByteArray(dictionary[byteValue]));
        }

        var tailBuffer = new byte[inputStream.Length - inputStream.Position];
        inputStream.Read(tailBuffer, 0, tailBuffer.Length);
        outputStream.Write(tailBuffer);
    }

    private static string GetEnsuredUnarchiveFilePath(FileStream stream) {
        var result = Path.Combine("./DecodedData", Path.GetFileName(stream.Name).Replace(".32a", ""));
        if (!Directory.Exists(result)) {
            Directory.CreateDirectory("./DecodedData");
        }

        return result;
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
        return BinaryPrimitives.ReadUInt16BigEndian(lengthBuffer);
    }
}
