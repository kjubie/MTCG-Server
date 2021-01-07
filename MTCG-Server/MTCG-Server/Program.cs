using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTCG_Server {
    public class Program {
        static void Main(string[] args) {
            MessageHandler MH = new MessageHandler();
            Listener lt = new Listener(MH);
            lt.InitListener();
        }
    }
}
