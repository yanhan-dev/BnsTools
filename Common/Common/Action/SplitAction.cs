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
    public class SplitAction : IAction
    {
        public override string Name { get; set; } = "Split";

        public SplitAction()
        {
            ActionHandler.Reg<SplitParams>(this);
        }

        public override string Do(string value, IParams param, bool elseMode = false)
        {
            SplitParams splitParam = param as SplitParams;
            int start = int.Parse(splitParam.Start);
            int end = int.Parse(splitParam.End);

            if (string.IsNullOrEmpty(splitParam.Char) || start > end)
            {
                throw new ArgumentException("Action: Split, Params: Start > End");
            }
            string[] strings = value.Split(splitParam.Char);
            StringBuilder sb = new();
            for (int i = start; i < end; i++)
            {
                sb.Append(strings[i]);
            }
            return sb.ToString();
        }
    }
}
