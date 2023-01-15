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
    public class AddAction : IAction
    {
        public override string Name { get; set; } = "Add";

        public AddAction()
        {
            ActionHandler.Reg<AddParams>(this);
        }

        public override string Do(string value, IParams param, bool elseMode = false)
        {
            var addParam = param as AddParams;
            return elseMode ? $"{addParam.ElseStart}{value}{addParam.ElseEnd}" : $"{addParam.Start}{value}{addParam.End}";
        }
    }
}
