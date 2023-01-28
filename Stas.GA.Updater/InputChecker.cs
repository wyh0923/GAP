using Stas.Utils;
using System.Diagnostics;
using System.Text;
using V2 = System.Numerics.Vector2;
using V3 = System.Numerics.Vector3;
namespace Stas.GA.Updater;

public partial class InputChecker  {
    Stopwatch sw = new Stopwatch();
    StringBuilder sb = new StringBuilder();
    Random R = new Random();
    public InputChecker() {
        var work = new Thread(() => {
            while (ui.b_running) {
               

                #region w8ting
                var t_elaps = (int)sw.Elapsed.TotalMilliseconds; //totale elaps
                if (t_elaps < ui.w8 / 2) {
                    Thread.Sleep(ui.w8 / 2 - t_elaps);
                }
                else {
                    ui.AddToLog("InputChecker Big tick time...", MessType.Warning);
                    Thread.Sleep(1);
                }
                #endregion
            }

        });
        work.IsBackground = true;
        work.Start();
    }
}
