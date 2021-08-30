using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

using Schema.Views;

namespace Schema
{
    public class SchemaModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<SchemaView>();
        }
    }
}