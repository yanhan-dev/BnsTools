using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

using XmlEditor.Views;

namespace XmlEditor
{
    public class XmlEditorModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<XmlEditorView>();
        }
    }
}