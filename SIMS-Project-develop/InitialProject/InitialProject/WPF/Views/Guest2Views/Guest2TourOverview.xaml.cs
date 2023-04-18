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
    /// Interaction logic for Guest2TourOverview.xaml
    /// </summary>
    public partial class Guest2TourOverview : Window, INotifyPropertyChanged
    {
        public User LoggedUser { get; set; }
        public readonly TourRepository _tourRepository;
        public readonly LocationRepository _locationRepository;
        public readonly TourImageRepository _tourImageRepository;
        public readonly TourReservationRepository _tourReservationRepository;

        private ObservableCollection<Tour> _tours;
        public ObservableCollection<Tour> Tours
        {
            get => _tours;
            set
            {
                if (_tours != value)
                {
                    _tours = value;
                    OnPropertyChanged("Tours");
                }
            }
        }

        private ObservableCollection<string> _countries;
        public ObservableCollection<string> Countries
        {
            get => _countries;
            set
            {
                if (_countries != value)
                {
                    _countries = value;
                    OnPropertyChanged("Countries");
                }
            }
        }

        private ObservableCollection<string> _cities;
        public ObservableCollection<string> Cities
        {
            get => _cities;
            set
            {
                if (value != _cities)
                {
                    _cities = value;
                    OnPropertyChanged("Cities");
                }
            }
        }

        private ObservableCollection<string> _languages;
        public ObservableCollection<string> Languages
        {
            get => _languages;
            set
            {
                if (value != _languages)
                {
                    _languages = value;
                    OnPropertyChanged("Languages");
                }
            }
        }

        private Tour _tour;
        public Tour SelectedTour
        {
            get => _tour;
            set
            {
                if (_tour != value)
                {
                    _tour = value;
                    OnPropertyChanged("SelectedTour");
                }
            }
        }

        private string _country;
        public string SelectedCountry
        {
            get => _country;
            set
            {
                if (_country != value)
                {
                    _country = value;
                    OnPropertyChanged("SelectedCountry");
                }
            }
        }

        private string _city;
        public string SelectedCity
        {
            get => _city;
            set
            {
                if (value != _city)
                {
                    _city = value;
                    OnPropertyChanged("SelectedCity");
                }
            }
        }

        private double? _tourDuration;
        public double? TourDuration
        {
            get => _tourDuration;
            set
            {
                if (_tourDuration != value)
                {
                    _tourDuration = value;
                    OnPropertyChanged("TourDuration");
                }
            }
        }

        private string _language;
        public string SelectedLanguage
        {
            get => _language;
            set
            {
                if (value != _language)
                {
                    _language = value;
                    OnPropertyChanged("SelectedLanguage");
                }
            }
        }

        private int? _maxGuests;
        public int? MaxGuests
        {
            get => _maxGuests;
            set
            {
                if (value != _maxGuests)
                {
                    _maxGuests = value;
                    OnPropertyChanged("MaxGuests");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Guest2TourOverview(TourRepository tourRepository, LocationRepository locationRepository, TourImageRepository tourImageRepository, TourReservationRepository tourReservationRepository, User user)
        {
            InitializeComponent();
            DataContext = this;
            _tourRepository = tourRepository;
            _locationRepository = locationRepository;
            _tourImageRepository = tourImageRepository;
            _tourReservationRepository = tourReservationRepository;
            LoggedUser = user;
            Countries = new ObservableCollection<string>();
            Cities = new ObservableCollection<string>();
            Tours = new ObservableCollection<Tour>();
            ShowInitialTourOptions();
        }

        private void ShowInitialTourOptions()
        {
            FillCountries();

            Languages = new ObservableCollection<string> { "Serbian", "Hungarian", "German", "Thai", "French", "Italian", "Turkish", "Chinese", "Bulgarian", "Swedish", "Finish", "Croatian", "Bosnian", "Japanese", "Eren Yeager", "Danish", "English", "Romanian", "Greek", "Albanian", "Ukranian", "Russian", "Slovenian", "Slovakian", "Belgian", "Dutch", "Portuguese", "Spanish", "Lithuanian", "Estonian" };

            foreach (var tour in _tourRepository.GetAll())
            {
                FillTours(tour);
            }
        }

        public void ShowAlternativeOptions(Tour tour)
        {
            if (IsFull(tour))
            {
                OfferOtherTours();
            }
        }

        public bool IsFull(Tour tour)
        {
            if (tour.MaxGuests == 0)
            {
                return true;
            }

            return false;
        }

        public void OfferOtherTours()
        {
            AlternativeTourOffers alternativeTourOffers = new AlternativeTourOffers(_tourRepository, _locationRepository, _tourImageRepository, _tourReservationRepository, LoggedUser, SelectedTour);
            alternativeTourOffers.Show();
            Close();
        }

        private void ComboBoxCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxCity.IsEnabled = true;
            Cities.Clear();
            FillCities();
            if (SelectedCountry == null)
            {
                ComboBoxCity.IsEnabled = false;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void FilterApply_Click(object sender, RoutedEventArgs e)
        {
            Tours = new ObservableCollection<Tour>();
            foreach (var tour in _tourRepository.GetAll())
            {
                FillTours(tour);

                RemoveTourByCountry(tour);
                RemoveTourByCity(tour);
                RemoveTourByDuration(tour);
                RemoveTourByMaxCapacity(tour);
                RemoveTourByLanguage(tour);
            }
        } 

        public void FillCountries()
        {
            foreach (var location in _locationRepository.GetAll())
            {
                if (!Countries.Contains(location.Country))
                {
                    Countries.Add(location.Country);
                }
            }
        }

        public void FillCities()
        {
            foreach (var location in _locationRepository.GetAll())
            {
                if (SelectedCountry == location.Country)
                {
                    Cities.Add(location.City);
                }
            }
        }
        public void FillTours(Tour tour)
        {
            if (tour.Status == TourStatus.ACTIVE || tour.Status == TourStatus.NOT_STARTED)
            {
                foreach (var location in _locationRepository.GetAll())
                {
                    if (location.Id == tour.LocationId)
                    {
                        tour.Location = _locationRepository.GetById(location.Id);
                    }
                }
                Tours.Add(tour);
            }
        }

        public void RemoveTourByDuration(Tour tour)
        {
            if (TourDuration != 0 && TourDuration.ToString() != "")
            {
                if (TourDuration != tour.Duration)
                {
                    Tours.Remove(tour);
                }
            }
        }

        public void RemoveTourByMaxCapacity(Tour tour)
        {
            if (MaxGuests != 0 && MaxGuests.ToString() != "")
            {
                if(MaxGuests > tour.MaxGuests) 
                {
                    if (MaxGuests != tour.MaxGuests)
                    {
                        Tours.Remove(tour);
                    }                    
                }
            }
        }

        public void RemoveTourByLanguage(Tour tour)
        {
            if (SelectedLanguage != null && SelectedLanguage != "")
            {
                if (SelectedLanguage != tour.Language)
                {
                    Tours.Remove(tour);
                }
            }
        }

        public void RemoveTourByCountry(Tour tour)
        {
            if (SelectedCountry != null && SelectedCountry != "")
            {
                if (SelectedCountry != tour.Location.Country)
                {
                    Tours.Remove(tour);
                }
            }
        }

        public void RemoveTourByCity(Tour tour)
        {
            if (SelectedCity != null && SelectedCity != "")
            {
                if (SelectedCity != tour.Location.City)
                {
                    Tours.Remove(tour);
                }
            }
        }

        private void ChooseTour_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTour == null)
            {
                return;
            }
            if(SelectedTour.MaxGuests == 0)
            {
                MessageBox.Show("Selected tour is full, try picking alternative tour on same location.");
                ShowAlternativeOptions(SelectedTour);
            }
            else
            {
                SelectedTourOverview selectedTourOverview = new SelectedTourOverview(_tourRepository, _locationRepository, _tourImageRepository, SelectedTour, _tourReservationRepository, LoggedUser);
                selectedTourOverview.Show();
            }
            Close();
        }
    }
}
