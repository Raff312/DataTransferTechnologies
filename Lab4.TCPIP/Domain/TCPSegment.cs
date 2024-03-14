namespace Lab4.TCPIP.Domain;

public sealed class TCPSegment {
    public TCPSegment(TCPSegmentHeader header, byte[] data) {
        Header = header.Clone();
        Data = data;
    }

    public TCPSegment(byte[] bytes) {
        Header = new TCPSegmentHeader(bytes.Take(20).ToArray());
        Data = bytes.Skip(20).ToArray();
    }

    public TCPSegmentHeader Header { get; private set; }
    public byte[] Data { get; private set; }

    public byte[] ToBytes() {
        var headerBytes = Header?.ToBytes();
        if (headerBytes is null) {
            return [];
        }

        return [.. headerBytes, .. Data ?? []];
    }
}
