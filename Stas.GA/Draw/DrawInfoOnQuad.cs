using ImGuiNET;
using System.Drawing;

namespace Stas.GA;
partial class DrawMain {
    void DrawInfoOnQuad(string text) {
        var impTextColor = Color.White.ToImguiVec4();
        var sp = ImGui.GetCursorScreenPos();
        var ts = ImGui.CalcTextSize(text);
        var lt = sp;
        var rt = sp.Increase(ts.X, 0);
        var rb = sp.Increase(ts.X, ts.Y);
        var lb = sp.Increase(0, ts.Y);
        info_ptr.AddQuadFilled(lt, rt, rb, lb, Color.Black.ToImgui());
        ImGui.TextColored(impTextColor, text);
    }
}