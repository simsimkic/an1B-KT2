using InitialProject.Domain.Models;
using InitialProject.Repositories;
using InitialProject.WPF.ViewModels.Guest2ViewModels;
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

namespace InitialProject.WPF.Views.Guest2Views
{
    /// <summary>
    /// Interaction logic for Guest2Menu.xaml
    /// </summary>
    public partial class Guest2Menu : Window
    {
        public User LoggedUser { get; set; }
        public readonly TourRepository _tourRepository;
        public readonly LocationRepository _locationRepository;
        public readonly TourImageRepository _tourImageRepository;
        public readonly TourReservationRepository _tourReservationRepository;


        public Guest2Menu(TourRepository tourRepository, LocationRepository locationRepository, TourImageRepository tourImageRepository, TourReservationRepository tourReservationRepository, User user)
        {
            InitializeComponent();
            this.DataContext = new MenuViewModel(this, tourRepository, locationRepository, tourImageRepository, tourReservationRepository, user);
            _tourRepository = tourRepository;
            _locationRepository = locationRepository;
            _tourImageRepository = tourImageRepository;
            _tourReservationRepository = tourReservationRepository;
            LoggedUser = user;
        }

        private void OpenTours_Click(object sender, RoutedEventArgs e)
        {
            Guest2TourOverview guest2TourOverview = new Guest2TourOverview(_tourRepository, _locationRepository, _tourImageRepository, _tourReservationRepository, LoggedUser);
            guest2TourOverview.Show();
            Close();
        }

        private void OpenReservedTours_Click(object sender, RoutedEventArgs e)
        {
            ReservedToursView reservedToursView = new ReservedToursView(LoggedUser);
            reservedToursView.Show();
            Close();
        }
    }
}
