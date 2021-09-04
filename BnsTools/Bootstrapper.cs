using BnsTools.Views;

using ExtractItem;

using Home;

using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;

using Schema;

using System.Windows;

using Translate;

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
            moduleCatalog.AddModule<SchemaModule>();
            moduleCatalog.AddModule<TranslateModule>();
            moduleCatalog.AddModule<ExtractItemModule>();

        }
    }
}
