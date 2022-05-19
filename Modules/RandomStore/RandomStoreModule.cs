using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

using RandomStore.Views;

namespace RandomStore
{
    public class RandomStoreModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<RandomStoreView>();
        }
    }
}