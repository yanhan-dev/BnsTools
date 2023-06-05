using BatchSetNpc.Views;

using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace BatchSetNpc
{
    public class BatchSetNpcModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<BatchSetNpcView>();
        }
    }
}