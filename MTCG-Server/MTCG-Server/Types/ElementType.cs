namespace MTCG_Server {
    public class ElementType {
        public string name { get; set; }
        public string strong { get; set; }
        public string weak { get; set; }

        public ElementType(string name, string strong, string weak) {
            this.name = name;
            this.strong = strong;
            this.weak = weak;
        }
    }
}
