using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_Server {
    public class BattleHandler {
        private BattleConnection BC;
        public RequestContext RC;
        private UserHandler UH;
        private Battle B;

        public Socket client1;
        private User user1;
        private int user1CanPick = 0;
        private int user1Pick = -1;

        public Socket client2;
        private User user2;
        private int user2CanPick = 0;
        private int user2Pick = -1;

        int roundCount;

        public BattleHandler(BattleConnection BC, RequestContext RC, Socket client1, Socket client2, string username1, string username2) {
            this.BC = BC;
            this.RC = RC;
            this.client1 = client1;
            this.client2 = client2;

            BC.ma.Users.TryGetValue(username1, out user1);
            BC.ma.Users.TryGetValue(username2, out user2);

            roundCount = 0;

            B = new Battle(ref user1, ref user2);

            UH = new UserHandler(ref BC.ma);
            B.StartBattle();
            Play();
        }

        ~BattleHandler() {

        }

        private void Play() {
            while (roundCount <= 100) {
                ++roundCount;
                B.Round();

                if (B.battleInProgress == 1) {
                    BC.SendBody("\nNew Handcards:\n" + B.HandCardsToString(user1.name), ref client1);
                    BC.SendBody("\nNew Handcards:\n" + B.HandCardsToString(user2.name), ref client2);

                    BC.client1.Close();
                    BC.client1 = null;

                    BC.client2.Close();
                    BC.client2 = null;

                    Console.WriteLine("Rewaiting");
                    BC.Rewait(this);

                    user1Pick = -1;
                    user2Pick = -1;
                } else {
                    break;
                }
            }

            if (roundCount > 100)
                B.EndBattle(0, ref user1, ref user2);
            else 
                B.EndBattle(0, ref user1, ref user2);

            BC.SendBody(B.roundEndText + "\n", ref client1);
            BC.SendBody(B.roundEndText + "\n", ref client2);

            client1.Close();
            client2.Close();

            BC.Reset();
        }

        public void DoRequest(ref Socket client, string usernameToAuth) {
            string resMassage;
            string username;

            string resource = RC.Resource;
            string content = RC.Body;

            if (resource.Equals("/pick")) {
                //try {
                if (UH.AuthorizeUser(this, out resMassage, out username) == 0) {
                    if (username.Equals(usernameToAuth)) {
                        User user;
                        BC.ma.Users.TryGetValue(username, out user);
                            
                        if (user.name.Equals(user1.name)) {
                            user1Pick = Int32.Parse(content);
                            Console.WriteLine("User 1 picks: " + user1Pick);
                        } else if (user.name.Equals(user2.name)) {
                            user2Pick = Int32.Parse(content);
                            Console.WriteLine("User 2 picks: " + user2Pick);
                        }

                        //BC.SendHead(200, "text/plain", 120, ref client);
                        BC.SendBody("Picks done!\n", ref client);

                        if (user1Pick != -1 && user2Pick != -1) {
                            Console.WriteLine("Cards Chosen!");
                            B.ChooseCard(user1Pick, user2Pick);
                            BC.SendBody(B.roundEndText + "\n", ref client1);
                            BC.SendBody(B.roundEndText + "\n", ref client2);
                        }
                    } else
                        BC.SendResponse(401, "text/plain", "Unauthorized!", ref client);
                } else
                    BC.SendResponse(401, "text/plain", resMassage, ref client);
                //} catch {
                    //BC.SendResponse(400, "text/plain", "Bad Request!", ref client);
                //}
            }
        }
    }
}
