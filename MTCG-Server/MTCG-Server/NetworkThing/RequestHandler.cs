using System.Collections.Generic;
using System.Linq;

namespace MTCG_Server {
    public class RequestHandler {
        private Listener TcpL { get; set; }
        public RequestContext RC;
        private UserHandler UH;

        public RequestHandler(Listener TcpL, RequestContext RC) {
            this.TcpL = TcpL;
            this.RC = RC;
            UH = new UserHandler(ref TcpL.ma);
        }
        
        /*
         * Decides with function to execute
         */
        public void DoRequest() {
            switch (RC.Verb) {
                case "GET":
                    Get(RC.Resource);
                    break;
                case "POST":
                    Post(RC.Resource, RC.Body);
                    break;
                case "PUT":
                    Update(RC.Resource, RC.Body);
                    break;
                case "DELETE":
                    Delete(RC.Resource);
                    break;
            }
        }

        /*
         * Handler for GET
         * 
         * @params:
         *      - resource: Requested resource
         */
        private void Get(string resource) {
            string resMassage = "";
            string username;

            if (resource.Equals("/users")) {
                List<string> keyList = new List<string>(TcpL.ma.Users.Keys);
                foreach (var l in keyList)
                    resMassage += "\nKey: " + l;
                TcpL.SendResponse(200, "text/plain", "\n" + resMassage);
            } else if (resource.Contains("/users/")) {
                string[] splited = resource.Split('/');
                if (UH.AuthorizeUser(this, out resMassage, out username) == 0) {
                    User user;
                    TcpL.ma.Users.TryGetValue(splited[2], out user);
                    TcpL.SendResponse(200, "text/plain", "\n" + user.ShowStats());
                } else
                    TcpL.SendResponse(401, "text/plain", resMassage);
            } else if (resource.Equals("/cards")) {
                if (UH.AuthorizeUser(this, out resMassage, out username) == 0) {
                    User user;
                    TcpL.ma.Users.TryGetValue(username, out user);
                    TcpL.SendResponse(200, "text/plain", "\n" + user.StackToString());
                } else
                    TcpL.SendResponse(401, "text/plain", resMassage);
            } else if (resource.Equals("/deck")) {
                if (UH.AuthorizeUser(this, out resMassage, out username) == 0) {
                    User user;
                    TcpL.ma.Users.TryGetValue(username, out user);
                    TcpL.SendResponse(200, "text/plain", "\n" + user.DeckToString());
                } else
                    TcpL.SendResponse(401, "text/plain", resMassage);
            } else if (resource.Equals("/score")) {
                if (UH.AuthorizeUser(this, out resMassage, out username) == 0) {
                    TcpL.SendResponse(200, "text/plain", "\n" + buildScoreboardString());
                } else
                    TcpL.SendResponse(401, "text/plain", resMassage);
            } else if (resource.Equals("/stats")) {
                if (UH.AuthorizeUser(this, out resMassage, out username) == 0) {
                    User user;
                    TcpL.ma.Users.TryGetValue(username, out user);
                    TcpL.SendResponse(200, "text/plain", "\n" + user.ShowStats());
                } else
                    TcpL.SendResponse(401, "text/plain", resMassage);
            } else
                TcpL.SendResponse(400, "text/plain", "Bad Request!"); ;
        }

