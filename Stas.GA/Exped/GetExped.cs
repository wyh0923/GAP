﻿using System.Threading.Tasks;
using V2 = System.Numerics.Vector2;
using V3 = System.Numerics.Vector3;
using sh = Stas.GA.SpriteHelper;

namespace Stas.GA;
public partial class AreaInstance {
    MapItem GetExped(Entity e, MapItem mi) {
        mi.info = pa_info(e);
        var key = e.GetKey;
        if (mi.info == "ExpeditionDetonator") {
            if (e.IsTargetable)
                return asStaticMapItem(e, miType.ExpedDeton, MapIconsIndex.ExpeditionDetonator);
            else {
                static_items.TryRemove(key, out _);
                return null;
            }
        }
        if (ui.curr_map.exped_detonator == null || ui.curr_map.danger > 0) {
            static_items.TryRemove(key, out _);
            return null;
        }
        if (mi.info == "ExpeditionMarker") {
            var mark = GetExpedMarker(e);
            if (mark != null) {//free
               
            }
            return mark;
        }
        else if (mi.info.Contains("Entrance")) {
            return asStaticMapItem(e, miType.door, MapIconsIndex.Red_door);
        }
        else if (mi.info == "ExpeditionRelic") {
            return GetExpedRelic(e);
        }
        else if (mi.info.Contains("ExpeditionExplosive")) { //ExpeditionConnectorPole
            if (!mi.info.Contains("Fuse"))
                exped_key_frame.Add(e);
            else
                exped_beams_frame.Add(e);
            return null;
        }
        else if (mi.info.Contains("ExpeditionStash")) {
            mi.uv = sh.GetUV(MapIconsIndex.ExpeditionStash);
        }
        else if (mi.info == "ExpeditionSwapDoodadSulphite")
            mi.uv = sh.GetUV(MapIconsIndex.Sulphite);
        else if (mi.info == "ExpeditionSwapDoodadCosmetic")
            mi.uv = sh.GetUV(MapIconsIndex.Red_door);
        else if (mi.info.EndsWith("Doodad")) {
            return null;
        }
        else {//for debug only //"ExpeditionConnectorPole"
            if (ui.sett.b_develop) //b_develop
                mi.uv = sh.GetUV(MapIconsIndex.unknow);
            else
                return null;
        }
        mi.size = (int)EXT.Lerp(14, 25, (float)ui.sett.map_scale / 20);
        return mi;
    }
}

