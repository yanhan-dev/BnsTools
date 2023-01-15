using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Action
{
    public class AddParams : IParams
    {
        public string Start { get; set; }
        public string End { get; set; }

        public string ElseStart { get; set; }
        public string ElseEnd { get; set;}
    }
}
