namespace Stas.GA;
internal class Projectile : EntComp {
    public override string tName => "Positioned";

    public Projectile(IntPtr ptr) : base(ptr) {
    }
   
    internal override void Tick(IntPtr ptr, string from=null) {
        Address = ptr;
        if (Address == IntPtr.Zero)
            return;
    }
  
    protected override void Clear() {
        base.Clear();
    }
}
