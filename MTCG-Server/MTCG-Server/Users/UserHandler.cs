using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Json.Net;
using Newtonsoft.Json;

namespace MTCG_Server {
    public class UserHandler {
        Manager ma;

        public UserHandler(ref Manager ma) {
            this.ma = ma;
        }

        public int AddUser(string jsonUser, out string resMassage) {
            var user = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonUser);

            string username;
            string password;

            user.TryGetValue("Username", out username);
            user.TryGetValue("Password", out password);

            if (ma.Users.ContainsKey(username)) {
                resMassage = "Username already taken!";
                return -1;
            }

            ma.Users.Add(username, new User(username, password));
            Console.WriteLine("Added User");
            resMassage = "Added new User!";

            return 0;
        }

        public int UpdateUser(string userToUpdate, string jsonUser, out string resMassage) {
            var user = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonUser);

            string username;
            string password;

            bool u = user.TryGetValue("Username", out username);
            bool p = user.TryGetValue("Password", out password);

            Console.WriteLine(username);
            Console.WriteLine(userToUpdate);

            User userUpdate;

            if (ma.Users.TryGetValue(userToUpdate, out userUpdate)) {
                if (u) {
                    ma.Users.Remove(userToUpdate);
                    userUpdate.name = username;
                    userUpdate.SetToken(username + "-mtcgToken");
                    ma.Users.Add(username, userUpdate);
                }
                if (p)
                    userUpdate.setPassword(password);
                resMassage = "User updated!";
            } else {
                resMassage = "User does not exitst!";
                return -1;
            }

            return 0;
        }

        public int LoginUser(string jsonUser, out string resMassage) {
            var user = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonUser);

            string username;
            string password;

            user.TryGetValue("Username", out username);
            user.TryGetValue("Password", out password);

            User userToLogIn;

            if (ma.Users.TryGetValue(username, out userToLogIn)) {
                if (userToLogIn.getPassword().Equals(password) && userToLogIn.online == 0) {
                    Console.WriteLine("Logged In: " + username);
                    resMassage = "Successfully logged in! Your token: " + userToLogIn.GetToken();
                    userToLogIn.online = 1;
                } else if (userToLogIn.online == 1) {
                    resMassage = "You are already online!";
                } else {
                    Console.WriteLine("Failed Login: " + username);
                    resMassage = "Login Failed!";
                }
            } else {
                Console.WriteLine("Failed Login: " + username);
                resMassage = "Login Failed!";
            }

            return 0;
        }

        public int AuthorizeUser(RequestHandler rh, out string resMassage, out string userAuth) {
            userAuth = "";
            resMassage = "";

            string tokenAuth;
            if (rh.RC.values.TryGetValue("Authorization", out tokenAuth)) {
                string[] split = tokenAuth.Split('-');

                Console.WriteLine("Auth: " + tokenAuth);
                Console.WriteLine("Auth: " + split[0]);

                User user;
                if(ma.Users.TryGetValue(split[0], out user)) {
                    Console.WriteLine("AuthToken: " + user.GetToken());
                    if (user.online == 0) {
                        resMassage = "You need to login first!";
                        return -1;
                    }

                    if (user.GetToken().Equals(tokenAuth)) {
                        userAuth = user.name;
                        resMassage = "Authorized!";
                        return 0;
                    }
                }
            }

            resMassage = "Authoriziaton Failed!";
            return -1;
        }
    }
}
