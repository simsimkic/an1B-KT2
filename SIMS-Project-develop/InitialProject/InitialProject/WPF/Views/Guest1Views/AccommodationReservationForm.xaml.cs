using InitialProject.Domain.Models;
using InitialProject.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for AccommodationReservationForm.xaml
    /// </summary>
    public partial class AccommodationReservationForm : Window, INotifyPropertyChanged
    {
        public readonly AccommodationRepository _accommodationRepository;
        public readonly AccommodationImageRepository _accommodationImageRepository;
        public readonly LocationRepository _locationRepository;
        public readonly AccommodationReservationRepository _accommodationReservationRepository;
        public User LoggedUser { get; set; }
        
        private Accommodation _accommodation;
        public Accommodation SelectedAccommodation
        {
            get => _accommodation;
            set
            {
                if (_accommodation != value)
                {
                    _accommodation = value;
                    OnPropertyChanged(nameof(SelectedAccommodation));
                }
            }
        }

        private DateTime _selectedStartDate;
        public DateTime SelectedStartDate
        {
            get => _selectedStartDate;
            set
            {
                if (_selectedStartDate != value)
                {
                    _selectedStartDate = value;
                    OnPropertyChanged(nameof(SelectedStartDate));
                }
            }
        }

        private DateTime _selectedEndDate;
        public DateTime SelectedEndDate
        {
            get => _selectedEndDate;
            set
            {
                if (_selectedEndDate != value)
                {
                    _selectedEndDate = value;
                    OnPropertyChanged(nameof(SelectedEndDate));
                }
            }
        }

        private string _lenghtOfStay;
        public string LenghtOfStay
        {
            get => _lenghtOfStay;
            set
            {
                if (_lenghtOfStay != value)
                {
                    _lenghtOfStay = value;
                    OnPropertyChanged(nameof(LenghtOfStay));
                }
            }
        }
        
        private string _guestNumber;
        public string GuestNumber
        {
            get => _guestNumber;
            set
            {
                if (_guestNumber != value)
                {
                    _guestNumber = value;
                    OnPropertyChanged(nameof(GuestNumber));
                }
            }
        }

        private ObservableCollection<AvailableDate> _availableDates;
        public ObservableCollection<AvailableDate> AvailableDates
        {
            get => _availableDates;
            set
            {
                if (_availableDates != value)
                {
                    _availableDates = value;
                    OnPropertyChanged(nameof(AvailableDates));
                }
            }
        }

        private AvailableDate _selectedAvailableDate;
        public AvailableDate SelectedAvailableDate
        {
            get => _selectedAvailableDate;
            set
            {
                if (_selectedAvailableDate != value)
                {
                    _selectedAvailableDate = value;
                    OnPropertyChanged(nameof(SelectedAvailableDate));
                }
            }
        }

        private Regex _NaturalNumberRegex = new Regex("^[1-9][0-9]*$");
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AccommodationReservationForm(User user, AccommodationRepository accommodationRepository, LocationRepository locationRepository, AccommodationImageRepository accommodationImageRepository, AccommodationReservationRepository accommodationReservationRepository, Accommodation selectedAccommodation)
        {
            InitializeComponent();
            DataContext = this;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            _accommodationRepository = accommodationRepository;
            _locationRepository = locationRepository;
            _accommodationImageRepository = accommodationImageRepository;
            _accommodationReservationRepository = accommodationReservationRepository;
            LoggedUser = user;
            SelectedAccommodation = selectedAccommodation;
            AvailableDates = new ObservableCollection<AvailableDate>();
        }

        private void SearchDatesButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsDateSearchInputValid()) return;

            AvailableDates.Clear();
            List<DateTime> allSingleDates = FindDatesBetween(SelectedStartDate, SelectedEndDate);

            List<DateTime> availableSingleDates = new List<DateTime>();
            foreach (var date in allSingleDates)
            {
                availableSingleDates.Add(date);
            }

            foreach (var date in allSingleDates)
            {
                RemoveIfUnavailable(date, availableSingleDates);
            }

            List<AvailableDate> availableDates = FindConnectedDates(availableSingleDates, SelectedEndDate);
            foreach(var date in availableDates)
            {
                AvailableDates.Add(date);
            }     
            
            if (AvailableDates.Count() == 0)
            {
                List<AvailableDate> recommendedDates = FindRecommendedDates();
                foreach (var date in recommendedDates)
                {
                    AvailableDates.Add(date);
                }
                MessageBox.Show("There are no available days for selected dates.\n" +
                        "Available dates around selected date are shown instead.");
            }
        }

        private List<DateTime> FindDatesBetween(DateTime startDate, DateTime endDate)
        {
            List<DateTime> resultingDates= new List<DateTime>();
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                resultingDates.Add(date);
            }

            return resultingDates;
        }

        private void RemoveIfUnavailable(DateTime date, List<DateTime> dates)
        {
            foreach (var accommodationReservation in _accommodationReservationRepository.GetAll())
            {
                if (FindDatesBetween(accommodationReservation.StartDate, accommodationReservation.EndDate).Contains(date) && accommodationReservation.AccommodationId == SelectedAccommodation.Id)
                {
                    dates.Remove(date);
                }
            }
        }

        private List<AvailableDate> FindConnectedDates(List<DateTime> singleDates, DateTime finishEndDate)
        {
            List<AvailableDate> connectedDates = new List<AvailableDate>(); 
            foreach (var singleDate in singleDates)
            {
                AvailableDate newDate = new AvailableDate(singleDate, singleDate.AddDays(Convert.ToInt32(LenghtOfStay) - 1));
                foreach (var date in FindDatesBetween(newDate.StartDate, newDate.EndDate))
                {
                    if (!singleDates.Contains(date))
                    {
                        break;
                    }
                    else if (date == newDate.EndDate)
                    {
                        if (DateTime.Compare(newDate.EndDate, finishEndDate) <= 0)
                        {
                            connectedDates.Add(newDate);
                        }
                    }
                }             
            }
            
            return connectedDates;
        }

        private bool IsDateSearchInputValid()
        {
            if (DateTime.Compare(SelectedStartDate, SelectedEndDate) >= 0)
            {
                MessageBox.Show("First date must be before the second one.");
                return false;
            }

            if (DateTime.Compare(DateTime.Now.Date, SelectedStartDate.Date) > 0) 
            {
                MessageBox.Show("You can't travel to the past.");
                return false;
            }

            if (LenghtOfStay == null || LenghtOfStay == "")
            {
                MessageBox.Show("Duration of stay field can't be empty.");
                return false;
            }

            if (!_NaturalNumberRegex.Match(LenghtOfStay).Success)
            {
                MessageBox.Show("Please enter a valid value.");
                return false;
            }

            if ((SelectedEndDate.Date - SelectedStartDate.Date).Days < Convert.ToInt32(LenghtOfStay) - 1)
            {
                MessageBox.Show("Calendar and Duration of stay values don't match.");
                return false;
            }

            if (Convert.ToInt32(LenghtOfStay) < SelectedAccommodation.MinDaysForStay)
            {
                MessageBox.Show("Minimum days for stay is " + SelectedAccommodation.MinDaysForStay + ".");
                return false;
            }

            return true;
        }

        private List<AvailableDate> FindRecommendedDates()
        {
            DateTime recommendationStartDate;
            if ((SelectedStartDate.Date - DateTime.Now.Date).Days < 14)
            {
                recommendationStartDate = DateTime.Now.Date;
            }
            else
            {
                recommendationStartDate = SelectedStartDate.AddDays(-14);
            }
            DateTime recommendationEndDate = SelectedEndDate.AddDays(14);
            List<DateTime> singleDates = FindDatesBetween(recommendationStartDate, recommendationEndDate);
            List<DateTime> availableSingleDates = FindDatesBetween(recommendationStartDate, recommendationEndDate);

            foreach (var date in singleDates)
            {
                RemoveIfUnavailable(date, availableSingleDates);
            }
            List<AvailableDate> recommendedDays = FindConnectedDates(availableSingleDates, recommendationEndDate);

            return recommendedDays;
        }

        private void MakeReservationButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsMakeReservationInputValid()) return;

            _accommodationReservationRepository.Save(SelectedAvailableDate.StartDate, SelectedAvailableDate.EndDate, Convert.ToInt32(LenghtOfStay), SelectedAccommodation, SelectedAccommodation.Id, LoggedUser, LoggedUser.Id);
            Close();
            MessageBox.Show("Reservation for " + SelectedAccommodation.Name + " (" + LenghtOfStay + " days) is successfully made!");
        }

        private bool IsMakeReservationInputValid()
        {
            if (GuestNumber == null || GuestNumber == "")
            {
                MessageBox.Show("Number of guests field can't be empty.");
                return false;
            }

            if (!_NaturalNumberRegex.Match(GuestNumber).Success)
            {
                MessageBox.Show("Please enter a valid value.");
                return false;
            }

            if (Convert.ToInt32(GuestNumber) > SelectedAccommodation.Capacity)
            {
                MessageBox.Show("Maximum capacity is " + SelectedAccommodation.Capacity + ".");
                return false;
            }

            if (SelectedAvailableDate == null)
            {
                MessageBox.Show("Please select date for the reservation.");
                return false;
            }

            return true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ViewImageButton_Click(object sender, RoutedEventArgs e)
        {
            if (_accommodationImageRepository.GetAll().Find(x => x.AccommodationId == SelectedAccommodation.Id) != null) 
            {
                AccommodationImagesOverview accommodationImagesOverview = new AccommodationImagesOverview(SelectedAccommodation, _accommodationImageRepository);
                accommodationImagesOverview.Show();
            }
            else
            {
                MessageBox.Show("Image url can not be loaded.");
            }
        }
    }

    public class AvailableDate 
    {
        public DateTime StartDate { get; set;}
        public DateTime EndDate { get; set;}
        public AvailableDate() { }
        public AvailableDate(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}


