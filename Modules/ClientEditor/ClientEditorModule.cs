using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

using ClientEditor.Views;

namespace ClientEditor
{
    public class ClientEditorModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ClientEditorView>();
        }
    }
}