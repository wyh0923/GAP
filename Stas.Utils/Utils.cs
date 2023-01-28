using System.Diagnostics;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using V3 = System.Numerics.Vector3;
using V2 = System.Numerics.Vector2;
using System.Drawing;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
namespace Stas.Utils;

public class ZIP {
    public static string UnZipToString(byte[] ba) {
        if(ba == null)
            return null;
        var sb = new StringBuilder();
        var raw = UnZip(ba);
        if(raw == null) {
            ut.AddToLog("UnZipToString error: UnZip(ba)==null");
            return null;
        }
        using(var ms = new MemoryStream(raw)) {
            var buffer = new byte[1024 * 1024];
            int done;
            while((done = ms.Read(buffer, 0, buffer.Length)) > 0) {
                var temp = new byte[done];
                Array.Copy(buffer, 0, temp, 0, done);
                sb.Append(Encoding.UTF8.GetString(temp));
            }
        }
        return sb.ToString();
    }
    public static byte[] UnZip(byte[] bytes) {
        using(var msi = new MemoryStream(bytes))
        using(var mso = new MemoryStream()) {
            try {
                using(var gs = new GZipStream(msi, CompressionMode.Decompress)) {
                    CopyTo(gs, mso);
                    return mso.ToArray();
                }
            }
            catch(Exception ex) {
                if(ex.Message.Contains("The magic number in GZip header is not correct.")) {
                    return bytes;
                }
                else
                    throw new NotImplementedException(ex.Message);
            }
        }
    }
    public static byte[] ToZip(string str) {
        var ba = Encoding.UTF8.GetBytes(str);
        return ToZip(ba);
    }
    public static byte[] ToZip(Byte[] ba) {
        using(var msi = new MemoryStream(ba))
        using(var mso = new MemoryStream()) {
            using(var gs = new GZipStream(mso, CompressionMode.Compress)) {
                CopyTo(msi, gs);
            }
            return mso.ToArray();
        }
    }
    public static void CopyTo(Stream src, Stream dest) {
        try {
            byte[] bytes = new byte[4096];
            int cnt;
            while((cnt = src.Read(bytes, 0, bytes.Length)) != 0) {
                dest.Write(bytes, 0, cnt);
            }
        }
        catch(Exception ex) {
            ut.AddToLog("ZIP.CopyTo [Err]: " + ex.Message);
        }
    }
}
[StructLayout(LayoutKind.Sequential)]
public struct CURSORINFO {
    public Int32 cbSize;
    public Int32 flags;
    public IntPtr hCursor;
    public POINTAPI ptScreenPos;
}
[StructLayout(LayoutKind.Sequential)]
public struct POINTAPI {
    public int x;
    public int y;
}


public class MurmurHash2Simple {
    public static UInt32 Hash(Byte[] data) {
        return Hash(data, 0xc58f1a7b);
    }
    const UInt32 m = 0x5bd1e995;
    const Int32 r = 24;

    static UInt32 Hash(Byte[] data, UInt32 seed) {
        Int32 length = data.Length;
        if(length == 0)
            return 0;
        UInt32 h = seed ^ (UInt32)length;
        Int32 currentIndex = 0;
        while(length >= 4) {
            UInt32 k = BitConverter.ToUInt32(data, currentIndex);
            k *= m;
            k ^= k >> r;
            k *= m;

            h *= m;
            h ^= k;
            currentIndex += 4;
            length -= 4;
        }
        switch(length) {
            case 3:
                h ^= BitConverter.ToUInt16(data, currentIndex);
                h ^= (UInt32)data[currentIndex + 2] << 16;
                h *= m;
                break;
            case 2:
                h ^= BitConverter.ToUInt16(data, currentIndex);
                h *= m;
                break;
            case 1:
                h ^= data[currentIndex];
                h *= m;
                break;
            default:
                break;
        }

        // Do a few final mixes of the hash to ensure the last few
        // bytes are well-incorporated.

        h ^= h >> 13;
        h *= m;
        h ^= h >> 15;

        return h;
    }
}

public class BYTE {
    public static byte[] Concat(byte[] a, byte[] b) {
        Debug.Assert(a != null && b != null, Environment.StackTrace);
        byte[] buffer1 = new byte[a.Length + b.Length];
        Buffer.BlockCopy(a, 0, buffer1, 0, a.Length);
        Buffer.BlockCopy(b, 0, buffer1, a.Length, b.Length);
        return buffer1;
    }

    public static byte[] Concat(byte[] a, byte[] b, byte[] c, byte[] d) {
        return Concat(Concat(a, b), Concat(c, d));
    }
    public static byte[] Concat(byte[] a, byte[] b, byte[] c) {
        return Concat(Concat(a, b), c);
    }
    public static string ToHexString(byte[] ba) {
        var sb = new StringBuilder(ba.Length * 2);
        foreach(byte b in ba) {
            // can be "x2" if you want lowercase & X2 for upper
            sb.Append(b.ToString("x2"));
        }
        return sb.ToString();
    }

}
