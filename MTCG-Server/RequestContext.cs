using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_Server {
    class RequestContext {
        public string Verb { get; set; }
        public string Resource { get; set; }
        public string Version { get; set; }
        public string Body { get; set; } = "";
        public IDictionary<string, string> values = new Dictionary<string, string>();

        public RequestContext(string Verb, string Resource, string Version) {
            this.Verb = Verb;
            this.Resource = Resource;
            this.Version = Version;
        }
    }
}
