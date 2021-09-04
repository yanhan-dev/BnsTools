using ExtractItem.Views;

using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace ExtractItem
{
    public class ExtractItemModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ExtractItemView>();
        }
    }
}