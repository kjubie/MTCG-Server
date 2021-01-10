using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MTCG_Server {
    public class Manager {
        DBConnector DBConnector;

        public Dictionary<string, User> Users;
        public Dictionary<string, Card> Cards;
        Types Types;

        Thread ctThread;

        public Manager() {
            DBConnector = new DBConnector("localhost", "postgres", "abru13", "mtcg");
            Types = new Types();

            Users = new Dictionary<string, User>();
            Cards = new Dictionary<string, Card>();

            DBConnector.LoadCards(ref Cards, Types);
            DBConnector.LoadUsers(ref Users, Cards);

            ctThread = new Thread(SaveLoop);
            ctThread.Start();
        }

        ~Manager() {
            try {
                ctThread.Abort();
            } catch {
                Console.WriteLine("Error while aborting Thread!");
            }

            DBConnector.SaveUsers(ref Users);
        }

        public void Save() {
            DBConnector.SaveUsers(ref Users);
        }

        public void SaveLoop() {
            while (true) {
                Save();
                Thread.Sleep(60000);
            }
        }
    }
}
