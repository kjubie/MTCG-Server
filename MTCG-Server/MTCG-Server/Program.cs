using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MTCG_Server {
    public class Program {
        static Manager MA;
        static BattleConnection BC;
        static Listener LT;
        static void Main(string[] args) {
            MA = new Manager();

            BC = new BattleConnection();

            Thread ctThread = new Thread(() => BC.StartBattleConnection(ref MA));
            ctThread.Start();

            LT = new Listener(ref MA);
            LT.InitListener();

            ctThread.Join();
        }
    }
}
