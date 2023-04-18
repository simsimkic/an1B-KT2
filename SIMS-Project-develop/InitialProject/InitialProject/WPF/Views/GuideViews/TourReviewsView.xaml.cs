using InitialProject.WPF.ViewModels;
using System.Windows;

namespace InitialProject.WPF.Views
{
    /// <summary>
    /// Interaction logic for TourReviewsView.xaml
    /// </summary>
    public partial class TourReviewsView : Window
    {
        public TourReviewsView()
        {
            InitializeComponent();
            this.DataContext = new TourReviewsViewModel(this);
        }
    }
}
