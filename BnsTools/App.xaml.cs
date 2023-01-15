using Autofac;
using Autofac.Annotation;

using HandyControl.Themes;

using System.Configuration;
using System.Windows;
using System.Windows.Media;

namespace BnsTools
{
    public partial class App : Application
    {
        private IContainer Container { get; set; }
        public Configuration Configuration { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ThemeManager.Current.ApplicationTheme = ApplicationTheme.Dark;
            var boot = new Bootstrapper();
            boot.Run();
                        
            //autofac
            var builder = new ContainerBuilder();

            builder.RegisterModule(new AutofacAnnotationModule());

            Container = builder.Build();
        }

        internal void UpdateTheme(ApplicationTheme theme)
        {
            if (ThemeManager.Current.ApplicationTheme != theme)
            {
                ThemeManager.Current.ApplicationTheme = theme;
            }
        }

        internal void UpdateAccent(Brush accent)
        {
            if (ThemeManager.Current.AccentColor != accent)
            {
                ThemeManager.Current.AccentColor = accent;
            }
        }
    }
}
