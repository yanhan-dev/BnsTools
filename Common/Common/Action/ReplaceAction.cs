using Autofac.Annotation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Action
{
    [Component(AutofacScope = AutofacScope.SingleInstance, AutoActivate = true)]
    public class ReplaceAction : IAction
    {
        public ReplaceAction()
        {
            ActionHandler.Reg<ReplaceParams>(this);
        }
        public override string Name { get => "Replace"; }

        public override string Do(string value, IParams param, bool elseMode = false)
        {
            ReplaceParams replaceParams = param as ReplaceParams;
            foreach (var item in replaceParams.Maps)
            {
                if (!value.StartsWith(item.Key))
                {
                    continue;
                }

                return value.Replace(item.Key, item.Value);
            }

            return value;
        }
    }
}
