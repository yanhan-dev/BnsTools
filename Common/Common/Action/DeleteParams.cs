using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Action
{
    public class DeleteParams : IParams
    {
        public string[] Start { get; set; }
        public string[] End { get; set; }
    }
}
