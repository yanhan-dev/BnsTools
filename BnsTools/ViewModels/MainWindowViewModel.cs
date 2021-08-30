using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace BnsTools.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {

        private readonly IRegionManager _regionManager;

        public MainWindowViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        #region Dependency Property

        private string _title = "BnsTools BETA 0.1";
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
        #endregion
    }
}
