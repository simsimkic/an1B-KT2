using InitialProject.Repositories;
using InitialProject.Domain.Models;
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

namespace InitialProject.WPF.Views.Guest1Views
{
    /// <summary>
    /// Interaction logic for Guest1Menu.xaml
    /// </summary>
    public partial class Guest1Menu : Window
    {
        private AccommodationRepository _accommodationRepository;
        private AccommodationImageRepository _accommodationImageRepository;
        private LocationRepository _locationRepository;
        private AccommodationRatingRepository _accommodationRatingRepository;
        private AccommodationRatingImageRepository _accommodationRatingImageRepository;
        private AccommodationReservationRepository _accommodationReservationRepository;
        private UserRepository _userRepository;
        private User _user;
        public Guest1Menu(AccommodationRepository accommodationRepository, AccommodationImageRepository accommodationImageRepository, LocationRepository locationRepository, AccommodationRatingRepository accommodationRatingRepository, AccommodationRatingImageRepository accommodationRatingImageRepository, AccommodationReservationRepository accommodationReservationRepository, UserRepository userRepository, User user)
        {
            InitializeComponent();
            this.DataContext = this;

            _accommodationRepository = accommodationRepository;
            _accommodationImageRepository = accommodationImageRepository;
            _locationRepository = locationRepository;
            _accommodationRatingRepository = accommodationRatingRepository;
            _accommodationRatingImageRepository= accommodationRatingImageRepository;
            _accommodationReservationRepository= accommodationReservationRepository;
            _userRepository = userRepository;
            _user = user;
        }

        private void ButtonAccommodations_Click(object sender, RoutedEventArgs e)
        {
            Guest1AccommodationOverview guest1AccommodationOverview = new Guest1AccommodationOverview(_user, _accommodationRepository, _locationRepository, _accommodationImageRepository, _accommodationReservationRepository, _userRepository);
            guest1AccommodationOverview.Show();
        }

        private void ButtonReservations_Click(object sender, RoutedEventArgs e)
        {
            ReservationsView reservationsView = new ReservationsView(_user.Id);
            reservationsView.Show();
        }

        private void ButtonReviews_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonNotifications_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonSignOut_Click(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            this.Close();
        }
    }
}
