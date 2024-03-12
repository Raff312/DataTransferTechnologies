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
        var bits = new BitArray(bytes);

        var index = 0;
        UpdatePropertyFrom(bits, Version, ref index);
        UpdatePropertyFrom(bits, Ihl, ref index);
        UpdatePropertyFrom(bits, TypeOfService, ref index);
        UpdatePropertyFrom(bits, TotalLength, ref index);
        UpdatePropertyFrom(bits, Identification, ref index);
        UpdatePropertyFrom(bits, Flags, ref index);
        UpdatePropertyFrom(bits, FragmentOffset, ref index);
        UpdatePropertyFrom(bits, TimeToLive, ref index);
        UpdatePropertyFrom(bits, Protocol, ref index);
        UpdatePropertyFrom(bits, HeaderChecksum, ref index);
        UpdatePropertyFrom(bits, SourceAddress, ref index);
        UpdatePropertyFrom(bits, DestinationAddress, ref index);
    }

    private static void UpdatePropertyFrom(BitArray bits, BitArray propertyBits, ref int index) {
        for (var i = 0; i < propertyBits.Length; i++) {
            propertyBits[i] = bits[index + i];
        }

        index += propertyBits.Length;
    }

    private BitArray Version { get; set; } = new(4);
    private BitArray Ihl { get; set; } = new(4);
    private BitArray TypeOfService { get; set; } = new(8);
    private BitArray TotalLength { get; set; } = new(16);
    private BitArray Identification { get; set; } = new(16);
    private BitArray Flags { get; set; } = new(3);
    private BitArray FragmentOffset { get; set; } = new(13);
    private BitArray TimeToLive { get; set; } = new(8);
    private BitArray Protocol { get; set; } = new(8);
    private BitArray HeaderChecksum { get; set; } = new(16);
    private BitArray SourceAddress { get; set; } = new(32);
    private BitArray DestinationAddress { get; set; } = new(32);

    public byte[] ToBytes() {
        var totalBitsLength = GetTotalBitsLength();
        var totalBits = new BitArray(totalBitsLength);

        var index = 0;
        MergeBitArrays(totalBits, Version, ref index);
        MergeBitArrays(totalBits, Ihl, ref index);
        MergeBitArrays(totalBits, TypeOfService, ref index);
        MergeBitArrays(totalBits, TotalLength, ref index);
        MergeBitArrays(totalBits, Identification, ref index);
        MergeBitArrays(totalBits, Flags, ref index);
        MergeBitArrays(totalBits, FragmentOffset, ref index);
        MergeBitArrays(totalBits, TimeToLive, ref index);
        MergeBitArrays(totalBits, Protocol, ref index);
        MergeBitArrays(totalBits, HeaderChecksum, ref index);
        MergeBitArrays(totalBits, SourceAddress, ref index);
        MergeBitArrays(totalBits, DestinationAddress, ref index);

        return totalBits.ToBytes();
    }

    private int GetTotalBitsLength() {
        return
            Version.Length +
            Ihl.Length +
            TypeOfService.Length +
            TotalLength.Length +
            Identification.Length +
            Flags.Length +
            FragmentOffset.Length +
            TimeToLive.Length +
            Protocol.Length +
            HeaderChecksum.Length +
            SourceAddress.Length +
            DestinationAddress.Length;
    }

    private static void MergeBitArrays(BitArray mergedBits, BitArray bits, ref int index) {
        foreach (var bit in bits.Cast<bool>()) {
            mergedBits[index] = bit;
            index++;
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
