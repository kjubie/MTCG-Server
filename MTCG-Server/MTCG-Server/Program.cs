using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTCG_Server {
    public class Program {
        static void Main(string[] args) {
            Manager ma = new Manager();

            MessageHandler MH = new MessageHandler();
            Listener lt = new Listener(MH, ref ma);
            lt.InitListener();
        }
    }
}
