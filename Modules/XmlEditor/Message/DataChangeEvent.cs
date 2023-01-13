using Prism.Events;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlEditor.Message
{
    public class DataChangeEvent : PubSubEvent<string> { }
}
