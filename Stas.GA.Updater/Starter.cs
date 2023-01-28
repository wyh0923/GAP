using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace Stas.GA.Updater {
    internal class Starter {
        private static async Task Main(string[] args) {
            AppDomain.CurrentDomain.UnhandledException += (sender, exceptionArgs) => {
                var errorText = "Program exited with message:\n " + exceptionArgs.ExceptionObject;
                try {
                    File.AppendAllText("Error.log", $"{DateTime.Now:g} {errorText}\r\n{new string('-', 30)}\r\n");
                }
                catch (Exception) {
                    //cant write to file coz it busy
                }
                Environment.Exit(1);
            };
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            if (Debugger.IsAttached)
                CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            var title_name = ui.sett.title_name + " - Notepad";
            using (ui.overlay = new DrawMain(title_name)) {
                await ui.overlay.Run();
            }
        }
        static void CurrentDomain_ProcessExit(object sender, EventArgs e) {
            ui.overlay?.Dispose();
        }
    }
}
