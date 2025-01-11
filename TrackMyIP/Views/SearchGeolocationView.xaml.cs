using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using TrackMyIP.ViewModels;

namespace TrackMyIP.Views
{
    /// <summary>
    /// Interaction logic for SearchGeolocationView.xaml
    /// </summary>
    public partial class SearchGeolocationView : MetroWindow
    {
        public SearchGeolocationView()
        {
            InitializeComponent();
            ((SearchGeolocationViewModel)DataContext).DialogCoordinator = DialogCoordinator.Instance;
        }
    }
}