using Home.Views;

using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

using System.Reflection;

namespace BnsTools.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {

        private readonly IRegionManager _regionManager;

        public MainWindowViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            _title = $"BnsTools BETA {Assembly.GetEntryAssembly().GetName().Version}";
        }

        #region Dependency Property

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        #endregion


        #region Command

        private DelegateCommand<string> _navigateCommand;
        public DelegateCommand<string> NavigateCommand => _navigateCommand ??= new DelegateCommand<string>(Navigate);

        private void Navigate(string navigatePath)
        {
            if (navigatePath != null)
                _regionManager.RequestNavigate("ContentRegion", navigatePath);
        }

        private DelegateCommand _ContentRenderedCommand;
        public DelegateCommand ContentRenderedCommand =>
            _ContentRenderedCommand ?? (_ContentRenderedCommand = new DelegateCommand(ExecuteContentRenderedCommand));

        void ExecuteContentRenderedCommand()
        {
            NavigateCommand.Execute(nameof(HomeView));
        }
        #endregion
    }
}
