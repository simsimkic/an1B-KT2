using InitialProject.Application.UseCases;
using InitialProject.Domain.Models;
using InitialProject.Repositories;
using InitialProject.WPF.Views.Guest1Views;
using InitialProject.WPF.Views.Guest2Views;
using System.ComponentModel;
using System.Configuration;
using System.Runtime.CompilerServices;
using System.Windows;

namespace InitialProject.WPF.Views
{
    /// <summary>
    /// Interaction logic for SignInForm.xaml
    /// </summary>
    public partial class SignInForm : Window
    {

        private readonly UserRepository _userRepository;
        private readonly AccommodationRepository _accommodationRepository;
        private readonly LocationRepository _locationRepository;
        private readonly AccommodationImageRepository _accommodationImageRepository;
        private readonly TourRepository _tourRepository;
        private readonly TourImageRepository _tourImageRepository;
        private readonly CheckpointRepository _checkpointRepository;
        private readonly AccommodationReservationRepository _accommodationReservationRepository;
        private readonly RatingRepository _ratingRepository;
        private readonly TourReservationRepository _tourReservationRepository;
        private readonly CheckpointArrivalRepository _checkpointArrivalRepository;
        private readonly AccommodationRatingRepository _accommodationRatingRepository;
        private readonly AccommodationRatingImageRepository _accommodationRatingImageRepository;

        private readonly SetOwnerRoleService _setOwnerRoleService;

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                if (value != _username)
                {
                    _username = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SignInForm()
        {
            InitializeComponent();
            DataContext = this;
            _userRepository = new UserRepository();
            _accommodationRepository = new AccommodationRepository();
            _locationRepository = new LocationRepository(); 
            _accommodationImageRepository = new AccommodationImageRepository();
            _tourRepository = new TourRepository();
            _tourImageRepository = new TourImageRepository();
            _checkpointRepository = new CheckpointRepository();
            _accommodationReservationRepository = new AccommodationReservationRepository();
            _ratingRepository = new RatingRepository();
            _tourReservationRepository = new TourReservationRepository();
            _checkpointArrivalRepository = new CheckpointArrivalRepository();
            _accommodationRatingRepository = new AccommodationRatingRepository();
            _accommodationRatingImageRepository = new AccommodationRatingImageRepository();

            _setOwnerRoleService = new SetOwnerRoleService();

            SetOwnerRole();
        }

        private void SetOwnerRole()
        {
            foreach (var user in _userRepository.GetAll())
            {
                if (user.Role == UserRole.OWNER || user.Role == UserRole.SUPER_OWNER)
                {
                    _setOwnerRoleService.SetOwnerRole(user.Id);
                }
                
            }
        }

        private void SignIn(object sender, RoutedEventArgs e)
        {
            User user = _userRepository.GetByUsername(Username);

            if (user == null)
            {
                MessageBox.Show("Wrong username!");
                return;
            }

            if (!user.Password.Equals(txtPassword.Password))
            {
                MessageBox.Show("Wrong password!");
                return;
            }

            OpenSuitableWindow(user);
        }

        private void OpenSuitableWindow(User user)
        {
            if (user.Role == UserRole.OWNER || user.Role == UserRole.SUPER_OWNER)
            {
                OwnerForm ownerForm = new OwnerForm(_accommodationRepository, _locationRepository, _accommodationImageRepository, user, _accommodationReservationRepository, _userRepository, _ratingRepository);
                ownerForm.Show();
                Close();
            }
            else if (user.Role == UserRole.GUEST1)
            {
                Guest1Menu guest1Menu = new Guest1Menu(_accommodationRepository, _accommodationImageRepository, _locationRepository, _accommodationRatingRepository, _accommodationRatingImageRepository, _accommodationReservationRepository, _userRepository, user);
                guest1Menu.Show();
                Close();
            }
            else if (user.Role == UserRole.GUEST2)
            {
                /*Guest2TourOverview guest2TourOverview = new Guest2TourOverview(_tourRepository, _locationRepository, _tourImageRepository, _tourReservationRepository, user);
                guest2TourOverview.Show();*/
                Guest2Menu guest2Menu = new Guest2Menu(_tourRepository, _locationRepository, _tourImageRepository, _tourReservationRepository, user);
                guest2Menu.Show();
                Close();
            }
            else if (user.Role == UserRole.GUIDE)
            {
                GuideMenu guideMenu = new GuideMenu(_tourRepository, _tourImageRepository, _locationRepository, _checkpointRepository, _tourReservationRepository, _checkpointArrivalRepository, _userRepository, user);
                guideMenu.Show();
                Close();
            }
        }
    }
}
