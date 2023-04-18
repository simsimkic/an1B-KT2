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
    /// Interaction logic for TourReservationForm.xaml
    /// </summary>
    public partial class SelectedTourOverview : Window, INotifyPropertyChanged
    {
        User LoggedUser { get; set; }
        public readonly TourRepository _tourRepository;
        public readonly LocationRepository _locationRepository;
        public readonly TourImageRepository _tourImageRepository;
        public readonly TourReservationRepository _tourReservationRepository;

        private ObservableCollection<Tour> _availableTours;
        public ObservableCollection<Tour> AvailableTours 
        { 
            get => _availableTours;
            set
            {
                if (value != _availableTours)
                {
                    _availableTours = value;
                    OnPropertyChanged(nameof(AvailableTours));
                }
            } 
        }

        private Tour _selectedTour;
        public Tour SelectedTour 
        { 
            get => _selectedTour;
            set
            {
                if (_selectedTour != value)
                {
                    _selectedTour = value;
                    OnPropertyChanged("SelectedTour");
                }
            } 
        }

        private int? _numberOfNewGuests;
        public int? NumberOfNewGuests 
        { 
            get => _numberOfNewGuests;
            set
            {
                if (_numberOfNewGuests != value)
                {
                    _numberOfNewGuests = value;
                    OnPropertyChanged("NumberOfNewGuests");
                }
            } 
        }

        private int? _availableSlots;
        public int? AvailableSlots
        {
            get => _availableSlots;
            set
            {
                if (_availableSlots != value)
                {
                    _availableSlots = value;
                    OnPropertyChanged(nameof(AvailableSlots));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SelectedTourOverview(TourRepository tourRepository, LocationRepository locationRepository, TourImageRepository tourImageRepository, Tour selectedTour, TourReservationRepository tourReservationRepository, User loggedUser)
        {
            InitializeComponent();
            DataContext = this;
            _tourRepository = tourRepository;
            _locationRepository = locationRepository;
            _tourImageRepository = tourImageRepository;
            _tourReservationRepository = tourReservationRepository;
            SelectedTour = selectedTour;
            LoggedUser=loggedUser;
            AvailableTours = new ObservableCollection<Tour>();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Guest2TourOverview guest2TourOverview = new Guest2TourOverview(_tourRepository, _locationRepository, _tourImageRepository, _tourReservationRepository, LoggedUser);
            guest2TourOverview.Show();
            
            Close();
        }

        private void Reserve_Click(object sender, RoutedEventArgs e)
        {
            if(SelectedTour.MaxGuests - NumberOfNewGuests < 0)
            {
                MessageBox.Show("There is no enough slots for this reservation!");
                OfferOtherTours();
            }
            else if(NumberOfNewGuests == 0 || NumberOfNewGuests == null)
            {
                MessageBox.Show("This field can't be empty!");
            }
            else 
            { 
                MakeNewReservation();
                Close();
            }
        }

        private void MakeNewReservation()
        {
            TourReservation tourReservation = new TourReservation();
            _tourReservationRepository.Save(SelectedTour.Id, LoggedUser.Id, NumberOfNewGuests, 0); //to-do: add option to enter age
            SelectedTour.MaxGuests = SelectedTour.MaxGuests - (int)NumberOfNewGuests;
            _tourRepository.Update(SelectedTour);
            
            MessageBox.Show("Your reservation was successful");
            
            Guest2TourOverview guest2TourOverview = new Guest2TourOverview(_tourRepository, _locationRepository, _tourImageRepository, _tourReservationRepository, LoggedUser);
            guest2TourOverview.Show();
        }
        public void OfferOtherTours()
        {
            AlternativeTourOffers alternativeTourOffers = new AlternativeTourOffers(_tourRepository, _locationRepository, _tourImageRepository, _tourReservationRepository, LoggedUser, SelectedTour);
            alternativeTourOffers.Show();
            Close();
        }
    }
}
