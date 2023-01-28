using System.Diagnostics;

namespace Stas.Utils;

public class Remnant {
    public Dictionary<string, int> positive { get; set; } = new();
    public Dictionary<string, int> negative { get; set; } = new();
    public Dictionary<string, int> unknow { get; set; } = new();
    public byte[] ToByte() {
        using (var ms = new MemoryStream()) {
            using (var bw = new BinaryWriter(ms)) {
                bw.Write(BitConverter.GetBytes(positive.Count));
                foreach (var p in positive) {
                    bw.Write(p.Key.To_UTF8_Byte());
                    bw.Write(BitConverter.GetBytes(p.Value));
                }
                bw.Write(BitConverter.GetBytes(negative.Count));
                foreach (var p in negative) {
                    bw.Write(p.Key.To_UTF8_Byte());
                    bw.Write(BitConverter.GetBytes(p.Value));
                }
                bw.Write(BitConverter.GetBytes(unknow.Count));
                foreach (var p in unknow) {
                    bw.Write(p.Key.To_UTF8_Byte());
                    bw.Write(BitConverter.GetBytes(p.Value));
                }
            }
            return ms.ToArray();
        }
    }
    public void FillFromByteArray(byte[] ba, int offs = 0) { // офссет на опкод
        Debug.Assert(offs < 5);
        using (var ms = new MemoryStream(ba)) {
            ms.Position = offs;
            using (var br = new BinaryReader(ms)) {
                var p_count = br.ReadInt32();
                for (int i = 0; i < p_count; i++) {
                    positive.Add(br.To_UTF8_String(), br.ReadInt32());
                }
                var n_count = br.ReadInt32();
                for (int i = 0; i < n_count; i++) {
                    negative.Add(br.To_UTF8_String(), br.ReadInt32());
                }
                var u_count = br.ReadInt32();
                for (int i = 0; i < u_count; i++) {
                    unknow.Add(br.To_UTF8_String(), br.ReadInt32());
                }
            }
        }
    }
    public override string ToString() {
        return "pos[" + positive.Count + "] "
            + "neg[" + negative.Count + "]"
            + " unkn[" + unknow.Count + "]";
    }
}
