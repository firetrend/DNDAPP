using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNDAPP.Entrails.Mechanics
{
    class DiceContext
    {
        public int Sides { get; set; }
        public int Quantity { get; set; }
        public int Modifier { get; set; }
        public List<int> Rolls { get; set; } = new List<int>();
        public int Sum { get; set; }
        public int Total { get; set; }
        public bool IsNatural20 { get; set; }
        public bool IsNatural1 { get; set; }
    }
}
