using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Action
{
    public class ReplaceParams : IParams
    {
        public Dictionary<string,string> Maps { get; set; }
    }
}
