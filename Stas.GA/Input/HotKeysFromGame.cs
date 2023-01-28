using System.IO;
namespace Stas.GA;

public class HotKeysFromGame {
    List<string> key_combos= new List<string>();
    public bool b_ok => File.Exists(fname);
    string fname { get {
            var end = "production_Config.ini";
            var full = ui.sett.production_config_path;
            if (!full.EndsWith(end)) {
                full = Path.Combine(full, end);
            }
            return full;
        } }
    DateTime last_update = DateTime.MinValue;
    public HotKeysFromGame() {
        var worker = new Thread(() => { 
            while (ui.b_running) {
                if (!b_ok) {
                    ui.AddToLog("HotKeysFromGame: production_Config.ini NOT found", MessType.Critical);
                    Thread.Sleep(5000);
                    continue;
                }
                var curr_update  = File.GetLastWriteTime(fname);
                if (last_update != curr_update) {
                    Reload();
                }
                Thread.Sleep(1000);
            }
        });
        worker.IsBackground= true;
        worker.Start();
    }
    public void Reload() {
        last_update = File.GetLastWriteTime(fname);
        var source = File.ReadAllText(fname);
        key_combos = source.Split('\n').ToList();
        use_bound_skill1 = GetCombo("use_bound_skill1");
        use_bound_skill2 = GetCombo("use_bound_skill2");
        use_bound_skill3 = GetCombo("use_bound_skill3");
        use_bound_skill4 = GetCombo("use_bound_skill4");
        use_bound_skill5 = GetCombo("use_bound_skill5");
        use_bound_skill6 = GetCombo("use_bound_skill6");
        use_bound_skill7 = GetCombo("use_bound_skill7");
        use_bound_skill8 = GetCombo("use_bound_skill8");
        use_bound_skill9 = GetCombo("use_bound_skill9");
        use_bound_skill10 = GetCombo("use_bound_skill10");
        use_bound_skill11 = GetCombo("use_bound_skill11");
        use_bound_skill12 = GetCombo("use_bound_skill12");
        use_bound_skill13 = GetCombo("use_bound_skill13");

        use_flask_in_slot1 = GetCombo("use_flask_in_slot1");
        use_flask_in_slot2 = GetCombo("use_flask_in_slot2");
        use_flask_in_slot3 = GetCombo("use_flask_in_slot3");
        use_flask_in_slot4 = GetCombo("use_flask_in_slot4");
        use_flask_in_slot5 = GetCombo("use_flask_in_slot5");
    }

    KeysCombo GetCombo(string action) {
        var raw_combo = key_combos.FirstOrDefault(kk => kk.Split("=")[0]==action);
        if (raw_combo == null)
            return new KeysCombo(Keys.None, Keys.None);
        var act = raw_combo.Split("=");
        var array =new string[] { };
        if (act.Length==2)
            array = act[1].Split(" ");
        switch (array.Length) {
            case 0:
                return new KeysCombo(Keys.None, Keys.None);
            case 1: {
                    int key;
                    if (int.TryParse(array[0], out key)) {
                        return new KeysCombo((Keys)key, Keys.None);
                    }
                    return new KeysCombo(Keys.None, Keys.None);
                }
            case 2: {
                    Keys key2 = Keys.None;
                    int num;
                    if (int.TryParse(array[0], out num)) {
                        key2 = (Keys)num;
                    }
                    Keys modifier = Keys.None;
                    int num2;
                    if (int.TryParse(array[1], out num2)) {
                        switch (num2) {
                            case 1:
                                modifier = Keys.ShiftKey;
                                break;
                            case 2:
                                modifier = Keys.ControlKey;
                                break;
                            case 3:
                                modifier = Keys.Menu;
                                break;
                        }
                    }
                    return new KeysCombo(key2, modifier);
                }
            default:
                return new KeysCombo(Keys.None, Keys.None);
        }
    }
    static KeysCombo def = new KeysCombo(Keys.None, Keys.None); 
    public KeysCombo use_bound_skill1 { get; private set; } = def;
    public KeysCombo use_bound_skill2 { get; private set; } = def;
    public KeysCombo use_bound_skill3 { get; private set; } = def;
    public KeysCombo use_bound_skill4 { get; private set; } = def;
    public KeysCombo use_bound_skill5 { get; private set; } = def;
    public KeysCombo use_bound_skill6 { get; private set; } = def;
    public KeysCombo use_bound_skill7 { get; private set; } = def;
    public KeysCombo use_bound_skill8 { get; private set; } = def;
    public KeysCombo use_bound_skill9 { get; private set; } = def;
    public KeysCombo use_bound_skill10 { get; private set;  } = def;
    public KeysCombo use_bound_skill11 { get; private set; } = def;
    public KeysCombo use_bound_skill12 { get; private set; } = def;
    public KeysCombo use_bound_skill13 { get; private set; } = def;

    public KeysCombo use_flask_in_slot1 { get; private set; } = def;
    public KeysCombo use_flask_in_slot2 { get; private set; } = def;
    public KeysCombo use_flask_in_slot3 { get; private set; } = def;
    public KeysCombo use_flask_in_slot4 { get; private set; } = def;
    public KeysCombo use_flask_in_slot5 { get; private set; } = def;

}

public class KeysCombo {
    public Keys Key { get; } = Keys.None;
    public Keys Modifier { get; } = Keys.None;

    internal KeysCombo(Keys key, Keys modifier) {
        this.Key = key;
        this.Modifier = modifier;
    }
    public override string ToString() {
        var mod = "";
        if (Modifier != Keys.None)
            mod = "[" + Modifier + "] ";
        return mod+Key;
    }
}
