using InitialProject.Domain.Models;
using InitialProject.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InitialProject.WPF.Views
{
    /// <summary>
    /// Interaction logic for GuideMenu.xaml
    /// </summary>
    public partial class GuideMenu : Window
    {
        private TourRepository _tourRepository;
        private TourImageRepository _tourImageRepository;
        private LocationRepository _locationRepository;
        private CheckpointRepository _checkpointRepository;
        private TourReservationRepository _tourReservationRepository;
        private CheckpointArrivalRepository _checkpointArrivalRepository;
        private UserRepository _userRepository;
        private User _guide;

        public string _welcomeMessage;
        public string WelcomeMessage { get; set; }

        public GuideMenu(TourRepository tourRepository, TourImageRepository tourImageRepository, LocationRepository locationRepository, CheckpointRepository checkpointRepository, TourReservationRepository tourReservationRepository, CheckpointArrivalRepository checkpointArrivalRepository, UserRepository userRepository, User guide)
        {
            InitializeComponent();
            this.DataContext = this;

            _tourRepository = tourRepository;
            _tourImageRepository = tourImageRepository;
            _locationRepository = locationRepository;
            _checkpointRepository = checkpointRepository;
            _tourReservationRepository = tourReservationRepository;
            _checkpointArrivalRepository = checkpointArrivalRepository;
            _userRepository = userRepository;
            _guide = guide;
            WelcomeMessage = String.Format("Welcome {0}", _guide.Username);
        }

        private void ButtonCreateTour_Click(object sender, RoutedEventArgs e)
        {
            TourCreationForm tourCreationForm = new TourCreationForm(_tourRepository, _tourImageRepository, _locationRepository, _checkpointRepository, _guide);
            tourCreationForm.Show();
        }

        private void ButtonTrackTours_Click(object sender, RoutedEventArgs e)
        {
            TourTrackingWindow tourTrackingWindow = new TourTrackingWindow(_tourRepository, _locationRepository, _checkpointRepository, _tourReservationRepository, _checkpointArrivalRepository, _userRepository, _guide);
            tourTrackingWindow.Show();
        }

        private void ButtonLogOut_Click(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            this.Close();
        }

        private void ButtonTodaysTours_Click(object sender, RoutedEventArgs e)
        {
            YourToursView yourToursView = new YourToursView();
            yourToursView.Show();
            this.Close();
        }
    }
}
