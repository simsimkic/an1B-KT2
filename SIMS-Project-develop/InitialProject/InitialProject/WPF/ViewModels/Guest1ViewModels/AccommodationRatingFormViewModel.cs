using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.WPF.ViewModels.Guest1ViewModels
{
    public class AccommodationRatingFormViewModel : ViewModelBase
    {
        #region PROPERTIES
        private string? _cleanliness;
        public string? Cleanliness
        {
            get 
            { 
                return _cleanliness; 
            }
            set
            {
                if (_cleanliness != value)
                {
                    _cleanliness = value;
                    OnPropertyChanged(nameof(Cleanliness));
                }
            }
        }
        private string? _correctness;
        public string? Correctness
        {
            get
            {
                return _correctness;
            }
            set
            {
                if (_correctness != value)
                {
                    _correctness = value;
                    OnPropertyChanged(nameof(Correctness));
                }
            }
        }
        private string? _comment;
        public string? Comment
        {
            get
            {
                return _comment;
            }
            set
            {
                if (_comment != value)
                {
                    _comment = value;
                    OnPropertyChanged(nameof(Comment));
                }
            }
        }
        private string? _imageUrl;
        public string? ImageUrl
        {
            get
            {
                return _imageUrl;
            }
            set
            {
                if (_imageUrl != value)
                {
                    _imageUrl = value;
                    OnPropertyChanged(nameof(ImageUrl));
                }
            }
        }
        private string? _currentImageUrl;
        public string? CurrentImageUrl
        {
            get
            {
                return _currentImageUrl;
            }
            set
            {
                if (_currentImageUrl != value)
                {
                    _currentImageUrl = value;
                    OnPropertyChanged(nameof(CurrentImageUrl));
                }
            }
        }
        public List<string> ImageUrls { get; set; }

        private readonly AccommodationReservation _selectedReservation;
        private readonly Window _accommodationRatingForm;
        private readonly AccommodationRatingService _accommodationRatingService;
        private readonly SetOwnerRoleService _setOwnerRoleService;
        #endregion

        public AccommodationRatingFormViewModel(Window accommodationRatingForm, AccommodationReservation reservation)
        {
            _accommodationRatingForm = accommodationRatingForm;
            _accommodationRatingService = new AccommodationRatingService();
            _setOwnerRoleService = new SetOwnerRoleService();
            _selectedReservation = reservation;
            ImageUrls = new List<string>();

            AddImageCommand = new RelayCommand(AddImageCommand_Execute, AddImageCommand_CanExecute);
            RemoveImageCommand = new RelayCommand(RemoveImageCommand_Execute, RemoveImageCommand_CanExecute);
            RateCommand = new RelayCommand(RateCommand_Execute, RateCommand_CanExecute);
            CancelCommand = new RelayCommand(CancelCommand_Execute);
        }

        #region COMMANDS
        public RelayCommand AddImageCommand { get; }
        public RelayCommand RemoveImageCommand { get; }
        public RelayCommand RateCommand { get; }
        public RelayCommand CancelCommand { get; }

        public void AddImageCommand_Execute(object? parameter)
        {
            CurrentImageUrl = ImageUrl;
            ImageUrls.Add(ImageUrl);
        }

        public bool AddImageCommand_CanExecute(object? parameter)
        {
            return CurrentImageUrl != ImageUrl && ImageUrl is not null && ImageUrl != "";
        }

        public void RemoveImageCommand_Execute(object? parameter)
        {
            int index = ImageUrls.IndexOf(ImageUrls.Find(x => x.Equals(CurrentImageUrl)));
            ImageUrls.Remove(ImageUrls.Find(x => x.Equals(CurrentImageUrl)));
            if (index > 0) CurrentImageUrl = ImageUrls[index - 1];
            else CurrentImageUrl = null;
        }

        public bool RemoveImageCommand_CanExecute(object? parameter)
        {
            return ImageUrls.Count != 0;
        }

        public void RateCommand_Execute(object? parameter)
        {
            AccommodationRating accommodationRating = _accommodationRatingService.SaveAccommodationRating(Convert.ToInt32(Cleanliness), Convert.ToInt32(Correctness), Comment, _selectedReservation.Id, _selectedReservation.Accommodation.OwnerId, _selectedReservation.GuestId);
            foreach (var url in ImageUrls)
            {
                _accommodationRatingService.SaveImage(url, accommodationRating.Id);
            }

            _setOwnerRoleService.SetOwnerRole(accommodationRating.OwnerId);
            _accommodationRatingForm.Close();
        }

        public bool RateCommand_CanExecute(object? parameter)
        {
            return Cleanliness is not null && Correctness is not null;
        }

        public void CancelCommand_Execute(object? parameter)
        {
            _accommodationRatingForm.Close();
        }
        #endregion
    }
}
