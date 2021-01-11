using System.Collections.Generic;

namespace MTCG_Server {
    public class Types {
        public Dictionary<string, ElementType> types;

        public Types() {
            types = new Dictionary<string, ElementType>();
            InitTypes();
        }

        private void InitTypes() {
            types.Add("fire", new ElementType("fire", "grass", "water"));
            types.Add("water", new ElementType("water", "fire", "grass"));
            types.Add("grass", new ElementType("grass", "water", "fire"));
            types.Add("normal", new ElementType("normal", "-", "-"));
        }
    }
}
