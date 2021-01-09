using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_Server {
    public class Manager {
        DBConnector DBConnector;

        public Dictionary<string, User> Users;
        public Dictionary<string, Card> Cards;
        Types Types;

        public Manager() {
            DBConnector = new DBConnector("localhost", "postgres", "abru13", "mtcg");
            Types = new Types();

            Users = new Dictionary<string, User>();
            Cards = new Dictionary<string, Card>();

            DBConnector.LoadCards(ref Cards, Types);
            DBConnector.LoadUsers(ref Users, Cards);
        }

        ~Manager() {
            DBConnector.SaveUsers(ref Users);
        }
    }
}
