using Autofac.Annotation;

using Common.Model;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Common.Action
{
    public static class ActionHandler
    {

        public static Dictionary<string, IAction> Actions { get; set; } = new();
        public static Dictionary<string, Type> ParamsTypes { get; set; } = new();

        public static void Reg<T>(IAction action) where T : IParams
        {
            Actions.Add(action.Name, action);
            ParamsTypes.Add(action.Name, typeof(T));
        }

        public static string Do(string value, TextDesc descAction, bool elseMode = false)
        {
            return Actions.GetValueOrDefault(descAction.Action, null)?.Do(value, descAction.Params, elseMode);
        }
    }
}
