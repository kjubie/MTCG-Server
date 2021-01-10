using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MTCG_Server {
    public class BattleConnection {
        private TcpListener Server { get; set; } = null;
        private int Port { get; set; } = 25576;
        private IPAddress Addr { get; set; } = IPAddress.Parse("127.0.0.1");
        private bool Running { get; set; } = false;

        public BattleHandler BH;
        private RequestContext RC;
        public Socket client1 = null;
        public Socket client2 = null;
        private string username1;
        private string username2;
        public Manager ma;

        public BattleConnection() {
            Running = true;
        }

        public BattleConnection(ref Manager ma) {
            Running = true;
            this.ma = ma;
        }

        public BattleConnection(string addr, int port) {
            Running = true;
            Addr = IPAddress.Parse(addr);
            Port = port;
        }

        public void StartBattleConnection(ref Manager ma) {
            this.ma = ma;
            InitListener();
        }

        /*
         * Initializes the socket and waits for connections
         * 
         * @returns:
         *      - 0: On success
         *      - -1: On failure
         */
        public int InitListener() {
            int err = 0;

            try {
                Server = new TcpListener(Addr, Port);
                Server.Start();

                while (Running) {
                    Console.WriteLine("Waiting for Battle Connections...");
                    if (client1 == null) {
                        client1 = Server.AcceptSocket();
                        Console.WriteLine("User 1 Connected!");
                        if (Authorize(ref client1, ref username1) == 0) {
                            SendHead(200, "text/plain", 250, ref client1);
                            SendBody("Authorized for Battle!\n", ref client1);
                        } else {
                            SendResponse(200, "text/plain", "Not Authorized for Battle!", ref client1);
                            client1.Close();
                        }

                    }
                    
                    if (client2 == null) {
                        client2 = Server.AcceptSocket();
                        Console.WriteLine("User 2 Connected!");
                        if (Authorize(ref client2, ref username2) == 0) {
                            SendHead(200, "text/plain", 250, ref client2);
                            SendBody("Authorized for Battle!\n", ref client2);
                        } else {
                            SendResponse(200, "text/plain", "Not Authorized for Battle!", ref client2);
                            client2.Close();
                        }
                    }

                    if(client1 != null && client2 != null)
                        if (client1.Connected && client2.Connected) {
                            BH = new BattleHandler(this, RC, client1, client2, username1, username2);
                        }
                    Running = false;
                }

            } catch (SocketException e) {
                Console.WriteLine(e);
            } finally {
                Server.Stop();
                err = -1;
            }

            return err;
        }

        /*
         * Waits for the Battling Users to send the next request
         * 
         * @params:
         *      - BH: The BattleHandler
         */
        public void Rewait(BattleHandler BH) {
            client1 = null;
            client2 = null;

            while (client1 == null || client2 == null) {
                if (client1 == null) {
                    client1 = Server.AcceptSocket();
                    string username;
                    if (AuthorizeAgain(client1, ref BH, out username) == 0) {
                        SendHead(200, "text/plain", 300, ref client1);
                        SendBody("Authorized for Battle!\n", ref client1);
                        BH.RC = RC;
                        BH.DoRequest(ref client1, username);
                    } else {
                        SendResponse(200, "text/plain", "Not Authorized for Battle!", ref client1);
                        client1.Close();
                    }
                }

                if (client2 == null) {
                    client2 = Server.AcceptSocket();
                    string username;
                    if (AuthorizeAgain(client2, ref BH, out username) == 0) {
                        //BuildContext(ReceiveRequest(ref client));
                        SendHead(200, "text/plain", 300, ref client2);
                        SendBody("Authorized for Battle!\n", ref client2);
                        BH.RC = RC;
                        BH.DoRequest(ref client2, username);
                    } else {
                        SendResponse(200, "text/plain", "Not Authorized for Battle!", ref client2);
                        client2.Close();
                    }
                }
            }
        }

        /*
         * Rests the BattleConnection
         */
        public void Reset() {
            try {
                client1.Close();
                client2.Close();
            } catch {

            }

            client1 = null;
            client2 = null;

            BH = null;
        }

        /*
         * Authorizes the User who wants to Start a Battle
         * 
         * @params:
         *      - client: Socket to which the User connected
         *      - username: Username of the Connected User
         */
        public int Authorize(ref Socket client, ref string username) {
            BuildContext(ReceiveRequest(ref client));

            string tokenAuth;
            if (RC.values.TryGetValue("Authorization", out tokenAuth)) {
                string[] split = tokenAuth.Split('-');

                User user;
                if (ma.Users.TryGetValue(split[0], out user)) {
                    if (user.online == 0) {
                        return -1;
                    }

                    if (user.GetToken().Equals(tokenAuth)) {
                        username = user.name;
                        return 0;
                    }
                }
            }

            return -1;
        }

        /*
         * Authorizes a User again who is in this Battle
         * 
         * @params:
         *      - client: Socket to which the User connected
         *      - BH: The BattleHandler
         *      - username: Username of the Connected User
         */
        public int AuthorizeAgain(Socket client, ref BattleHandler BH, out string username) {
            username = "";
            
            BuildContext(ReceiveRequest(ref client));

            string tokenAuth;
            if (RC.values.TryGetValue("Authorization", out tokenAuth)) {
                string[] split = tokenAuth.Split('-');

                User user;
                if (ma.Users.TryGetValue(split[0], out user)) {
                    if (user.online == 0) {
                        return -1;
                    }

                    if (user.GetToken().Equals(tokenAuth)) {
                        if (user.name.Equals(username1)) {
                            BH.client1 = client;
                            username = username1;
                        } else if (user.name.Equals(username2)) {
                            BH.client2 = client;
                            username = username2;
                        }
                        return 0;
                    }
                }
            }

            return -1;
        }

        /*
         * Fills the request context object with the http request data
         * 
         * @params:
         *      - request: Raw http request data
         */
        public void BuildContext(string request) {
            if (!request.Contains("HTTP"))
                return;

            string[] reqArray = request.Split(new char[0]); //Split the request at spaces
            int i = 0, countSpaces = 0;

            RC = new RequestContext(reqArray[0], reqArray[1], reqArray[2]); //Set request verb (reqArray[0]), resource (reqArray[1]) and version (reqArray[2]) 

            foreach (string element in reqArray) {  //Loop throu each element
                if (element.EndsWith(":") && countSpaces < 3)   //If element ends with ':' e.q.: 'Content-Type:'
                    RC.values.Add(element.Remove(element.Length - 1), reqArray[i + 1]); //Then store this element without the ':' as key and the next element as value
                else if (element.Equals(""))    //Count the empty spaces
                    ++countSpaces;
                else if (!element.Equals("") && countSpaces < 3)    //Reset empty spaces if element is not an empty space
                    countSpaces = 0;
                else if (countSpaces >= 3 && !element.Equals(""))    //If three empty spaces in a row occured we know that the header ended and the body started.
                    RC.Body += element + " ";
                ++i;
            }

            try {
                RC.Body = RC.Body.Remove(int.Parse(RC.values["Content-Length"]), RC.Body.Length - int.Parse(RC.values["Content-Length"])); //Just remove every character from the body that is over the content length
            } catch {
                RC.Body = "";
            }
        }

        /*
         * Reads the bytes from the socket
         * 
         * @returns:
         *      - http request in string format
         */
        public string ReceiveRequest(ref Socket client) {
            string request;

            byte[] bRequest = new byte[1024];
            int i = client.Receive(bRequest, bRequest.Length, 0);
            request = Encoding.ASCII.GetString(bRequest);

            return request;
        }

        /*
         * Sends http response to the client
         * 
         * @params:
         *      - statusCode: http status code
         *      - contentType: http content type. Always 'text/plain' in this case.
         *      - msg: http body
         */
        public void SendResponse(int statusCode, string contentType, string msg, ref Socket client) {
            byte[] bSendHead = Encoding.ASCII.GetBytes(BuildHeader(statusCode, contentType, msg.Length));
            byte[] bSendMsg = Encoding.ASCII.GetBytes(msg);
            client.Send(bSendHead, bSendHead.Length, 0);
            client.Send(bSendMsg, bSendMsg.Length, 0);
        }

        public void SendHead(int statusCode, string contentType, int contentLength, ref Socket client) {
            byte[] bSendHead = Encoding.ASCII.GetBytes(BuildHeader(statusCode, contentType, contentLength));
            client.Send(bSendHead, bSendHead.Length, 0);
        }

        public void SendBody(string msg, ref Socket client) {
            byte[] bSendMsg = Encoding.ASCII.GetBytes(msg);
            client.Send(bSendMsg, bSendMsg.Length, 0);
        }

        /*
         * Builds the http response header
         * 
         * @params:
         *      - statusCode: you should know what that means
         *      - contentType: --""--
         *      - contentLength: --""--
         *      
         * @returns:
         *      - http response header in string format
         */
        public string BuildHeader(int statusCode, string contentType, int contentLength) {
            string header = "";

            header = header + "HTTP/1.1 " + statusCode + "\r\n";
            header = header + "Server: 127.0.0.1\r\n";
            header = header + "Content-ElementType: " + contentType + "\r\n";
            header = header + "Accept-Ranges: bytes\r\n";
            header = header + "Content-Length: " + contentLength + "\r\n\r\n";

            return header;
        }
    }
}