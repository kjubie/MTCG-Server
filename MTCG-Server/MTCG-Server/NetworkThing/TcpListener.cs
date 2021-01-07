using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MTCG_Server {
    public class Listener {
        private TcpListener Server { get; set; } = null;
        private int Port { get; set; } = 25575;
        private IPAddress Addr { get; set; } = IPAddress.Parse("127.0.0.1");
        private bool Running { get; set; } = false;
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
                err = -1;
            }

            return err;
        }

        /*
         * Fills the request context object with the http request data
         * 
         * @params:
         *      - request: Raw http request data
         */
        private void BuildContext(string request) {
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
        public string ReceiveRequest() {
            string request;

            byte[] bRequest = new byte[1024];
            int i = client.Receive(bRequest, bRequest.Length, 0);
            request = Encoding.ASCII.GetString(bRequest);

            Console.Write(request + "\n");

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
        public void SendResponse(int statusCode, string contentType, string msg) {
            byte[] bSendHead = Encoding.ASCII.GetBytes(BuildHeader(statusCode, contentType, msg.Length));
            byte[] bSendMsg = Encoding.ASCII.GetBytes(msg);
            client.Send(bSendHead, bSendHead.Length, 0);
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
            header = header + "Content-Type: " + contentType + "\r\n";
            header = header + "Accept-Ranges: bytes\r\n";
            header = header + "Content-Length: " + contentLength + "\r\n\r\n";

            return header;
        }
    }
}