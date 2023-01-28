using System.Diagnostics;
namespace Stas.Utils;

public abstract class iSave {
    public virtual string fname => tname + ".sett";
    public string tname { get { return GetType().Name; } }
    public string tname_full { get { return GetType().FullName; } }
    public DateTime created_date { get; }
    public iSave() { //_extension
        created_date = DateTime.Now;
    }
    public virtual T Load<T>(Action if_err = null) where T : iSave, new() {
        try {
            if (!File.Exists(fname)) {
                ut.AddToLog(tname + ".loading err: Not founf=[" + fname + "]", MessType.Error);
                FILE.SaveAsJson(this, fname);
                return new T();
            }
            else
                return FILE.LoadJson<T>(fname, if_err);
        }
        catch (Exception) {
            if (if_err == null) {
                File.Delete(fname);
                FILE.SaveAsJson(this, fname);
                return new T();
            }
            else {
                if_err.Invoke();
                return null;
            }
        }
    }
    public virtual void Save() {
        FILE.SaveAsJson(this, fname);
    }
    public override string ToString() {
        return tname;
    }
}
public abstract class iSett : iSave {
    public override T Load<T>(Action if_err = null) {
        Debug.Assert(!string.IsNullOrEmpty(fname) && fname.EndsWith(".sett"));
        return base.Load<T>(if_err);
    }
    public override void Save() {
        Debug.Assert(!string.IsNullOrEmpty(fname) && fname.EndsWith(".sett"));
        base.Save();
    }
}
