using ImGuiNET;
using System.Runtime.InteropServices;
using V2 = System.Numerics.Vector2;
namespace Stas.GA;

partial class DrawMain {
    void DrawManaFlaskSetup() {
        //ui.SetDebugPossible(null); 
        var flist = ui.flask_keys.Select(x => x.ToString()).ToArray();

        var use_mana_flask = ui.sett.b_use_mana_flask;
        if (ui.worker != null) {
            use_mana_flask = ui.worker.b_use_mana_flask;
        }
        if (ImGui.Checkbox("Mana  ", ref use_mana_flask)) {
            ui.sett.Save();
        }
        ImGuiExt.ToolTip("Will also use this flask if you do't have" +
            "\n enough mana to use the mainskill");

        var mana_key = ui.sett.mana_flask_key;
        if (ui.worker != null) {
            mana_key = ui.worker.mana_flask_key;
        }
        var mana_index = ui.flask_keys.IndexOf(mana_key);

        ImGui.SameLine();
        ImGui.SetNextItemWidth(40);
        if (ImGui.Combo("<=mKey ", ref mana_index, flist, flist.Length)) {
            ui.sett.mana_flask_key = ui.flask_keys[mana_index];
            ui.sett.Save();
        }

        var mana_cast_price = ui.sett.mana_cast_price;
        if (ui.worker != null)
            mana_cast_price = ui.worker.main.mana_cost;
        ImGui.SetNextItemWidth(60);
        ImGui.SameLine();
        if (ImGui.SliderInt("<=Cast price   ", ref mana_cast_price, 5, 140)) {
            ui.sett.Save();
        }
        ImGuiExt.ToolTip("Set the remaining amount of mana " +
            "\nbelow which the flask will be used (default==20)\n" +
            "Note: if you have configured Worker, \n" +
            "this value is taken automatically");

      
        ImGui.SameLine();
        if (ui.worker != null) {
            if (ImGui.Checkbox("Auto", ref ui.worker.b_mana_use_auto)) {
                ui.worker.Save();
            }
        }
        else {
            if (ImGui.Checkbox("Auto", ref ui.sett.b_mana_use_auto)) {
                ui.sett.Save();
            }
        }
        
        ImGuiExt.ToolTip("automatically cast in the presence of danger nearby\n" +
            "For example, if there is a mod on the map...\n" +
            "Players cannot regenerate life, mana and ES.\n" +
            "Note: Vote for automatic activation of this option under the right conditions");
    }
}
