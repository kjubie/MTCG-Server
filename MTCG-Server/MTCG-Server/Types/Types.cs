using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_Server {
    class Types {
        public Dictionary<string, Type> types;

        public Types() {
            types = new Dictionary<string, Type>();
            InitTypes();
        }

        private void InitTypes() {
            types.Add("fire", new Type("fire", "grass", "water"));
            types.Add("water", new Type("water", "fire", "grass"));
            types.Add("grass", new Type("grass", "water", "fire"));
            types.Add("normal", new Type("normal", "-", "-"));
        }
    }
}
