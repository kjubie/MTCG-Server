using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_Server {
    /* 
     * Class for handling messages (Add, Update, Read and Delete).
     */
    class MessageHandler {
        private List<Message> MessageList; //List that holds every message

        public MessageHandler() {
            MessageList = new List<Message>();
        }

        /*
         * Adds a message to the list.
         * 
         * @params:
         *      - msg: Message to add to the list
         *      
         * @return:
         *      - 0: On success
         */
        public int AddMessage(string msg) {
            MessageList.Add(new Message(msg));
            return 0;
        }

        /*
         * Reads a message from the list.
         * 
         * @params:
         *      - id: Message id to read from the list
         *      
         * @return:
         *      - Message at id
         */
        public string ReadMessage(int id) {
            return MessageList[id].MsgText;
        }

        /*
         * Reads all messages from the list.
         * 
         * @return:
         *      - All messages in string fromat
         */
        public string ReadAllMessages() {
            string msg = "";

            foreach (Message element in MessageList)
                msg += element.MsgText + "\n";

            return msg;
        }

        /*
         * Updates a message from the list.
         * 
         * @params:
         *      - id: Message to update
         *      - msg: New message text
         *      
         * @return:
         *      - 0: On success
         *      - -1: On failure
         */
        public int UpdateMessage(int id, string msg) {
            try {
                MessageList[id].MsgText = msg;
            } catch (Exception) {
                return -1;
            }
            return 0;
        }

        /*
         * Removes a message from the list.
         * 
         * @params:
         *      - id: Message to remove from the list
         *      
         * @return:
         *      - 0: On success
         */
        public int DeleteMessage(int id) {
            MessageList.RemoveAt(id);
            return 0;
        }
    }
}
