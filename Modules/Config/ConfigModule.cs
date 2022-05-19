using Config.Views;

using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Config
{
    public class ConfigModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ConfigView>();
        }
    }
}