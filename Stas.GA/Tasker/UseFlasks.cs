using System;
using System.Diagnostics;

namespace Stas.GA {
    public abstract partial class aTasker {
        DateTime next_m_use; //last mana flask used
        public void UseManaFlask() {
            Debug.Assert(ui.worker != null);
            if (ui.worker == null)
                return;
            var low_mana = ui.life.Mana.Current < ui.worker.main.mana_cost;
            var can_use = DateTime.Now > next_m_use;
            var auto_mana = ui.danger > 0 && ui.worker.b_mana_use_auto;
            if (can_use &&( low_mana || auto_mana)) {
                Keyboard.KeyPress(ui.worker.mana_flask_key, "Use mana flask", true);
                next_m_use = DateTime.Now.AddMilliseconds(ui.worker.mana_flask_ms);
            }
        }
        DateTime next_l_use;//next life flask use
        public void UseLifeFlask(bool dont_check = false) { //dont_check =>if bot use if from leader comand
            if (ui.worker == null || ui.worker.b_use_low_life || ui.life == null)
                return;
            var low_life = ui.life.Health.CurrentInPercent < ui.worker.mim_life_percent;
            bool can_use = DateTime.Now > next_l_use;
            if (dont_check || (low_life && can_use)) {
                Keyboard.KeyPress(Keys.E, "CheckLife");
                next_l_use = DateTime.Now.AddMilliseconds(ui.worker.life_flask_ms);
                ui.warning = "Use Life potion was OK";
            }
        }
    }
}
