using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_Server {
    class RequestHandler {
        private Listener TcpL { get; set; }
        private MessageHandler MH;
        private RequestContext RC;

        public RequestHandler(Listener TcpL, MessageHandler MH, RequestContext RC) {
            this.TcpL = TcpL;
            this.MH = MH;
            this.RC = RC;
        }

        public void DoRequest() {
            switch (RC.Verb) {
                case "GET":
                    Get(RC.Resource);
                    break;
                case "POST":
                    Post(RC.Body);
                    break;
                case "PUT":
                    Update(RC.Resource, RC.Body);
                    break;
                case "DELETE":
                    Delete(RC.Resource);
                    break;
            }
        }

        private void Get(string resource) {
            if (resource.Equals("/messages")) {
                string mList = MH.ReadAllMessages();
                if(!mList.Equals(""))
                    TcpL.SendResponse(200, "text/plain", mList);
                else
                    TcpL.SendResponse(200, "text/plain", "No Messages Yet!");
            } else if (resource.Contains("/message/")) {
                string[] splited = resource.Split('/');
                try {
                    TcpL.SendResponse(200, "text/plain", MH.ReadMessage(int.Parse(splited[2])));
                } catch {
                    TcpL.SendResponse(404, "text/plain", "Message Not Found!");
                }
            } else
                TcpL.SendResponse(400, "text/plain", "Bad Request!");
        }

        private void Post(string msg) {
            try {
                if (RC.values["Content-Type"].Equals("text/plain;"))
                    if (MH.AddMessage(msg) == 0)
                        TcpL.SendResponse(200, "text/plain", "Message sent!");
                    else
                        TcpL.SendResponse(500, "text/plain", "Something went wrong!");
                else
                    TcpL.SendResponse(400, "text/plain", "Bad Content Type!");

            } catch {
                TcpL.SendResponse(400, "text/plain", "Bad Request!");
            }
        }

        private void Update(string resource, string msg) {
            if (resource.Contains("/message/")) {
                string[] splited = resource.Split('/');
                try {
                    if (RC.values["Content-Type"].Equals("text/plain;"))
                        if (MH.UpdateMessage(int.Parse(splited[2]), msg) == 0)
                            TcpL.SendResponse(200, "text/plain", "Updated Message!");
                        else
                            TcpL.SendResponse(404, "text/plain", "Message Does Not Exist!");
                    else
                        TcpL.SendResponse(400, "text/plain", "Bad Content Type!");

                } catch {
                    TcpL.SendResponse(404, "text/plain", "Invalid Message ID!");
                }
            } else
                TcpL.SendResponse(400, "text/plain", "Bad Request!");
        }

        private void Delete(string resource) {
            if (resource.Contains("/message/")) {
                string[] splited = resource.Split('/');
                try {
                    if (MH.DeleteMessage(int.Parse(splited[2])) == 0)
                        TcpL.SendResponse(200, "text/plain", "Deleted Message!");
                    else
                        TcpL.SendResponse(404, "text/plain", "Message Does Not Exist!");
                } catch {
                    TcpL.SendResponse(404, "text/plain", "Invalid Message ID!");
                }
            } else
                TcpL.SendResponse(400, "text/plain", "Bad Request!");
        }
    }
}
