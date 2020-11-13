using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MTCG_Server {
    class Listener {
        private TcpListener Server { get; set; } = null;
        private int Port { get; set; } = 25575;
        private IPAddress Addr { get; set; } = IPAddress.Parse("127.0.0.1");
        private bool Running { get; set; } = false;
        //private List<RequestContext> Rc = new List<RequestContext>();
        private RequestHandler RH;
        private RequestContext RC;
        private Socket client;
        private MessageHandler MH;
        public Listener(MessageHandler MH) {
            Running = true;
            this.MH = MH; 
        }

        public Listener(string addr, int port, MessageHandler MH) {
            Running = true;
            Addr = IPAddress.Parse(addr);
            Port = port;
            this.MH = MH;
        }

        public int InitListener() {
            int err = 0;

            try {
                Server = new TcpListener(Addr, Port);
                Server.Start();

                while (Running) {
                    Console.WriteLine("Waiting...");
                    client = Server.AcceptSocket();
                    Console.WriteLine("Connected!");

                    if (client.Connected) {
                        BuildContext(ReceiveRequest());
                        
                        RH = new RequestHandler(this, MH, RC);
                        RH.DoRequest();
                    }

                    client.Close();
                }

            } catch (SocketException e) {
                Console.WriteLine(e);
            } finally {
                Server.Stop();
            }

            return err;
        }

        private void BuildContext(string request) {
            string[] reqArray = request.Split(new char[0]);
            int i = 0, countSpaces = 0;

            RC = new RequestContext(reqArray[0], reqArray[1], reqArray[2]);

            foreach (string element in reqArray) {
                if (element.EndsWith(":") && countSpaces < 3)
                    RC.values.Add(element.Remove(element.Length - 1), reqArray[i + 1]);
                else if (element.Equals(""))
                    ++countSpaces;
                else if (!element.Equals("") && countSpaces < 3)
                    countSpaces = 0;
                else if (countSpaces >= 3 && !element.Equals("")) {
                    RC.Body += element + " ";
                }
                
                ++i;
            }

            try {
                RC.Body = RC.Body.Remove(int.Parse(RC.values["Content-Length"]), RC.Body.Length - int.Parse(RC.values["Content-Length"]));
            } catch (KeyNotFoundException e) {
                RC.Body = "";
            }

        }

        public string ReceiveRequest() {
            string request;

            byte[] bRequest = new byte[1024];
            int i = client.Receive(bRequest, bRequest.Length, 0);
            request = Encoding.ASCII.GetString(bRequest);

            Console.Write(request + "\n");

            return request;
        }

        public void SendResponse(int statusCode, string contentType, string msg) {;
            byte[] bSendHead = Encoding.ASCII.GetBytes(BuildHeader(statusCode, contentType, msg.Length));
            byte[] bSendMsg = Encoding.ASCII.GetBytes(msg);
            client.Send(bSendHead, bSendHead.Length, 0);
            client.Send(bSendMsg, bSendMsg.Length, 0);
        }

        public string BuildHeader(int statusCode, string contentType, int contentLength) {
            string header = "";

            header = header + "HTTP/1.1 " + statusCode + "\r\n";
            header = header + "Server: 127.0.0.1\r\n";
            header = header + "Content-Type: " + contentType + "\r\n";
            header = header + "Accept-Ranges: bytes\r\n";
            header = header + "Content-Length: " + contentLength + "\r\n\r\n";

            return header;
        }
    }
}