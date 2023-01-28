using System.Runtime.InteropServices;

namespace Stas.GA;
public partial class AreaInstance {
    [DllImport("Stas.GA.Native.dll", SetLastError = true, EntryPoint = "GetExpedMarkerWrap")]
    public static extern int GetExpedMarkerWrap(IntPtr anim_ptr, ref MapItemWrap miw);

    MapItem GetExpedMarker(Entity e) {
        var miw = new MapItemWrap();
        var anim_ptr = e.componentAddresses["Animated"];
        GetExpedMarkerWrap(anim_ptr, ref miw);
        var info = Marshal.PtrToStringAnsi(miw.info_ptr);
        return asStaticMapItem(e, miw.type, miw.index, info, miw.priority, null);
    }
}
