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
    /// Interaction logic for RatingGuestReminderForm.xaml
    /// </summary>
    public partial class RatingGuestReminderForm : Window
    {
        public int _ownerId;
        public readonly AccommodationReservationRepository _reservationRepository;
        public readonly AccommodationRepository _accommodationRepository;
        public readonly UserRepository _userRepository;
        public readonly RatingRepository _ratingRepository;

        public RatingGuestReminderForm(int ownerId, AccommodationReservationRepository reservationRepository, AccommodationRepository accommodationRepository, UserRepository userRepository, RatingRepository ratingRepository)
        {
            InitializeComponent();
            DataContext = this;
            _ownerId = ownerId;
            _reservationRepository = reservationRepository;
            _accommodationRepository = accommodationRepository;
            _userRepository = userRepository;
            _ratingRepository = ratingRepository;
        }

        private void ButtonRemindMeLater_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonRateNow_Click(object sender, RoutedEventArgs e)
        {
            GuestsOverview guestsOverview = new GuestsOverview(_ownerId, _reservationRepository, _accommodationRepository, _userRepository, _ratingRepository);
            guestsOverview.Show();
            this.Close();
        }
    }
}
