namespace Lab1.DataSourceEncoding;

public static class Archiver {
    private const ushort BlockSize = 3;
    private const ushort ControlBytes = 0xFFFF;

    public static void ArchiveFile(string inputFilePath, string outputFilePath) {
        var dictionary = new Dictionary<string, ushort>();
        ushort iterator = 1;

        try {
            using var inputStream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read);
            using var outputStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write);

            byte[] buffer = new byte[BlockSize];
            int bytesRead;

            while ((bytesRead = inputStream.Read(buffer, 0, BlockSize)) > 0) {
                if (bytesRead < BlockSize || dictionary.Count >= ushort.MaxValue) {
                    WriteControlBytes(outputStream, buffer, bytesRead);
                    continue;
                }

                string blockKey = BitConverter.ToString(buffer, 0, bytesRead);
                if (!dictionary.ContainsKey(blockKey)) {
                    if (iterator != 1) {
                        dictionary[blockKey] = iterator++;
                    } else {
                        iterator++;
                        continue;
                    }
                }

                byte[] dictKeyBytes = BitConverter.GetBytes(dictionary[blockKey]);
                outputStream.Write(dictKeyBytes, 0, 2);
            }
        } catch {
            throw;
        }
    }

    private static void WriteControlBytes(FileStream outputStream, byte[] buffer, int bytesRead) {
        var controlBytes = BitConverter.GetBytes(ControlBytes);
        outputStream.Write(controlBytes, 0, controlBytes.Length);
        outputStream.Write(buffer, 0, bytesRead);
    }
}
