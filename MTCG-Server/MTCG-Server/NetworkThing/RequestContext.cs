using System.Collections.Generic;

namespace MTCG_Server {
    /*
     * Class that holds the http header and body content
     */
    public class RequestContext {
        public string Verb { get; set; }    //Http verb (GET, POST, PUT, or DELETE)
        public string Resource { get; set; }    //Which resource is requested 
        public string Version { get; set; } //Http version
        public string Body { get; set; } = "";  //Content of the http request body
        public IDictionary<string, string> values = new Dictionary<string, string>();   //Futher header values in key-value storage

        public RequestContext(string Verb, string Resource, string Version) {
            this.Verb = Verb;
            this.Resource = Resource;
            this.Version = Version;
        }
    }
}
