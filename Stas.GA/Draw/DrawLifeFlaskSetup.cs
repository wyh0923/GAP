using ImGuiNET;
using System.Runtime.InteropServices;
using V2 = System.Numerics.Vector2;
namespace Stas.GA;

partial class DrawMain {
    void DrawLifeFlaskSetup() {
        var flist = ui.flask_keys.Select(x => x.ToString()).ToArray();

        if (ImGui.Checkbox("Life  ", ref ui.sett.b_use_life_flask)) {
            ui.sett.Save();
        }
        ImGuiExt.ToolTip("Will also use this flask if you left [%] life");
        var life_flask_key = ui.sett.life_flask_key;
        if (ui.worker != null)
            life_flask_key = ui.worker.life_flask_key;
        var life_index = ui.flask_keys.IndexOf(life_flask_key);
        ImGui.SameLine();
        ImGui.SetNextItemWidth(40);
        if (ImGui.Combo("<=lKey ", ref life_index, flist, flist.Length)) {
            ui.sett.life_flask_key = ui.flask_keys[life_index];
            ui.sett.Save();
        }
        ImGui.SetNextItemWidth(60);
        ImGui.SameLine();
        if (ImGui.SliderInt("<=trigger last %   ", ref ui.sett.trigger_life_left_persent, 5, 80)) {
            ui.sett.Save();
        }
        ImGuiExt.ToolTip("Set the triggering percentage (default==50)");


        ImGuiExt.ToolTip("automatically cast in the presence of danger nearby\n" +
             "For example, if there is a mod on the map...\n" +
             "layers cannot regenerate life, mana and ES.");
    }
}
