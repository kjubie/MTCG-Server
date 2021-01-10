using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MTCG_Server {
    public class Program {
        static void Main(string[] args) {
            Manager ma = new Manager();

            BattleConnection BC = new BattleConnection();

            Thread ctThread = new Thread(() => BC.StartBattleConnection(ref ma));
            ctThread.Start();

            Listener lt = new Listener(ref ma);
            lt.InitListener();

            ctThread.Join();
        }
    }
}
