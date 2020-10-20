using System;
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
        public Listener() {
            Running = true;
        }

        public Listener(string addr, int port) {
            Running = true;
            Addr = IPAddress.Parse(addr);
            Port = port;
        }
        public int InitListener() {
            int err = 0;

            try {
                Server = new TcpListener(Addr, Port);
                Server.Start();

                while (Running) {
                    Console.WriteLine("Waiting...");
                    Socket client = Server.AcceptSocket();
                    Console.WriteLine("Connected!");

                    if (client.Connected) {
                        string rMsg = ReceiveMsg(client);
                        Console.WriteLine(rMsg);
                        FilterMsg(rMsg, client);
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

        public string ReceiveMsg(Socket client) {
            string msg;

            byte[] bMsg = new byte[1024];
            int i = client.Receive(bMsg, bMsg.Length, 0);
            msg = Encoding.ASCII.GetString(bMsg);

            return msg;
        }

        private void FilterMsg(string rMsg, Socket client) {
            string[] msgArray = rMsg.Split(new char[0]);

            switch (msgArray[0]) {
                case "GET":
                    SendMsg(200, "text/plain", msgArray[1], client);
                    break;
                case "POST":
                    SendMsg(501, "text/plain", "Not yet Implemented", client);
                    break;
                case "UPDATE":
                    SendMsg(501, "text/plain", "Not yet Implemented", client);
                    break;
                case "DELETE":
                    SendMsg(501, "text/plain", "Not yet Implemented", client);
                    break;
            }

        }

        public void SendMsg(int statusCode, string contentType, string msg, Socket client) {;
            byte[] bSendHead = Encoding.ASCII.GetBytes(BuildHeader(statusCode, contentType, msg.Length));
            byte[] bSendMsg = Encoding.ASCII.GetBytes(msg);
            client.Send(bSendHead, bSendHead.Length, 0);
            client.Send(bSendMsg, bSendMsg.Length, 0);
        }

        public string BuildHeader(int statusCode, string contentType, int contentLength) {
            string header = "";

            header = header + "HTTP/1.1" + statusCode + "\r\n";
            header = header + "Server: cx1193719-b\r\n";
            header = header + "Content-Type: " + contentType + "\r\n";
            header = header + "Accept-Ranges: bytes\r\n";
            header = header + "Content-Length: " + contentLength + "\r\n\r\n";

            return header;
        }
    }
}