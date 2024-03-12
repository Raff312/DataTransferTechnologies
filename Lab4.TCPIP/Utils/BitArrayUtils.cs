using System.Buffers.Binary;
using System.Collections;

namespace Lab4.TCPIP.Utils;

public static class BitArrayUtils {
    public static byte[] ToBytes(this BitArray bits) {
        var ret = new byte[(bits.Length - 1) / 8 + 1];
        bits.CopyTo(ret, 0); // Little Endian
        return ret;
    }

    public static uint ToUInt(this BitArray bits) {
        if (bits.Count > 32) {
            throw new ArgumentException("Too many bits for uint");
        }

        if (bits.Count < 32) {
            var list = new List<bool>(bits.Cast<bool>());
            list.InsertRange(0, new bool[32 - bits.Count]);
            return new BitArray(list.ToArray()).ToUIntInternal();
        }

        return bits.ToUIntInternal();
    }

    private static uint ToUIntInternal(this BitArray bits) {
        var bytes = new byte[4];
        bits.CopyTo(bytes, 0); // Little Endian
        return BinaryPrimitives.ReadUInt32LittleEndian(bytes);
    }
}
