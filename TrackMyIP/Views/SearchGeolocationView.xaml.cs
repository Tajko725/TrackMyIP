using MahApps.Metro.Controls;
using TrackMyIP.ViewModels;

namespace TrackMyIP.Views
{
    /// <summary>
    /// Interaction logic for SearchGeolocationView.xaml
    /// </summary>
    public partial class SearchGeolocationView : MetroWindow
    {
        public SearchGeolocationView( SearchGeolocationViewModel searchGeolocationViewModel)
        {
            InitializeComponent();
            DataContext = searchGeolocationViewModel;
        }
    }
}