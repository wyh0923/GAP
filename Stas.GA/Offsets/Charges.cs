namespace Stas.GA
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    struct ChargesOffsets
    {
        [FieldOffset(0x00)] public ComponentHeader Header;
        [FieldOffset(0x10)] public IntPtr ext_ptr;
        [FieldOffset(0x18)] public int current;
        [FieldOffset(0x18+4)] public int int_1;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    struct ChargesExt {
        [FieldOffset(0x10)] public int MaxBaseCharges;
        [FieldOffset(0x14)] public int MaxBaseCharges2;//same like 0x10
        [FieldOffset(0x18)] public int ChargesPerUse;
        [FieldOffset(0x1C)] public int ChargesPerUse2;//same like 0x18
    }
}