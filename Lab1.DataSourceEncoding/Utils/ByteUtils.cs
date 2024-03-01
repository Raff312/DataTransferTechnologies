namespace Lab1.DataSourceEncoding.Utils;

public static class ByteUtils {
    public static byte[] HexStringToByteArray(string s) {
        var arr = s.Split('-');
        var array = new byte[arr.Length];

        for (var i = 0; i < arr.Length; i++) {
            array[i] = Convert.ToByte(arr[i], 16);
        }

        return array;
    }
}
