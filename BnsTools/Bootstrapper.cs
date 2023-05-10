using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;
using BnsTools.Views;
using RandomStore;
using Schema;
using ExtractItem;
using Home;
using Translate;
using Config;
using ServerEditor;
using ClientEditor;

namespace BnsTools
{
    public class Bootstrapper : PrismBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<HomeModule>();
            moduleCatalog.AddModule<ConfigModule>();
            moduleCatalog.AddModule<SchemaModule>();
            moduleCatalog.AddModule<TranslateModule>();
            moduleCatalog.AddModule<ExtractItemModule>();
            moduleCatalog.AddModule<RandomStoreModule>();
            moduleCatalog.AddModule<ServerEditorModule>();
            moduleCatalog.AddModule<ClientEditorModule>();
        }
    }
}
