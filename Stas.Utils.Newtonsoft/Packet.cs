namespace Stas.Utils;
class Packet {
    public MemoryStream ms;
    public Opcode opc { get; }
    public int size { get; }
    public int done;
    public Packet(Opcode _opc, int _size) {
        opc = _opc;
        size = _size;
        ms = new MemoryStream(size);
    }
    ~Packet() {
        ms.Dispose();
    }
    public override string ToString() {
        return opc + " " + size + " " + ms.Length;
    }
}

