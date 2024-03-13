using System.Collections;

using Lab4.TCPIP.Utils;

namespace Lab4.TCPIP.Domain;

public sealed class IPPackageHeader {
    public const ushort Length = 20;

    private IPPackageHeader() {
    }

    public IPPackageHeader(ushort totalLength, uint sourceAddress, uint destinationAddress) {
        TotalLength = new BitArray(BitConverter.GetBytes(totalLength));
        SourceAddress = new BitArray(BitConverter.GetBytes(sourceAddress));
        DestinationAddress = new BitArray(BitConverter.GetBytes(destinationAddress));
    }

    public IPPackageHeader(byte[] bytes) {
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

    private BitArray Version { get; set; } = new(4);
    private BitArray Ihl { get; set; } = new(4);
    private BitArray TypeOfService { get; set; } = new(8);
    private BitArray TotalLength { get; set; } = new(16);
    private BitArray Identification { get; set; } = new(16);
    private BitArray Flags { get; set; } = new(3);
    private BitArray FragmentOffset { get; set; } = new(13);
    private BitArray TimeToLive { get; set; } = new(8);
    private BitArray Protocol { get; set; } = new BitArray(new byte[] { ProtocolNumbers.TCP });
    private BitArray HeaderChecksum { get; set; } = new(16);
    private BitArray SourceAddress { get; set; } = new(32);
    private BitArray DestinationAddress { get; set; } = new(32);

    private BitArray[] Properties => [
        Version,
        Ihl,
        TypeOfService,
        TotalLength,
        Identification,
        Flags,
        FragmentOffset,
        TimeToLive,
        Protocol,
        HeaderChecksum,
        SourceAddress,
        DestinationAddress
    ];

    private int TotalBitsCount => Properties.Sum(x => x.Length);

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

    public IPPackageHeader Clone() {
        return new() {
            Version = (BitArray)Version.Clone(),
            Ihl = (BitArray)Ihl.Clone(),
            TypeOfService = (BitArray)TypeOfService.Clone(),
            TotalLength = (BitArray)TotalLength.Clone(),
            Identification = (BitArray)Identification.Clone(),
            Flags = (BitArray)Flags.Clone(),
            FragmentOffset = (BitArray)FragmentOffset.Clone(),
            TimeToLive = (BitArray)TimeToLive.Clone(),
            Protocol = (BitArray)Protocol.Clone(),
            HeaderChecksum = (BitArray)HeaderChecksum.Clone(),
            SourceAddress = (BitArray)SourceAddress.Clone(),
            DestinationAddress = (BitArray)DestinationAddress.Clone()
        };
    }
}
