using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_Server {
    class MessageHandler {
        private List<Message> MessageList;

        public MessageHandler() {
            MessageList = new List<Message>();
        }

        public int AddMessage(string msg) {
            MessageList.Add(new Message(msg));
            return 0;
        }

        public string ReadMessage(int id) {
            return MessageList[id].MsgText;
        }

        public string ReadAllMessages() {
            string msg = "";

            foreach (Message element in MessageList)
                msg += element.MsgText + "\n";

            return msg;
        }

        public int UpdateMessage(int id, string msg) {
            try {
                MessageList[id].MsgText = msg;
            } catch (Exception) {
                return -1;
            }
            return 0;
        }

        public int DeleteMessage(int id) {
            MessageList.RemoveAt(id);
            return 0;
        }
    }
}
