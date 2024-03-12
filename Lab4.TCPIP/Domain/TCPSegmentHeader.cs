using System.Collections;

using Lab4.TCPIP.Utils;

namespace Lab4.TCPIP.Domain;

public sealed class TCPSegmentHeader {
    public const ushort Length = 20;

    private TCPSegmentHeader() {
    }

    public TCPSegmentHeader(uint number, ushort sourcePort, ushort destinationPort) {
        SequenceNumber = new BitArray(BitConverter.GetBytes(number));
        SourcePort = new BitArray(BitConverter.GetBytes(sourcePort));
        DestinationPort = new BitArray(BitConverter.GetBytes(destinationPort));
    }

    public TCPSegmentHeader(byte[] bytes) {
        var bits = new BitArray(bytes);

        var index = 0;
        UpdatePropertyFrom(bits, SourcePort, ref index);
        UpdatePropertyFrom(bits, DestinationPort, ref index);
        UpdatePropertyFrom(bits, SequenceNumber, ref index);
        UpdatePropertyFrom(bits, AcknowledgmentNumber, ref index);
        UpdatePropertyFrom(bits, DataOffset, ref index);
        UpdatePropertyFrom(bits, Reserved, ref index);
        UpdatePropertyFrom(bits, ControlBits, ref index);
        UpdatePropertyFrom(bits, Window, ref index);
        UpdatePropertyFrom(bits, Checksum, ref index);
        UpdatePropertyFrom(bits, UrgentPointer, ref index);
    }

    private static void UpdatePropertyFrom(BitArray bits, BitArray propertyBits, ref int index) {
        for (var i = 0; i < propertyBits.Length; i++) {
            propertyBits[i] = bits[index + i];
        }

        index += propertyBits.Length;
    }

    private BitArray SourcePort { get; set; } = new(16);
    private BitArray DestinationPort { get; set; } = new(16);
    private BitArray SequenceNumber { get; set; } = new(32);
    private BitArray AcknowledgmentNumber { get; set; } = new(32);
    private BitArray DataOffset { get; set; } = new(4);
    private BitArray Reserved { get; set; } = new(4);
    private BitArray ControlBits { get; set; } = new(8);
    private BitArray Window { get; set; } = new(16);
    private BitArray Checksum { get; set; } = new(16);
    private BitArray UrgentPointer { get; set; } = new(16);

    public uint SequenceNumberValue => SequenceNumber.ToUInt();

    public byte[] ToBytes() {
        var totalBitsLength = GetTotalBitsLength();
        var totalBits = new BitArray(totalBitsLength);

        var index = 0;
        MergeBitArrays(totalBits, SourcePort, ref index);
        MergeBitArrays(totalBits, DestinationPort, ref index);
        MergeBitArrays(totalBits, SequenceNumber, ref index);
        MergeBitArrays(totalBits, AcknowledgmentNumber, ref index);
        MergeBitArrays(totalBits, DataOffset, ref index);
        MergeBitArrays(totalBits, Reserved, ref index);
        MergeBitArrays(totalBits, ControlBits, ref index);
        MergeBitArrays(totalBits, Window, ref index);
        MergeBitArrays(totalBits, Checksum, ref index);
        MergeBitArrays(totalBits, UrgentPointer, ref index);

        return totalBits.ToBytes();
    }

    private int GetTotalBitsLength() {
        return
            SourcePort.Length +
            DestinationPort.Length +
            SequenceNumber.Length +
            AcknowledgmentNumber.Length +
            DataOffset.Length +
            Reserved.Length +
            ControlBits.Length +
            Window.Length +
            Checksum.Length +
            UrgentPointer.Length;
    }

    private static void MergeBitArrays(BitArray mergedBits, BitArray bits, ref int index) {
        foreach (var bit in bits.Cast<bool>()) {
            mergedBits[index] = bit;
            index++;
        }
    }

    public TCPSegmentHeader Clone() {
        return new() {
            SourcePort = (BitArray)SourcePort.Clone(),
            DestinationPort = (BitArray)DestinationPort.Clone(),
            SequenceNumber = (BitArray)SequenceNumber.Clone(),
            AcknowledgmentNumber = (BitArray)AcknowledgmentNumber.Clone(),
            DataOffset = (BitArray)DataOffset.Clone(),
            Reserved = (BitArray)Reserved.Clone(),
            ControlBits = (BitArray)ControlBits.Clone(),
            Window = (BitArray)Window.Clone(),
            Checksum = (BitArray)Checksum.Clone(),
            UrgentPointer = (BitArray)UrgentPointer.Clone()
        };
    }
}
