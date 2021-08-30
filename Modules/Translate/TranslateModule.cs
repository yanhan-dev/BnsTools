using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

using Translate.Views;

namespace Translate
{
    public class TranslateModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<TranslateView>();
        }
    }
}