        /*
         * Handler for POST
         * 
         * @params:
         *      - resource: resource
         *      - content: http body
         */
        private void Post(string resource, string content) {
            string resMassage;
            string username;

            if (resource.Equals("/save")) {
                TcpL.ma.Save();
                TcpL.SendResponse(200, "text/plain", "Saved!"); ;
            }

            if (resource.Equals("/users")) {
                try {
                    if (RC.values["Content-Type"].Equals("application/json;") || RC.values["Content-Type"].Equals("application/json"))    //Check if content type is 'text/plain'
                        if(UH.AddUser(content, out resMassage) == 0)
                            TcpL.SendResponse(200, "text/plain", resMassage);
                        else
                            TcpL.SendResponse(400, "text/plain", resMassage);
                    else
                        TcpL.SendResponse(400, "text/plain", "Bad Content ElementType!");  //Return error when content type is not 'text/plain'
                } catch {
                    TcpL.SendResponse(400, "text/plain", "Bad Request!"); //Return error on bad request
                }
            }else if (resource.Equals("/sessions")) {
                try {
                    if (RC.values["Content-Type"].Equals("application/json;") || RC.values["Content-Type"].Equals("application/json"))    //Check if content type is 'text/plain'
                        if (UH.LoginUser(content, out resMassage) == 0)
                            TcpL.SendResponse(200, "text/plain", resMassage);
                        else
                            TcpL.SendResponse(400, "text/plain", resMassage);
                    else
                        TcpL.SendResponse(400, "text/plain", "Bad Content ElementType!");  //Return error when content type is not 'text/plain'
                } catch {
                    TcpL.SendResponse(400, "text/plain", "Bad Request!"); //Return error on bad request
                }
            } else if (resource.Equals("/transactions/packages")) {
                try {
                    if (UH.AuthorizeUser(this, out resMassage, out username) == 0) {
                        User user;
                        TcpL.ma.Users.TryGetValue(username, out user);

                        if (user.BuyPack(TcpL.ma.Cards, out resMassage) == 0)
                            TcpL.SendResponse(200, "text/plain", "\n" + resMassage);
                        else
                            TcpL.SendResponse(400, "text/plain", "\nYou cannot afford any Packages!");
                    } else
                        TcpL.SendResponse(401, "text/plain", "Authorization Requiered!");
                } catch {
                    TcpL.SendResponse(400, "text/plain", "Bad Request!"); //Return error on bad request
                }
            } else if (resource.Equals("/battles")) {
                try {
                    if (UH.AuthorizeUser(this, out resMassage, out username) == 0) {
                        User user;
                        TcpL.ma.Users.TryGetValue(username, out user);
                    } else
                        TcpL.SendResponse(401, "text/plain", "Authorization Requiered!");
                } catch {
                    TcpL.SendResponse(400, "text/plain", "Bad Request!"); //Return error on bad request
                }
            }
        }

        /*
         * Hander for PUT
         * 
         * @params:
         *      - resource: resource
         *      - msg: http body
         */
        private void Update(string resource, string msg) {
            string resMassage;
            string username;

            if (resource.Contains("/users/")) {   //Check if resource is correct (I just noticed: '127.0.0.1/wdsahfhh/message/1' would also work but then the code below would return an error so it doesnt)
                string[] splited = resource.Split('/'); //Split the resource at '/' 
                try {
                    if (RC.values["Content-Type"].Equals("application/json;") || RC.values["Content-Type"].Equals("application/json"))
                        if (UH.AuthorizeUser(this, out resMassage, out username) == 0) {
                            if (username.Equals(splited[2])) {
                                UH.UpdateUser(username, msg, out resMassage);
                                TcpL.SendResponse(404, "text/plain", resMassage);
                            } else {
                                TcpL.SendResponse(404, "text/plain", "Wrong User!");
                            }
                        } else
                            TcpL.SendResponse(404, "text/plain", resMassage);
                    else
                        TcpL.SendResponse(400, "text/plain", "Bad Content Type!");

                } catch {
                    TcpL.SendResponse(404, "text/plain", "Invalid User!");
                }
            } else if (resource.Contains("/deck")) {
                if (UH.AuthorizeUser(this, out resMassage, out username) == 0) {
                    User user;
                    TcpL.ma.Users.TryGetValue(username, out user);

                    if(user.UpdateDeck(msg) == 0)
                        TcpL.SendResponse(401, "text/plain", "Deck Updated!");
                    else
                        TcpL.SendResponse(401, "text/plain", "You either dont have the Cards in your Collection or the Card names are invalid!");
                } else
                    TcpL.SendResponse(401, "text/plain", resMassage);
            } else
                TcpL.SendResponse(400, "text/plain", "Bad Request!");
        }

        private void Delete(string resource) {  //Look at the funtions above, at this point you should understand what this does
            ;
        }

        /*
         * Builds the Scoreboard
         * 
         * @return:
         *      - The Scoreboard in String Format
         */
        private string buildScoreboardString() { //Wusste nicht wo sonst hin mit der function
            string scoreboardString = "\nScoreboard:\n";

            Dictionary<string, int> entry = new Dictionary<string, int>();

            foreach (var user in TcpL.ma.Users.Values) {
                entry.Add(user.name, user.elo);
            }

            foreach (KeyValuePair<string, int> entryList in entry.OrderBy(key => key.Value)) {
                scoreboardString += "Name: " + entryList.Key + " Elo: " + entryList.Value + "\n";
            }

            return scoreboardString;
        }
    }
}