using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Stas.Utils; 

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct Patt_wrapp {
    public IntPtr ptr;
    public int ba_size;
}
public enum PattNams_old : byte {
    PlayerInventory, GameStates, FileRoot, AreaChangeCounter,
    GameWindowScaleValues, TerrainRotatorHelper, TerrainRotationSelector
}
public enum PattNams : byte {
    PlayerInventory, GameController, GameStates, FileRoot,
    AreaChangeCounter, GameWindowScaleValues, TerrainRotatorHelper,
    TerrainRotationSelector, GameCullSize
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct Patt {
    public PattNams patt;
    public long ptr;

    public byte[] ToByte() {
        return BYTE.Concat(new byte[] { (byte)patt }, BitConverter.GetBytes(ptr));
    }
    public void Fill(BinaryReader br) {
        patt = (PattNams)br.ReadByte();
        ptr = br.ReadInt64();
    }
    public override string ToString() {
        return patt + "=" + ptr.ToString("X");
    }
}
