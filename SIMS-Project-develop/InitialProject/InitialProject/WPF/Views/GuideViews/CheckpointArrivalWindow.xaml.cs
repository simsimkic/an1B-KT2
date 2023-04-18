using InitialProject.Domain.Models;
using InitialProject.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for CheckpointsArrivalWindow.xaml
    /// </summary>
    public partial class CheckpointArrivalWindow : Window, INotifyPropertyChanged
    {
        private TourReservationRepository _tourReservationRepository;
        private CheckpointArrivalRepository _checkpointArrivalRepository;
        private UserRepository _userRepository;
        private Checkpoint _currentCheckpoint;
        private Tour _currentTour;


        private User _selectedArrivedGuest;
        public User SelectedArrivedGuest
        {
            get => _selectedArrivedGuest;
            set
            {
                if (_selectedArrivedGuest != value)
                {
                    _selectedArrivedGuest = value;
                    OnPropertyChanged(nameof(SelectedArrivedGuest));
                }
            }
        }

        private User _selectedUnarrivedGuest;
        public User SelectedUnarrivedGuest
        {
            get => _selectedUnarrivedGuest;
            set
            {
                if (_selectedUnarrivedGuest != value)
                {
                    _selectedUnarrivedGuest = value;
                    OnPropertyChanged(nameof(SelectedUnarrivedGuest));
                }
            }
        }

        public ObservableCollection<User> ArrivedGuests { get; set; }
        public ObservableCollection<User> UnarrivedGuests { get; set; }

        public CheckpointArrivalWindow(TourReservationRepository tourReservationRepository, CheckpointArrivalRepository checkpointArrivalRepository, UserRepository userRepository, Checkpoint currentCheckpoint, Tour currentTour)
        {
            InitializeComponent();
            this.DataContext = this;

            _tourReservationRepository = tourReservationRepository;
            _checkpointArrivalRepository = checkpointArrivalRepository;
            _userRepository = userRepository;
            _currentCheckpoint = currentCheckpoint;
            _currentTour = currentTour;

            ArrivedGuests = new ObservableCollection<User>();
            UnarrivedGuests = new ObservableCollection<User>();
            LoadArrivedGuests();
            LoadUnarrivedGuests();
        }

        private void LoadUnarrivedGuests()
        {
            /*UnarrivedGuests.Clear();
            foreach (var tourReservation in _tourReservationRepository.GetAll())
            {
                var hasUserReservedTour = tourReservation.TourId == _currentTour.Id;
                var hasUserArrivedToCheckpoint = _checkpointArrivalRepository.GetByUserId(tourReservation.UserId) == null;
                if (hasUserReservedTour && hasUserArrivedToCheckpoint)
                {
                    UnarrivedGuests.Add(_userRepository.GetById(tourReservation.UserId));
                }
            }*/
        }

        private void LoadArrivedGuests()
        {
            /*ArrivedGuests.Clear();
            foreach (var tourReservation in _tourReservationRepository.GetAll())
            {
                foreach (var checkpointArrival in _checkpointArrivalRepository.GetAll())
                {
                    var hasUserWithReservationArrived = tourReservation.UserId == checkpointArrival.UserId && checkpointArrival.CheckpointId == _currentCheckpoint.Id;
                    if (!hasUserWithReservationArrived) continue;
                    ArrivedGuests.Add(_userRepository.GetById(tourReservation.UserId));
                }
            }*/
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ButtonRemoveGuest_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedArrivedGuest == null) return;

            UnarrivedGuests.Add(SelectedArrivedGuest);
            ArrivedGuests.Remove(SelectedArrivedGuest);
        }

        private void ButtonAddGuest_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedUnarrivedGuest == null) return;

            ArrivedGuests.Add(SelectedUnarrivedGuest);
            UnarrivedGuests.Remove(SelectedUnarrivedGuest);
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            /*List<CheckpointArrival> currentCheckpointsArrivals = _checkpointArrivalRepository.GetAllByCheckpointId(_currentCheckpoint.Id).ToList();
            foreach (var arrivedGuest in ArrivedGuests)
            {
                var arrivedGuestArrival = currentCheckpointsArrivals.Find(c => c.UserId == arrivedGuest.Id);
                if (arrivedGuestArrival != null)
                {
                    currentCheckpointsArrivals.Remove(arrivedGuestArrival);
                    continue;
                }

                _checkpointArrivalRepository.Create(_currentCheckpoint.Id, arrivedGuest.Id);
            }

            foreach (var checkpointArrival in currentCheckpointsArrivals)
            {
                _checkpointArrivalRepository.Delete(checkpointArrival);
            }
            this.Close();*/
        }
    }
}
