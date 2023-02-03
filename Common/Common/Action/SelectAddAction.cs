using Autofac.Annotation;

using Common.Model;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Action
{
    [Component(AutofacScope = AutofacScope.SingleInstance, AutoActivate = true)]
    public class SelectAddAction : IAction
    {
        public override string Name { get => "SelectAdd"; }

        public SelectAddAction()
        {
            ActionHandler.Reg<SelectAddParams>(this);
        }

        public override string Do(string value, IParams param, bool elseMode = false)
        {
            var selectAddParam = param as SelectAddParams;

            foreach (var selector in selectAddParam.Selector)
            {
                if (null != selector.AndContains.FirstOrDefault(s => !value.Contains(s)))
                {
                    continue;
                }
                return $"{selector.Start}{value}{selector.End}";
            }
            return value;
        }
    }
}
