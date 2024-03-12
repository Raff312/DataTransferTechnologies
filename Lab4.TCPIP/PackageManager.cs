using Lab4.TCPIP.Domain;

namespace Lab4.TCPIP;

public static class PackageManager {
    private const uint SourceAddress = 1234;
    private const uint DestinationAddress = 5678;
    private const ushort SourcePort = 80;
    private const ushort DestinationPort = 80;
    private const ushort MTU = 1500;
    private const ushort PayloadLength = MTU - TCPSegmentHeader.Length - IPPackageHeader.Length;

    public static void Pack(string filePath, string packagesDirectory) {
        var bytes = File.ReadAllBytes(filePath);
        var partsCount = (bytes.Length - 1) / PayloadLength + 1;

        for (var i = 0; i < partsCount; i++) {
            var tcpHeader = new TCPSegmentHeader((uint)i, SourcePort, DestinationPort);
            var payloadData = bytes.Skip(i * PayloadLength).Take(PayloadLength).ToArray();
            var tcpSegment = new TCPSegment(tcpHeader, payloadData);

            var ipHeader = new IPPackageHeader((ushort)payloadData.Length, SourceAddress, DestinationAddress);
            var ipPackage = new IPPackage(ipHeader, tcpSegment);

            File.WriteAllBytes($"{packagesDirectory}{i:0000}.ip", ipPackage.ToBytes());
        }
    }

    public static void Unpack(string packagesDirectory, string outputDirectory) {
        var dataSequence = new SortedList<uint, byte[]>();
        var directory = new DirectoryInfo(packagesDirectory);

        foreach (var file in directory.EnumerateFiles()) {
            var fileBytes = File.ReadAllBytes(file.FullName);

            var ipPackage = new IPPackage(fileBytes);
            dataSequence.Add(ipPackage.Segment.Header.SequenceNumberValue, ipPackage.Segment.Data);
        }

        var combinedBytes = dataSequence.SelectMany(x => x.Value).ToArray();
        File.WriteAllBytes($"{outputDirectory}file", combinedBytes);
    }
}
