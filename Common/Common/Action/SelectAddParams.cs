using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Action
{
    public class SelectAddParams : IParams
    {
        public List<Selector> Selector { get; set; }
    }

    public class Selector
    {
        public List<string> AndContains { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
    }
}