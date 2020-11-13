using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_Server {
    class Message {
        public String MsgText { get; set; }

        public Message(String msgText) {
            this.MsgText = msgText;
        }
    }
}
