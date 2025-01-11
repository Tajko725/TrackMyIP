using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using TrackMyIP.ViewModels;

namespace TrackMyIP.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : MetroWindow
    {
        public MainView()
        {
            InitializeComponent();
            ((MainViewModel)DataContext).DialogCoordinator = DialogCoordinator.Instance;
            ((MainViewModel)DataContext).SettingsViewModel = new SettingsViewModel(DialogCoordinator.Instance);
        }
    }
}