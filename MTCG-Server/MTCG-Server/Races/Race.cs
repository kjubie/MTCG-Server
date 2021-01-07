using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_Server {
    abstract class Race {
        public string race;
        public string text;
        public abstract void DoRaceEffect(ref Card opposingCard, out int calcDamage);
    }
}