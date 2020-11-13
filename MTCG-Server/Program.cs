using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_Server {
    class Program {
        static void Main(string[] args) {
            MessageHandler MH = new MessageHandler();
            Listener lt = new Listener(MH);
            lt.InitListener();
        }
    }
}
