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
        
        /*
         * Decides with function to execute
         */
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

        /*
         * Reads a given resource and sends it to the client
         * 
         * @params:
         *      - resource: Requested resource
         */
        private void Get(string resource) {
            if (resource.Equals("/messages")) {     //Check if all messages are requested
                string mList = MH.ReadAllMessages();    //Save all messages into string
                if(!mList.Equals(""))   //Check if string is empty
                    TcpL.SendResponse(200, "text/plain", mList);    //If not empty, send list to client
                else
                    TcpL.SendResponse(200, "text/plain", "No Messages Yet!");   //If empty send client that no messages exist 
            } else if (resource.Contains("/message/")) {    //Check if specific message is requested
                string[] splited = resource.Split('/');     //Split the resource at '/' 
                try {
                    TcpL.SendResponse(200, "text/plain", MH.ReadMessage(int.Parse(splited[2])));    //Send requested message to client
                } catch {
                    TcpL.SendResponse(404, "text/plain", "Message Not Found!");     //Send error to client if message not found
                }
            } else
                TcpL.SendResponse(400, "text/plain", "Bad Request!");   //Send error to client if request was bad
        }

        /*
         * Adds a new message
         * 
         * @params:
         *      - msg: Message to add
         */
        private void Post(string msg) {
            try {
                if (RC.values["Content-Type"].Equals("text/plain;") || RC.values["Content-Type"].Equals("text/plain"))    //Check if content type is 'text/plain'
                    if (MH.AddMessage(msg) == 0)
                        TcpL.SendResponse(200, "text/plain", "Message sent!");  //Return success to client
                    else
                        TcpL.SendResponse(500, "text/plain", "Something went wrong!");
                else
                    TcpL.SendResponse(400, "text/plain", "Bad Content Type!");  //Return error when content type is not 'text/plain'

            } catch {
                TcpL.SendResponse(400, "text/plain", "Bad Request!"); //Return error on bad request
            }
        }

        /*
         * Updates a message
         * 
         * @params:
         *      - resource: Message to update
         *      - msg: New message text
         */
        private void Update(string resource, string msg) {
            if (resource.Contains("/message/")) {   //Check if resource is correct (I just noticed: '127.0.0.1/wdsahfhh/message/1' would also work but then the code below would return an error so it doesnt)
                string[] splited = resource.Split('/'); //Split the resource at '/' 
                try {
                    if (RC.values["Content-Type"].Equals("text/plain;") || RC.values["Content-Type"].Equals("text/plain"))    //Check if content type is 'text/plain'
                        if (MH.UpdateMessage(int.Parse(splited[2]), msg) == 0)  //Update message
                            TcpL.SendResponse(200, "text/plain", "Updated Message!");   //Send success to client
                        else
                            TcpL.SendResponse(404, "text/plain", "Message Does Not Exist!");    //Send error to client if message does not exist
                    else
                        TcpL.SendResponse(400, "text/plain", "Bad Content Type!");  //Send error to client if content type is bad

                } catch {
                    TcpL.SendResponse(404, "text/plain", "Invalid Message ID!");    //Send error to client if message id is invalid
                }
            } else
                TcpL.SendResponse(400, "text/plain", "Bad Request!");   //Send error to client if request was bad
        }

        /*
         * Deletes a message
         * 
         * @params:
         *      - resource: Message to delete
         */
        private void Delete(string resource) {  //Look at the funtions above, at this point you should understand what this does
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
