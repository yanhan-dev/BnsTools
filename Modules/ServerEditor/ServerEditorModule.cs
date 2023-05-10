using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

using ServerEditor.Views;

namespace ServerEditor
{
    public class ServerEditorModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ServerEditorView>();
        }
    }
}