using Common.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Action
{
    public abstract class IAction
    {
        public abstract string Name { get; }

        public abstract string Do(string value, IParams param, bool elseMode = false);
    }
}
