namespace Stas.GA;

public class Animated : EntComp {
    public override string tName => "Animated";
    public Animated(IntPtr address) : base(address) {
        if (address != default)
            Tick(address, tName + "()");
    }
    internal override void Tick(IntPtr ptr, string from=null) {
        Address = ptr;
        if (Address == IntPtr.Zero)
            return;
    }
    public Entity BaseAnimatedObjectEntity { get; private set; } = new Entity();


    protected override void Clear() {
        BaseAnimatedObjectEntity.Tick(IntPtr.Zero);
    }
}