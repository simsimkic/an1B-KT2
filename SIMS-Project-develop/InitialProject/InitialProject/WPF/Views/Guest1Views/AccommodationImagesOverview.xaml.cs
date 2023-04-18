using InitialProject.Domain.Models;
using InitialProject.Repositories;
using System;
using System.Collections.Generic;
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
using static System.Net.Mime.MediaTypeNames;

namespace InitialProject.WPF.Views
{
    /// <summary>
    /// Interaction logic for AccommodationImagesOverview.xaml
    /// </summary>
    public partial class AccommodationImagesOverview : Window, INotifyPropertyChanged
    {
        public readonly AccommodationRepository _accommodationRepository;
        public readonly AccommodationImageRepository _accommodationImageRepository;
        public Accommodation SelectedAccommodation { get; set; }

        private AccommodationImage _currentImage;
        public AccommodationImage CurrentImage
        {
            get => _currentImage;
            set
            {
                if (_currentImage != value)
                {
                    _currentImage = value;
                    OnPropertyChanged(nameof(CurrentImage));
                }
            }
        }
        public List<AccommodationImage> AccommodationImages { get; set; }

        public AccommodationImagesOverview(Accommodation selectedAccommodation, AccommodationImageRepository accommodationImageRepository)
        {
            InitializeComponent();
            DataContext = this;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            SelectedAccommodation = selectedAccommodation;
            _accommodationImageRepository = accommodationImageRepository;
            AccommodationImages = FindSelectedAccommodationImages();
            CurrentImage = AccommodationImages[0];
            if (AccommodationImages.Count > 1)
            {
                ButtonNextImage.IsEnabled = true;
                ButtonNextImage.Opacity = 100;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private List<AccommodationImage> FindSelectedAccommodationImages()
        {
            List<AccommodationImage> accommodationImages = new List<AccommodationImage>();
            foreach (var image in _accommodationImageRepository.GetAll())
            {
                if (image.AccommodationId == SelectedAccommodation.Id)
                {
                    accommodationImages.Add(image);
                }
            }
            return accommodationImages;
        }
        private int GetImageIndex()
        {
            return AccommodationImages.IndexOf(CurrentImage);
        }

        private void RightArrowButton_Click(object sender, RoutedEventArgs e)
        {
            var currentIndex = GetImageIndex();
            var isSecondToLastImage = currentIndex == AccommodationImages.Count - 2;

            if (isSecondToLastImage)
            {
                DisableButton(ButtonNextImage);
            }
            EnableButton(ButtonPreviousImage);

            CurrentImage = AccommodationImages[currentIndex + 1];
        }

        private void LeftArrowButton_Click(object sender, RoutedEventArgs e)
        {
            var currentIndex = GetImageIndex();
            var isSecondImage = currentIndex == 1;
            if (isSecondImage)
            {
                DisableButton(ButtonPreviousImage);
            }
            EnableButton(ButtonNextImage);

            CurrentImage = AccommodationImages[currentIndex - 1];
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
    }
}
