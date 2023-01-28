using ImGuiNET;
using System.Runtime.InteropServices;
using V2 = System.Numerics.Vector2;
namespace Stas.GA;

partial class DrawMain {
    bool b_need_save_path_to_config = false;
    void CheckHotKeys() {
        if (!ui.hot_keys.b_ok) {
            b_need_save_path_to_config = true;
        }
        if (b_need_save_path_to_config) {
            var text = ("Set path to the folder with the production_Config.ini file." +
                  "\r\nIt looks like this: C:\\Users\\poe\\Documents\\My Games\\Path of Exile" +
                  "\r\nEdit the production_config_path value in the Settings.sett file and...");
            DrawInfoOnQuad(text);
            ImGui.SetNextItemWidth(400);
            ImGui.InputText("##Dir", ref ui.sett.production_config_path, 100);
            ImGui.SameLine();
            if (ui.hot_keys.b_ok) {
                if (ImGui.Button("Save")) {
                    ui.sett.Save();
                    ui.hot_keys.Reload();
                    b_need_save_path_to_config = false;
                }
            }
            else {
                ImGuiExt.DrawDisabledButton("Save");
                ImGuiExt.ToolTip("Incorrect file path");
            }
        }
    }
}
