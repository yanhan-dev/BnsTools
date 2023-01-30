using Autofac.Annotation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Action
{
    [Component(AutofacScope = AutofacScope.SingleInstance, AutoActivate = true)]
    public class DirectAction : IAction
    {
        public DirectAction()
        {
            ActionHandler.Reg<DirectParams>(this);
        }
        public override string Name { get => "Direct"; }

        public override string Do(string value, IParams param, bool elseMode = false)
        {
            return value;
        }
    }
}
