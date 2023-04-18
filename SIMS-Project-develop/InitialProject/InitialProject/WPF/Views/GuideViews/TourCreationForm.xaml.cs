using InitialProject.Domain.Models;
using InitialProject.Repositories;
using Microsoft.Win32;
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
    /// Interaction logic for TourCreationForm.xaml
    /// </summary>
    public partial class TourCreationForm : Window, INotifyPropertyChanged, IDataErrorInfo
    {
        private TourRepository _tourRepository;
        private TourImageRepository _tourImageRepository;
        private LocationRepository _locationRepository;
        private CheckpointRepository _checkpointRepository;
        private User _guide;

        private string _tourName;
        public string TourName
        {
            get => _tourName;

            set
            {
                if (_tourName != value)
                {
                    _tourName = value;
                    OnPropertyChanged(nameof(TourName));
                }
            }
        }

        private DateTime _datePickerSelectedDate;
        public DateTime DatePickerSelectedDate
        {
            get => _datePickerSelectedDate;
            set
            {
                if (_datePickerSelectedDate != value)
                {
                    _datePickerSelectedDate = value;
                    OnPropertyChanged(nameof(DatePickerSelectedDate));
                }
            }
        }

        private DateTime _dataGridSelectedDate;
        public DateTime DataGridSelectedDate
        {
            get => _dataGridSelectedDate;
            set
            {
                if (_dataGridSelectedDate != value)
                {
                    _dataGridSelectedDate = value;
                    OnPropertyChanged(nameof(DataGridSelectedDate));
                }
            }
        }

        private string _selectedCountry;
        public string SelectedCountry
        {
            get => _selectedCountry;
            set
            {
                if (_selectedCountry != value)
                {
                    _selectedCountry = value;
                    OnPropertyChanged(nameof(SelectedCountry));
                }
            }
        }

        private string _selectedCity;
        public string SelectedCity
        {
            get => _selectedCity;
            set
            {
                if (_selectedCity != value)
                {
                    _selectedCity = value;
                    OnPropertyChanged(nameof(SelectedCity));
                }
            }
        }

        private int _duration;
        public int Duration
        {
            get => _duration;
            set
            {
                if (_duration != value)
                {
                    _duration = value;
                    OnPropertyChanged(nameof(Duration));
                }
            }
        }

        private string _selectedLanguage;
        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                if (_selectedLanguage != value)
                {
                    _selectedLanguage = value;
                    OnPropertyChanged(nameof(SelectedLanguage));
                }
            }
        }

        private int _maxGuests;
        public int MaxGuests
        {
            get => _maxGuests;
            set
            {
                if (_maxGuests != value)
                {
                    _maxGuests = value;
                    OnPropertyChanged(nameof(MaxGuests));
                }
            }
        }

        private string _enteredCheckpointName;
        public string EnteredCheckpointName
        {
            get => _enteredCheckpointName;
            set
            {
                if (_enteredCheckpointName != value)
                {
                    _enteredCheckpointName = value;
                    OnPropertyChanged(nameof(EnteredCheckpointName));
                }
            }
        }

        private string _selectedCheckpointName;
        public string SelectedCheckpointName
        {
            get => _selectedCheckpointName;
            set
            {
                if (_selectedCheckpointName != value)
                {
                    _selectedCheckpointName = value;
                    OnPropertyChanged(nameof(SelectedCheckpointName));
                }
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        private BitmapImage _selectedImage;
        public BitmapImage SelectedImage
        {
            get => _selectedImage;
            set
            {
                if (_selectedImage != value)
                {
                    _selectedImage = value;
                    OnPropertyChanged(nameof(SelectedImage));
                }
            }
        }


        public ObservableCollection<DateTime> Dates { get; set; }
        public ObservableCollection<string> Countries { get; set; }
        public ObservableCollection<string> Cities { get; set; }
        public ObservableCollection<string> Languages { get; set; }
        public ObservableCollection<string> CheckpointNames { get; set; }
        public List<BitmapImage> Images { get; set; }


        public TourCreationForm(TourRepository tourRepository, TourImageRepository tourImageRepository, LocationRepository locationRepository, CheckpointRepository checkpointRepository, User guide)
        {
            InitializeComponent();
            this.DataContext = this;

            _tourRepository = tourRepository;
            _tourImageRepository = tourImageRepository;
            _locationRepository = locationRepository;
            _checkpointRepository = checkpointRepository;
            _guide = guide;

            Dates = new ObservableCollection<DateTime>();
            Countries = new ObservableCollection<string>();
            Cities = new ObservableCollection<string>();
            Languages = new ObservableCollection<string>() { "Serbian", "Hungarian", "German", "Thai", "French", "Italian", "Turkish", "Chinese", "Bulgarian", "Swedish", "Finish", "Croatian", "Bosnian", "Japanese", "Eren Yeager", "Danish", "English", "Romanian", "Greek", "Albanian", "Ukranian", "Russian", "Slovenian", "Slovakian", "Belgian", "Dutch", "Portuguese", "Spanish", "Lithuanian", "Estonian" };
            CheckpointNames = new ObservableCollection<string>();
            Images = new List<BitmapImage>();

            DatePickerSelectedDate = DateTime.Now;

            LoadCountries();
        }
        private void LoadCountries()
        {
            foreach (var location in _locationRepository.GetAll())
            {
                if (Countries.Contains(location.Country)) continue;
                Countries.Add(location.Country);
            }
        }
        private void LoadCities()
        {
            Cities.Clear();
            foreach (var location in _locationRepository.GetAll())
            {
                if (!location.Country.Equals(SelectedCountry)) continue;
                Cities.Add(location.City);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Error => null;
        public string this[string columnName]
        {
            get
            {
                if (columnName.Equals(nameof(TourName)))
                {
                    if (string.IsNullOrEmpty(TourName))
                    {
                        return "Name is required";
                    }
                }
                else if (columnName.Equals(nameof(SelectedCountry)))
                {
                    if (string.IsNullOrEmpty(SelectedCountry))
                    {
                        return "Country is required";
                    }
                }
                else if (columnName.Equals(nameof(SelectedCity)))
                {
                    if (string.IsNullOrEmpty(SelectedCity))
                    {
                        return "City is required";
                    }
                }
                else if (columnName.Equals(nameof(Duration)))
                {
                    if (Duration < 1)
                    {
                        return "Must be positive";
                    }
                }
                else if (columnName.Equals(nameof(SelectedLanguage)))
                {
                    if (string.IsNullOrEmpty(SelectedLanguage))
                    {
                        return "Language is required";
                    }
                }
                else if (columnName.Equals(nameof(MaxGuests)))
                {
                    if (MaxGuests < 1)
                    {
                        return "Must be positive";
                    }
                }
                else if (columnName.Equals(nameof(Description)))
                {
                    if (string.IsNullOrEmpty(Description))
                    {
                        return "Description is required";
                    }
                }
                else if (columnName.Equals(nameof(Dates)))
                {
                    if (Dates.Count == 0)
                    {
                        return "At least 1 date is required";
                    }
                }
                else if (columnName.Equals(nameof(CheckpointNames)))
                {
                    if (CheckpointNames.Count < 2)
                    {
                        return "At least 2 checkpoints are required";
                    }
                }
                else if (columnName.Equals(nameof(SelectedImage)))
                {
                    if (SelectedImage == null)
                    {
                        return "At least 1 image is required";
                    }
                }
                return null;
            }
        }

        private readonly string[] _validatedProperties = { "TourName", "SelectedCountry", "SelectedCity", "Duration", "SelectedLanguage", "MaxGuests", "Description", "Dates", "CheckpointNames", "SelectedImage" };

        public bool IsValid
        {
            get
            {
                foreach (var property in _validatedProperties)
                {
                    if (this[property] != null)
                        return false;
                }

                return true;
            }
        }

        private void ComboBoxCities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(SelectedCountry))
            {
                ComboBoxCity.IsEnabled = false;
                return;
            }

            ComboBoxCity.IsEnabled = true;
            LoadCities();
        }

        private void ButtonAddDate_Click(object sender, RoutedEventArgs e)
        {
            if (Dates.Contains(DatePickerSelectedDate)) return;
            Dates.Add(DatePickerSelectedDate);
            EnableConfirmButtonIfValid();
        }

        private void EnableConfirmButtonIfValid()
        {
            if (IsValid)
            {
                ButtonConfirm.IsEnabled = true;
            }
            else
            {
                ButtonConfirm.IsEnabled = false;
            }
        }

        private void ButtonRemoveDate_Click(object sender, RoutedEventArgs e)
        {
            Dates.Remove(DataGridSelectedDate);
            EnableConfirmButtonIfValid();
        }

        private void ButtonAddCheckpoint_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(EnteredCheckpointName)) return;
            if (CheckpointNames.Contains(EnteredCheckpointName)) return;

            CheckpointNames.Add(EnteredCheckpointName);
            EnteredCheckpointName = "";

            EnableConfirmButtonIfValid();
        }

        private void ButtonRemoveCheckpoint_Click(object sender, RoutedEventArgs e)
        {
            CheckpointNames.Remove(SelectedCheckpointName);

            EnableConfirmButtonIfValid();
        }

        private void ButtonAddImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
 
            //dlg.DefaultExt = ".png";
            dlg.Filter = "JPG Files (*.jpg)|*.jpg|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|GIF Files (*.gif)|*.gif";
 
            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                AddImage(dlg.FileName);
            }

            EnableConfirmButtonIfValid();
        }

        private void AddImage(string fileName)
        {
            Uri uri = new Uri(fileName);
            if (SelectedImage == null)
            {
                SelectedImage = new BitmapImage(uri);
                Images.Add(SelectedImage);
            }
            else
            {
                var currentIndex = GetImageIndex();
                SelectedImage = new BitmapImage(uri);
                var isLastImage = currentIndex + 1 == Images.Count;
                if (!isLastImage)
                {
                    EnableButton(ButtonNextImage);
                }
                EnableButton(ButtonPreviousImage);
                Images.Insert(currentIndex + 1, SelectedImage);
            }
            ButtonRemoveImage.IsEnabled = true;
        }

        private int GetImageIndex()
        {
            return Images.IndexOf(SelectedImage);
        }

        private void EnableButton(Button button)
        {
            button.IsEnabled = true;
            button.Opacity = 100;
        }
        private void DisableButton(Button button)
        {
            button.IsEnabled = false;
            button.Opacity = 0;
        }

        private void ButtonPreviousImage_Click(object sender, RoutedEventArgs e)
        {
            var currentIndex = GetImageIndex();
            var isSecondImage = currentIndex == 1;
            if (isSecondImage)
            {
                DisableButton(ButtonPreviousImage);
            }
            EnableButton(ButtonNextImage);

            SelectedImage = Images[currentIndex - 1];
        }


        private void ButtonNextImage_Click(object sender, RoutedEventArgs e)
        {
            var currentIndex = GetImageIndex();
            var isSecondToLastImage = currentIndex == Images.Count - 2;

            if (isSecondToLastImage)
            {
                DisableButton(ButtonNextImage);
            }
            EnableButton(ButtonPreviousImage);

            SelectedImage = Images[currentIndex + 1];
        }

        private void ButtonRemoveImage_Click(object sender, RoutedEventArgs e)
        {
            var currentIndex = GetImageIndex();
            var isLastImage = currentIndex + 1 == Images.Count;
            var isSecondToLastImage = currentIndex + 2 == Images.Count;

            Images.Remove(SelectedImage);

            if (isLastImage)
            {
                if (Images.Count == 0)
                {
                    SelectedImage = null;
                    ButtonRemoveImage.IsEnabled = false;
                    EnableConfirmButtonIfValid();
                    return;
                }

                SelectedImage = Images[currentIndex - 1];

                if (Images.Count == 1)
                {
                    DisableButton(ButtonNextImage);
                    DisableButton(ButtonPreviousImage);
                }
            }
            else
            {
                SelectedImage = Images[currentIndex];

                if (isSecondToLastImage)
                {
                    DisableButton(ButtonNextImage);
                }
            }

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableConfirmButtonIfValid();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableConfirmButtonIfValid();
        }

        private void IntegerUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            EnableConfirmButtonIfValid();
        }

        private void ButtonConfirm_Click(object sender, RoutedEventArgs e)
        {
            Location location = _locationRepository.GetByCountryAndCity(SelectedCountry, SelectedCity);
            foreach (var startTime in Dates)
            {
                Tour tour = _tourRepository.Create(TourName, location, location.Id, Description, SelectedLanguage, MaxGuests, startTime, Duration, SelectedImage.ToString(), _guide.Id);
                foreach (var checkpointName in CheckpointNames)
                {
                    _checkpointRepository.Create(checkpointName, false, tour, tour.Id);
                }

                foreach (var image in Images)
                {
                    _tourImageRepository.Save(image.ToString(), tour.Id);
                }
            }
            this.Close();
        }
    }
}
