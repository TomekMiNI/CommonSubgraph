using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAIO
{
    class Edge
    {
        public Edge(int v1, int v2)
        {
            this.v1 = v1;
            this.v2 = v2;
        }
        public int v1, v2;
        public override string ToString()
        {
            return String.Format("<{0}, {1}>", v1, v2);
        }
        public override bool Equals(object obj)
        {
            return ((obj as Edge).v1 == v1 && (obj as Edge).v2 == v2) || ((obj as Edge).v1 == v2 && (obj as Edge).v2 == v1);
        }
    }
}
