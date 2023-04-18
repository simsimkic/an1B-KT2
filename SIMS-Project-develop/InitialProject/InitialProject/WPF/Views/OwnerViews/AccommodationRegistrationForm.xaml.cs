using InitialProject.Domain.Models;
using InitialProject.Repositories;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
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
using static System.Net.WebRequestMethods;

namespace InitialProject.WPF.Views
{
    /// <summary>
    /// Interaction logic for AccommodationRegistrationForm.xaml
    /// </summary>
    public partial class AccommodationRegistrationForm : Window
    {
        public User LoggedInUser { get; set; }

        public Accommodation SelectedAccommodation { get; set; }

        private readonly AccommodationRepository _accommodationRepository;
        private readonly LocationRepository _locationRepository;
        private readonly AccommodationImageRepository _imageRepository;
        private readonly UserRepository _userRepository;

        private int _imageNumber;
        private int _ownerId;

        private string _accommodationName;
        public string AccommodationName
        {
            get => _accommodationName;
            set
            {
                if (value != _accommodationName)
                {
                    _accommodationName = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        private string _city;
        public string City
        {
            get => _city;
            set
            {
                if (value != _city)
                {
                    _city = value;
                    OnPropertyChanged("City");
                }
            }
        }

        private string _country;
        public string Country
        {
            get => _country;
            set
            {
                if (_country != value)
                {
                    _country = value;
                    OnPropertyChanged("Country");
                }
            }
        }

        private string _type;

        public string Type
        {
            get => _type;
            set
            {
                if (value != _type)
                {
                    _type = value;
                    OnPropertyChanged("Type");
                }
            }
        }

        private string _capacity;
        public string Capacity
        {
            get => _capacity;
            set
            {
                if (_capacity != value)
                {
                    _capacity = value;
                    OnPropertyChanged("Capacity");
                }
            }
        }

        private string _minDaysForStay;
        public string MinDaysForStay
        {
            get => _minDaysForStay;
            set
            {
                if (_minDaysForStay != value)
                {
                    _minDaysForStay = value;
                    OnPropertyChanged("MinDaysForStay");
                }
            }
        }

        private string _minDaysBeforeCancel;
        public string MinDaysBeforeCancel
        {
            get => _minDaysBeforeCancel;
            set
            {
                if (_minDaysBeforeCancel != value)
                {
                    _minDaysBeforeCancel = value;
                    OnPropertyChanged("MinDaysBeforeCancel");
                }
            }
        }

        private string _superOwnerMark;
        public string SuperOwnerMark
        {
            get => _superOwnerMark;
            set
            {
                if (_superOwnerMark != value)
                {
                    _superOwnerMark = value;
                    OnPropertyChanged("SuperOwnerMark");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AccommodationRegistrationForm(AccommodationRepository accommodationRepository, LocationRepository locationRepository, AccommodationImageRepository imageRepository, int ownerId, UserRepository userRepository)
        {
            InitializeComponent();
            this.DataContext = this;
            _accommodationRepository = accommodationRepository;
            _locationRepository = locationRepository;
            _imageRepository = imageRepository;
            _userRepository = userRepository;
            _imageNumber = 0;
            _ownerId = ownerId;
            MinDaysBeforeCancel = "1";
        }

        public AccommodationType FindType(string type)
        {
            switch (type)
            {
                case "apartment":
                    return AccommodationType.apartment;
                case "house":
                    return AccommodationType.house;
                case "cottage":
                    return AccommodationType.cottage;
                default:
                    return 0;
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            _imageRepository.RemovePicturesForCanceledAccommodation();
            this.Close();
        }

        private void ButtonRegister_Click(object sender, RoutedEventArgs e)
        {
            Location location = _locationRepository.GetByCountryAndCity(Country, City);
            AccommodationType accommodationType = FindType(Type);

            if (location != null)
            {
                SetSuperOwnerMark();
                int accommodationId = _accommodationRepository.Save(AccommodationName, location, accommodationType, Capacity, MinDaysForStay, MinDaysBeforeCancel, _ownerId, SuperOwnerMark, _locationRepository).Id;
                _imageRepository.AddAccommodationId(accommodationId);
            }
            
            this.Close();
        }

        private void SetSuperOwnerMark()
        {
            SuperOwnerMark = " ";

            User owner = _userRepository.GetById(_ownerId);

            if (owner.Role == UserRole.SUPER_OWNER)
            {
                SuperOwnerMark = "*";
            }
        }

        private void AccommodationRegistrationLoaded(object sender, RoutedEventArgs e)
        {
            List<string> countries = _locationRepository.GetAllCountries();
            ComboBoxCountry.ItemsSource = countries;
        }

        private void EnableButtonIfValid()
        {
            if (IsValid)
            {
                ButtonRegister.IsEnabled = true;
            }
            else
            {
                ButtonRegister.IsEnabled = false;
            }
        }

        public void FillInCities(List<string> comboBoxCityItems)
        {
            EnableButtonIfValid();

            foreach (var comboCity in comboBoxCityItems)
            {
                ComboBoxCity.Items.Add(comboCity);
            }
        }

        private void ComboBoxCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableButtonIfValid();

            ComboBoxCity.IsEnabled = true;
            ComboBoxCity.Items.Clear();

            List<string> comboBoxCityItems = _locationRepository.GetCitiesByCountry(ComboBoxCountry.SelectedItem.ToString());

            FillInCities(comboBoxCityItems);
        }

        private void ButtonAddImages_Click(object sender, RoutedEventArgs e)
        {
            String url = TestTextBox.Text;

            if (IsImageAlreadyAdded(url))
            {
                MessageBox.Show("Image already added. Type another url");
            }
            else if (url != null && url != "")
            {
                Uri resourceUri = new Uri(url);
                UploadedPicture.Source = new BitmapImage(resourceUri);
                ButtonSaveImage.IsEnabled = true;
            }

            TextBlockPictureSaved.Text = "";
        }

        public bool IsImageAlreadyAdded(string url)
        {
            AccommodationImage image = _imageRepository.GetAll().Find(i => i.Url == url && i.AccommodationId == -1);

            if (image != null)
            {
                return true;
            }
            return false;
            
        }

        private void ButtonSaveImage_Click(object sender, RoutedEventArgs e)
        {
            TextBlockPictureSaved.Text = "Image added, if you want to add more images, type another url and click button 'Add images'";

            _imageNumber++;
            _imageRepository.Save(TestTextBox.Text, -1);

            EnableButtonIfValid();

            ButtonSaveImage.IsEnabled = false;
            ButtonAddImages.IsEnabled = false;
        }

        private Regex _Url = new Regex("^https?:\\/\\/[-a-zA-Z0-9@:%._\\+~#=]{1,256}\\.[a-zA-Z0-9()]{1,6}\\b(?:[-a-zA-Z0-9()@:%_\\+.~#?&\\/=]*)$");

        private void TestTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Match urlMatch = _Url.Match(TestTextBox.Text);

            if (urlMatch.Success)
            {
                ButtonAddImages.IsEnabled = true;
            } else
            {
                ButtonAddImages.IsEnabled = false;
            }

            EnableButtonIfValid();

            if (TestTextBox.Text == null || TestTextBox.Text == "")
            {
                ButtonSaveImage.IsEnabled = false;
            }
        }

        private Regex _Number = new Regex(@"^[1-9][0-9]*$");

        public bool IsValid
        {
            get
            { 
                if (AccommodationName == null || AccommodationName == "")
                {
                    return false;
                }
                else if (Country == null)
                {
                    return false;
                }
                else if (City == null)
                {
                    return false;
                }
                else if (Type == null)
                {
                    return false;
                }
                else if (Capacity == null || Capacity == "")
                {
                    return false;
                }
                else if (MinDaysForStay == null || MinDaysForStay == "")
                {
                    return false;
                }
                else if (MinDaysBeforeCancel == null || MinDaysBeforeCancel == "")
                {
                    return false;
                }
                else if (_imageNumber == 0)
                {
                    return false;
                }
                else if (ComboBoxCity.SelectedItem == null)
                {
                    return false;
                }

                Match capacityMatch = _Number.Match(Capacity);
                Match minDaysForStay = _Number.Match(MinDaysForStay);
                Match minDayBeforeCancel = _Number.Match(MinDaysBeforeCancel);

                if (!capacityMatch.Success || !minDaysForStay.Success || !minDayBeforeCancel.Success)
                {
                    return false;
                }

                return true;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableButtonIfValid();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _imageRepository.RemovePicturesForCanceledAccommodation();
        }
    }
}
