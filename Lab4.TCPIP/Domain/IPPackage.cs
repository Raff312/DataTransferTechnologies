namespace Lab4.TCPIP.Domain;

public sealed class IPPackage {
    public IPPackage(IPPackageHeader header, TCPSegment data) {
        Header = header.Clone();
        Segment = data;
    }

    public IPPackage(byte[] bytes) {
        Header = new IPPackageHeader(bytes.Take(20).ToArray());
        Segment = new TCPSegment(bytes.Skip(20).ToArray());
    }

    public IPPackageHeader Header { get; private set; }
    public TCPSegment Segment { get; private set; }

    public byte[] ToBytes() {
        var headerBytes = Header?.ToBytes();
        if (headerBytes is null) {
            return [];
        }

        return [.. headerBytes, .. Segment?.ToBytes() ?? []];
    }
}
