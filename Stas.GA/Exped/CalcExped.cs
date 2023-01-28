using System.Threading.Tasks;
using V2 = System.Numerics.Vector2;
using V3 = System.Numerics.Vector3;
using sh = Stas.GA.SpriteHelper;

namespace Stas.GA;
public partial class AreaInstance {
    List<Entity> exped_key_frame = new List<Entity>();
    List<Entity> exped_beams_frame = new List<Entity>();
    public StaticMapItem exped_detonator => static_items.Values.FirstOrDefault(i => i.m_type == miType.ExpedDeton);
    //TODO exped_key_frame нужно сделать в кеш, чтобы они не пропадали если их не видно 
    public void CalcExped() {
        if (exped_detonator == null)
            return;
        if (exped_keys.Count != exped_key_frame.Count) {
            exped_keys = new(exped_key_frame);
            exped_beams = new(exped_beams_frame);
        }
    }
}
