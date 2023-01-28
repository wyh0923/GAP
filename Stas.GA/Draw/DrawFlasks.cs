using ImGuiNET;
using System.Runtime.InteropServices;
using V2 = System.Numerics.Vector2;
namespace Stas.GA; 

partial class DrawMain {
    
    void DrawFlasks() {
        CheckHotKeys();

        ImGui.Separator();
        if (ImGui.Checkbox("2Left ", ref ui.sett.b_use_left_flasks)) {
            ui.sett.Save();
        }
        ImGuiExt.ToolTip("Pressing on this will click the two left flasks" +
      "\nI use them to press [Silver] +[mana] flask " +
      "\nbefore pooling the next group of monsters");
        ImGui.SameLine();
        ImGui.SetNextItemWidth(40);
        if (ImGuiExt.NonContinuousEnumComboBox("=[" + ui.hot_keys.use_flask_in_slot1.Key
            + "]+[" + ui.hot_keys.use_flask_in_slot2.Key + "]   ", ref ui.sett.two_left_flask_key)) {
            ui.sett.Save();
        }

        ImGui.SameLine();
        if (ImGui.Checkbox("2Right", ref ui.sett.b_use_right_flasks)) {
            ui.sett.Save();
        }
        ImGuiExt.ToolTip("Pressing on this button will click the two right flasks" +
           "\nI use them to press [Armor] +[Dodge] flasks " +
           "\nwhen I'm inside a group of monsters");
        ImGui.SameLine();
        ImGui.SetNextItemWidth(40);
        if (ImGuiExt.NonContinuousEnumComboBox("=[" + ui.hot_keys.use_flask_in_slot4.Key + "]+["
            + ui.hot_keys.use_flask_in_slot5.Key + "]", ref ui.sett.two_right_flask_key)) {
            ui.sett.Save();
        }

        ImGui.Separator();
        DrawManaFlaskSetup();

        ImGui.Separator();
        DrawLifeFlaskSetup();
       
        ImGui.Separator();
        var flist = ui.flask_keys.Select(x => x.ToString()).ToArray();
        if (ImGui.Checkbox("Silver", ref ui.sett.b_use_silver_flask)) {
            ui.sett.Save();
        }
        ImGuiExt.ToolTip("Automatically uses this flask if there are charges on it" +
            "\n and the nearest enemies further than 100 grid points");

        var silver_flask_key = ui.sett.silver_flask_key;
        if(ui.worker!=null)
            silver_flask_key = ui.worker.silver_flask_key;
        int silver_index = ui.flask_keys.IndexOf(silver_flask_key);
        ImGui.SetNextItemWidth(50);
        ImGui.SameLine();
        if (ImGui.Combo("<=sKey ", ref silver_index, flist, flist.Length)) {
            ui.sett.silver_flask_key = ui.flask_keys[silver_index];
            ui.sett.Save();
        }
        ImGui.SameLine();
        ImGui.SetNextItemWidth(60);
        if (ImGui.SliderInt("<=Set grid dist  ", ref ui.sett.silver_gdist, 50, 250)) {
            ui.sett.Save();
        }
        ImGuiExt.ToolTip("sets the triggering distance (80gp =~ 1 screen)");

    }
}