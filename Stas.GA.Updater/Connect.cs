using Stas.Utils;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace Stas.GA.Updater;

public partial class ui {
    Session sess;
    public void Connect() {
        var con_thread = new Thread(async() => {
            while (b_running) {
                try {
                    switch (state) {
                        case State.App_started:
                            state = State.Conn_check;
                            AddToLog("Check server online");
                            TcpClient tc = new TcpClient();
                            IPAddress.TryParse(sett.srv_ip, out var srv_ip);
                            await tc.ConnectAsync(srv_ip, sett.srv_port);
                            //sess = new Session(tc, Decode, AddToLog, null);
                            //sess.Send(Opcode.Login, sett.login + "|123");
                            break;
                    }

                    //udp_screen.Send(Opcode.ScreenPing);
                }
                catch (Exception ex) {
                    AddToLog("Err: " + ex.Message);
                }
                Thread.Sleep(3000);
            }
        });
        con_thread.IsBackground = true;
        con_thread.Start();
       
    }
}