using Common.Model;

using Microsoft.VisualBasic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Action
{

    public class ActionConverter : JsonConverter
    {

        public override bool CanConvert(Type objectType) => true;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            string name = jsonObject["Action"].ToString();
            Type type = ActionHandler.ParamsTypes[name];
            var tt = Activator.CreateInstance(type);
            serializer.Populate(jsonObject["Params"].CreateReader(), tt);
            return new TextDesc { Action = name, Params = (IParams)tt };
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
