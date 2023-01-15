using Autofac.Annotation;

using Common.Model;

using Newtonsoft.Json.Linq;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Action
{
    [Component(AutofacScope = AutofacScope.SingleInstance, AutoActivate = true)]
    public class DeleteAction : IAction
    {
        public override string Name { get; set; } = "Delete";

        public DeleteAction()
        {
            ActionHandler.Reg<DeleteParams>(this);
        }


        public override string Do(string value, IParams param, bool elseMode = false)
        {
            DeleteParams deleteParam = param as DeleteParams;

            foreach (var s in deleteParam.Start)
            {
                if (!value.StartsWith(s, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                value = value.Substring(s.Length, value.Length - s.Length);
            }

            foreach (var s in deleteParam.End)
            {
                if (!value.EndsWith(s, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                value = value[..^s.Length];
            }

            return value;
        }
    }
}
