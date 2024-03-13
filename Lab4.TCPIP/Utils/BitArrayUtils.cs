using System.Buffers.Binary;
using System.Collections;

namespace Lab4.TCPIP.Utils;

public static class BitArrayUtils {
    public static byte[] ToBytes(this BitArray bits) {
        var ret = new byte[(bits.Length - 1) / 8 + 1];
        bits.CopyTo(ret, 0);
        return ret;
    }

    public static uint ToUInt(this BitArray bits) {
        if (bits.Count != 32) {
            throw new ArgumentException("Bits count is not correspond to uint");
        }

        var bytes = new byte[4];
        bits.CopyTo(bytes, 0);
        return BinaryPrimitives.ReadUInt32LittleEndian(bytes);
    }
}
