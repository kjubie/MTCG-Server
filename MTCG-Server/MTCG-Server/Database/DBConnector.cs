using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace MTCG_Server {
    public class DBConnector {
        private string ConnectionString;
        public NpgsqlConnection SqlConnection;

        public DBConnector(string host, string username, string password, string database) {
            ConnectionString = "Host=" + host + ";Username=" + username + ";Password=" + password + ";Database=" + database;
            SqlConnection = new NpgsqlConnection(ConnectionString);
            SqlConnection.Open();
        }

        ~DBConnector() {
            SqlConnection.Close();
        }

        public void LoadCards(ref Dictionary<string, Card> cards, Types types) {
            string cardsSelect = "select * from card";
            NpgsqlCommand cmd = new NpgsqlCommand(cardsSelect, SqlConnection);
            NpgsqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read()) {
                Dictionary<string, Effect> effects = new Dictionary<string, Effect>();
                LoadEffects(reader.GetString(3), ref effects);

                types.types.TryGetValue(reader.GetString(2), out ElementType t);

                if (reader.GetInt32(4) == 0)
                    cards.Add(reader.GetString(0), new SpellCard(reader.GetString(0), t, reader.GetInt32(1), effects));
                else {
                    LoadRace(reader.GetString(5), out Race race);

                    cards.Add(reader.GetString(0), new MonsterCard(reader.GetString(0), t, reader.GetInt32(1), effects, race));
                }
            }

            reader.Close();
        }

        public void LoadUsers(ref Dictionary<string, User> users, Dictionary<string, Card> cards) {
            string usersCountSelect = "select count(*) from mtcguser";
            NpgsqlCommand usersCountCmd = new NpgsqlCommand(usersCountSelect, SqlConnection);
            int count = Int32.Parse(usersCountCmd.ExecuteScalar().ToString());

            for (int i = 1; i <= count; ++i) {
                string usersSelect = "select * from (select username, password, credits, elo, row_number () over (order by username) from mtcguser) as mtcguseraccount where row_number = " + i;
                NpgsqlCommand usersCmd = new NpgsqlCommand(usersSelect, SqlConnection);
                NpgsqlDataReader readerUser = usersCmd.ExecuteReader();

                readerUser.Read();

                string username = readerUser.GetString(0);
                string password = readerUser.GetString(1);
                int credits = readerUser.GetInt32(2);
                int elo = readerUser.GetInt32(3);

                readerUser.Close();

                Stack stack = new Stack();
                string stackSelect = "select * from stackcards where username = '" + username + "'";
                NpgsqlCommand stackCmd = new NpgsqlCommand(stackSelect, SqlConnection);
                NpgsqlDataReader readerStack = stackCmd.ExecuteReader();

                while (readerStack.Read()) {
                    cards.TryGetValue(readerStack.GetString(1), out Card card);
                    stack.AddCard(card);
                }

                readerStack.Close();

                Deck deck = new Deck();
                string deckSelect = "select * from deckcards where username = '" + username + "'";
                NpgsqlCommand deckCmd = new NpgsqlCommand(deckSelect, SqlConnection);
                NpgsqlDataReader readerDeck = deckCmd.ExecuteReader();

                while (readerDeck.Read()) {
                    cards.TryGetValue(readerDeck.GetString(1), out Card card);
                    deck.AddCard(card);
                }

                readerDeck.Close();

                users.Add(username, new User(username, password, credits, elo, stack, deck));
            }
        }

        public void SaveUsers(ref Dictionary<string, User> users) {
            foreach(var user in users) {
                NpgsqlCommand cmdUser = new NpgsqlCommand();
                cmdUser.Connection = SqlConnection;
                cmdUser.CommandText = "insert into mtcguser values('" + user.Value.name + "', '" + user.Value.getPassword() + "', '" + user.Value.credits + "', '" + user.Value.elo + "') on conflict(username) do update set password = excluded.password, credits = excluded.credits, elo = excluded.elo";
                cmdUser.ExecuteNonQuery();

                user.Value.GetStack().GetCards(out Dictionary<string, CardInStack> cardsStack);
                foreach (var stackCard in cardsStack) {
                    Console.WriteLine("insert into stackCards values ('" + user.Value.name + "', '" + stackCard.Value.GetCard().name + "') on conflict(cardname) do nothing");
                    cmdUser.CommandText = "insert into stackCards values ('" + user.Value.name + "', '" + stackCard.Value.GetCard().name + "') on conflict(cardname) do nothing";
                    cmdUser.ExecuteNonQuery();
                }

                user.Value.GetDeck().GetCards(out Dictionary<string, Card> cardsDeck);
                foreach (var deckCard in cardsDeck) {
                    Console.WriteLine("insert into deckCards values ('" + user.Value.name + "', '" + deckCard.Value.name + "') on conflict(cardname) do nothing");
                    cmdUser.CommandText = "insert into deckCards values ('" + user.Value.name + "', '" + deckCard.Value.name + "') on conflict(cardname) do nothing";
                    cmdUser.ExecuteNonQuery();
                }
            }
        }

        public void LoadRace(string raceName, out Race race) {
            switch (raceName) {
                case "Human":
                    race = new Human();
                    break;
                case "Dragon":
                    race = new Dragon();
                    break;
                case "Wizzard":
                    race = new Wizzard();
                    break;
                case "Knight":
                    race = new Knight();
                    break;
                case "Elve":
                    race = new Elve();
                    break;
                case "Kraken":
                    race = new Kraken();
                    break;
                case "Ork":
                    race = new Ork();
                    break;
                case "Goblin":
                    race = new Goblin();
                    break;
                default:
                    race = new Human();
                    break;
            }
        }

        public void LoadEffects(string effectName, ref Dictionary<string, Effect> effects) {
            string[] effectsArray = effectName.Split(',');

            foreach (var effect in effectsArray) {
                switch (effect) {
                    case "DoublePower":
                        effects.Add(effect, new DoublePower());
                        break;
                    case "NegateType":
                        effects.Add(effect, new NegateType());
                        break;
                    case "Overwehlm":
                        effects.Add(effect, new Overwehlm());
                        break;
                    case "Silence":
                        effects.Add(effect, new Silence());
                        break;
                    case "Spellshield":
                        effects.Add(effect, new Spellshield());
                        break;
                    case "Undead":
                        effects.Add(effect, new Undead());
                        break;
                    case "OnFire":
                        effects.Add(effect, new OnFire());
                        break;
                    case "SetOnFire":
                        effects.Add(effect, new SetOnFire());
                        break;
                    case "-":
                        break;
                    default:
                        try {
                            effects.Add("Buff", new Buff(Int32.Parse(effect)));
                        } catch (Exception e) {
                            Console.WriteLine("Error while loading Effect " + effect);
                        }
                        break;
                }
            }
        }
    }
}