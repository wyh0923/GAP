using Stas.Utils;
using System.Diagnostics;
namespace Stas.GA.Updater;

public partial class ui {
    void  Decode(Session sess, Opcode opc, byte[] data) {
        try {
            switch (opc) {

                //case Opcode.Message:
                //    string mess = BYTE.ToString(data);
                //    mes_frame.AddMessage(mess, true);
                //    break;

                //case Opcode.User:
                //    user = JSON.From<User>(data);
                //    state = UserState.Wite;
                //    break;

                //case Opcode.Site:
                //    site = BYTE.ToClass<Site>(data);
                //    state = UserState.Site;
                //    break;
                //case Opcode.Server_down:
                //    state = UserState.Server_down;
                //    break;


                default:
                    throw new Exception("Unknown opcode =" + opc);
            }
        }
        catch (Exception ex) {
            throw new Exception("Decode:" + ex.Message);
        }
    }
}