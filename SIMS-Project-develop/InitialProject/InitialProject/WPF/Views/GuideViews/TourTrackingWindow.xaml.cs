using InitialProject.Domain.Models;
using InitialProject.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
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
    /// Interaction logic for TourTrackingWindow.xaml
    /// </summary>
    public partial class TourTrackingWindow : Window, INotifyPropertyChanged
    {
        
        private TourRepository _tourRepository;
        private LocationRepository _locationRepository;
        private CheckpointRepository _checkpointRepository;
        private TourReservationRepository _tourReservationRepository;
        private CheckpointArrivalRepository _checkpointArrivalRepository;
        private UserRepository _userRepository;
        private User _guide;

        private Tour _selectedTour;
        public Tour SelectedTour
        {
            get => _selectedTour;
            set
            {
                if (_selectedTour != value)
                {
                    _selectedTour = value;
                    OnPropertyChanged(nameof(SelectedTour));
                }
            }
        }

        private Checkpoint _selectedCheckpoint;
        public Checkpoint SelectedCheckpoint
        {
            get => _selectedCheckpoint;
            set
            {
                if (_selectedCheckpoint != value)
                {
                    _selectedCheckpoint = value;
                    OnPropertyChanged(nameof(SelectedCheckpoint));
                }
            }
        }

        private Tour _activeTour;
        public Tour ActiveTour
        {
            get => _activeTour;
            set
            {
                if (_activeTour != value)
                {
                    _activeTour = value;
                    OnPropertyChanged(nameof(ActiveTour));
                }
            }
        }

        public ObservableCollection<Tour> TodaysTours { get; set; }
        public ObservableCollection<Checkpoint> Checkpoints { get; set; }
        

        public TourTrackingWindow(TourRepository tourRepository, LocationRepository locationRepository, CheckpointRepository checkpointRepository, TourReservationRepository tourReservationRepository, CheckpointArrivalRepository checkpointArrivalRepository, UserRepository userRepository, User guide)
        {
            InitializeComponent();
            this.DataContext = this;

            _tourRepository = tourRepository;
            _locationRepository = locationRepository;
            _checkpointRepository = checkpointRepository;
            _tourReservationRepository = tourReservationRepository;
            _checkpointArrivalRepository = checkpointArrivalRepository;
            _userRepository = userRepository;
            _guide = guide;

            TodaysTours = new ObservableCollection<Tour>();
            Checkpoints = new ObservableCollection<Checkpoint>();

            LoadTodaysTours();
            LoadCheckpoints();
        }

        private void LoadTodaysTours()
        {
            TodaysTours.Clear();
            var guidesToursToday = _tourRepository.GetAll().Where(t => t.GuideId == _guide.Id && t.StartTime.Date == DateTime.Now.Date);
            foreach (var tour in guidesToursToday)
            {
                tour.Location = _locationRepository.GetById(tour.LocationId);
                TodaysTours.Add(tour);
                if (tour.Status == TourStatus.ACTIVE)
                {
                    ActiveTour = tour;
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ButtonActivateTour_Click(object sender, RoutedEventArgs e)
        {
            if (ActiveTourExists()) return;
            var isPossibleToActivate = SelectedTour == null || SelectedTour.Status != TourStatus.NOT_STARTED;
            if (isPossibleToActivate) return;
            ActivateTour(SelectedTour);
            LoadCheckpoints();
        }

        private void LoadCheckpoints()
        {
            Checkpoints.Clear();
            if (ActiveTour == null) return;
            foreach (var checkpoint in _checkpointRepository.GetAll())
            {
                if (checkpoint.TourId == ActiveTour.Id)
                {
                    Checkpoints.Add(checkpoint);
                }
            }
        }

        private void ActivateTour(Tour tour)
        {
            tour.Status = TourStatus.ACTIVE;
            _tourRepository.Update(tour);
            ActiveTour = tour;
            UpdateTodaysTours();
        }

        private void UpdateTodaysTours()
        {
            var index = TodaysTours.IndexOf(ActiveTour);
            TodaysTours.Remove(ActiveTour);
            TodaysTours.Insert(index, ActiveTour);
        }

        private bool ActiveTourExists()
        {
            foreach (var tour in TodaysTours)
            {
                if (tour.Status == TourStatus.ACTIVE)
                    return true;
            }
            return false;
        }

        private void ButtonCompleteCheckpoint_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedCheckpoint == null) return;

            var isLastCheckpoint = Checkpoints.IndexOf(SelectedCheckpoint) + 1 == Checkpoints.Count;

            CompleteCheckpoint(SelectedCheckpoint);

            if (isLastCheckpoint)
            {
                FinishTour();
                LoadCheckpoints();
            }
        }

        private void FinishTour()
        {
            if (ActiveTour == null) return;
            ActiveTour.Status = TourStatus.FINISHED;
            UpdateTodaysTours();
            ActiveTour = null;
        }

        private void CompleteCheckpoint(Checkpoint checkpoint)
        {
            checkpoint.Active = true;
            _checkpointRepository.Update(checkpoint);
            UpdateCheckpoints();
        }

        private void UpdateCheckpoints()
        {
            if (SelectedCheckpoint == null) return;
            var index = Checkpoints.IndexOf(SelectedCheckpoint);
            var selectedCheckpoint = SelectedCheckpoint;
            Checkpoints.Remove(SelectedCheckpoint);
            Checkpoints.Insert(index, selectedCheckpoint);
        }

        private void ButtonEndTour_Click(object sender, RoutedEventArgs e)
        {
            FinishTour();
            LoadCheckpoints();
        }

        private void ButtonGuestList_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedCheckpoint == null) return;
            CheckpointArrivalWindow checkpointArrivalWindow = new CheckpointArrivalWindow(_tourReservationRepository, _checkpointArrivalRepository, _userRepository, SelectedCheckpoint, ActiveTour);
            checkpointArrivalWindow.Show();
        }
    }
}
