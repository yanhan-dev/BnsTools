using BatchBoostJob.Views;

using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace BatchBoostJob
{
    public class BatchBoostJobModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<BatchBoostJobView>();
        }
    }
}