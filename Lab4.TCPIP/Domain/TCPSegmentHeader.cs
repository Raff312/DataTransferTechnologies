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
        UpdatePropertiesFrom(new BitArray(bytes));
    }

    private void UpdatePropertiesFrom(BitArray bits) {
        var index = 0;
        foreach (var property in Properties) {
            for (var i = 0; i < property.Length; i++) {
                property[i] = bits[index + i];
            }

            index += property.Length;
        }
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

    private BitArray[] Properties => [
        SourcePort,
        DestinationPort,
        SequenceNumber,
        AcknowledgmentNumber,
        DataOffset,
        Reserved,
        ControlBits,
        Window,
        Checksum,
        UrgentPointer
    ];

    private int TotalBitsCount => Properties.Sum(x => x.Length);

    public uint SequenceNumberValue => SequenceNumber.ToUInt();

    public byte[] ToBytes() {
        var totalBits = new BitArray(TotalBitsCount);
        MergePropertiesBits(totalBits);
        return totalBits.ToBytes();
    }

    private void MergePropertiesBits(BitArray mergedBits) {
        var index = 0;
        foreach (var property in Properties) {
            foreach (var bit in property.Cast<bool>()) {
                mergedBits[index] = bit;
                index++;
            }
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
