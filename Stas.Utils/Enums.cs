using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stas.Utils {
    public enum State : byte {
        App_started,
        Conn_check,
        Login,
        Test,
        ServerDown
       
    }
}
