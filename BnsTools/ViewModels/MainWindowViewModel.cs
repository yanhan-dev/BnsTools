using Config.Views;

using Home.Views;

using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

using System;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;

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
        public DelegateCommand ContentRenderedCommand => _ContentRenderedCommand ??= new DelegateCommand(ExecuteContentRenderedCommand);

        void ExecuteContentRenderedCommand()
        {
            NavigateCommand.Execute(nameof(ConfigView));
        }

        private DelegateCommand<System.ComponentModel.CancelEventArgs> _ClosingCommand;
        public DelegateCommand<System.ComponentModel.CancelEventArgs> ClosingCommand => _ClosingCommand ??= new DelegateCommand<System.ComponentModel.CancelEventArgs>(ExecuteClosingCommand);

        private void ExecuteClosingCommand(CancelEventArgs e)
        {
            //e.Cancel = true;
        }

        #endregion
    }
}
