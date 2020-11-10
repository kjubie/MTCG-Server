using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_Server {
    class RequestContext {
        private Listener TcpL { get; set; }

        public RequestContext(Listener TcpL) {
            this.TcpL = TcpL;
        }

        private void FilterMsg(string rMsg, Socket client) {
            string[] msgArray = rMsg.Split(new char[0]);

            switch (msgArray[0]) {
                case "GET":
                    Get(msgArray[1]);
                    break;
                case "POST":
                    Post();
                    break;
                case "UPDATE":
                    Update();
                    break;
                case "DELETE":
                    Delete();
                    break;
            }
        }

        private void Get(string resource) {
            TcpL.SendMsg(200, "text/plain", resource);
        }

        private void Post() {
            TcpL.SendMsg(501, "text/plain", "Not yet Implemented");
        }

        private void Update() {
            TcpL.SendMsg(501, "text/plain", "Not yet Implemented");
        }

        private void Delete() {
            TcpL.SendMsg(501, "text/plain", "Not yet Implemented");
        }
    }
}
