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
    /// Interaction logic for AlternativeTourOffers.xaml
    /// </summary>
    public partial class AlternativeTourOffers : Window, INotifyPropertyChanged
    {
        User LoggedUser { get; set; }
        public readonly TourRepository _tourRepository;
        public readonly LocationRepository _locationRepository;
        public readonly TourImageRepository _tourImageRepository;
        public readonly TourReservationRepository _tourReservationRepository;

        private ObservableCollection<Tour> _alternativeTours;
        public ObservableCollection<Tour> AlternativeTours 
        { 
            get => _alternativeTours;
            set
            {
                if (value != _alternativeTours)
                {
                    _alternativeTours = value;
                    OnPropertyChanged(nameof(AlternativeTours));
                }
            } 
        }

        private Tour _alternativeSelectedTour;
        public Tour AlternativeSelectedTour 
        { 
            get => _alternativeSelectedTour;
            set
            {
                if (value != _alternativeSelectedTour)
                {
                    _alternativeSelectedTour = value;
                    OnPropertyChanged(nameof(AlternativeSelectedTour));
                }
            } 
        }

        public Tour PreviouslySelectedTour { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public AlternativeTourOffers(TourRepository tourRepository, LocationRepository locationRepository, TourImageRepository tourImageRepository, TourReservationRepository tourReservationRepository, User loggedUser, Tour previouslySelectedTour)
        {
            InitializeComponent();
            DataContext = this;
            _tourRepository = tourRepository;
            _locationRepository = locationRepository;
            _tourImageRepository = tourImageRepository;
            _tourReservationRepository = tourReservationRepository;
            LoggedUser = loggedUser;
            PreviouslySelectedTour = previouslySelectedTour;
            AlternativeTours = new ObservableCollection<Tour>();
            ShowAlternativeTourOptions();
        }

        public void ShowAlternativeTourOptions()
        {
            foreach (var tour in _tourRepository.GetAll())
            {
                FillAlternativeTours(tour);
            }
        }

        public void FillAlternativeTours(Tour tour)
        {
            foreach (var location in _locationRepository.GetAll())
            {
                if (location.Id == tour.LocationId)
                {
                    tour.Location = _locationRepository.GetById(location.Id);
                }
            }
            if(PreviouslySelectedTour.LocationId == tour.LocationId && PreviouslySelectedTour.Id != tour.Id)
            {
                if(tour.Status == TourStatus.ACTIVE || tour.Status == TourStatus.NOT_STARTED)
                {
                    AlternativeTours.Add(tour);
                }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Guest2TourOverview guest2TourOverview = new Guest2TourOverview(_tourRepository, _locationRepository, _tourImageRepository, _tourReservationRepository, LoggedUser);
            guest2TourOverview.Show();

            Close();
        }

        private void ChooseButton_Click(object sender, RoutedEventArgs e)
        {
            if (AlternativeSelectedTour == null)
            {
                return;
            }
            SelectedTourOverview selectedTourOverview = new SelectedTourOverview(_tourRepository, _locationRepository, _tourImageRepository, AlternativeSelectedTour, _tourReservationRepository, LoggedUser);
            selectedTourOverview.Show();
            Close();
        }
    }
}
