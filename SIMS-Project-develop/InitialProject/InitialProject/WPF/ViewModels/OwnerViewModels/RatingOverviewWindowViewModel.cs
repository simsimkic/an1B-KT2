using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Models;
using InitialProject.WPF.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.WPF.ViewModels.OwnerViewModels
{
    public class RatingOverviewWindowViewModel : ViewModelBase
    {
        #region PROPERTIES
        private AccommodationReservation _selectedAccommodationReservation;
        public AccommodationReservation SelectedAccommodationReservation
        {
            get
            {
                return _selectedAccommodationReservation;
            }
            set
            {
                if (_selectedAccommodationReservation != value)
                {
                    _selectedAccommodationReservation = value;
                    OnPropertyChanged(nameof(SelectedAccommodationReservation));
                }
            }
        }

        private Rating _ownerRated;
        public Rating OwnerRated
        {
            get
            { 
                return _ownerRated; 
            }
            set
            {
                if (_ownerRated != value)
                {
                    _ownerRated = value;
                    OnPropertyChanged(nameof(OwnerRated));
                }
            }
        }

        private AccommodationRating _guestRated;

        public AccommodationRating GuestRated
        {
            get
            {
                return _guestRated;
            }
            set
            {
                if (_guestRated != value)
                {
                    _guestRated = value;
                    OnPropertyChanged(nameof(GuestRated)); 
                }
            }
        }

        private readonly Window _ratingOverviewWindow;
        private readonly RatingService _ratingService;
        private readonly AccommodationRatingService _accommodationRatingService;
        #endregion

        public RatingOverviewWindowViewModel(Window ratingOverviewWindow, AccommodationReservation selectedAccommodationReservation)
        {
            _ratingOverviewWindow = ratingOverviewWindow;
            SelectedAccommodationReservation = selectedAccommodationReservation;
            _ratingService = new RatingService();
            _accommodationRatingService = new AccommodationRatingService();
            FindOwnerRating();
            FindGuestRating();

            CloseWindowCommand = new RelayCommand(CloseWindowCommand_Execute);
        }

        private void FindOwnerRating()
        {
            OwnerRated = _ratingService.FindRatingByReservationId(SelectedAccommodationReservation.Id);

            if (OwnerRated == null)
            {
                OwnerRated = new Rating();
                OwnerRated.Cleanliness = 0;
                OwnerRated.FollowingTheRules = 0;
                OwnerRated.Comment = "nije ocenjen";
            }
        }

        
        private void FindGuestRating()
        {
            GuestRated = _accommodationRatingService.FindAccommodationRatingByReservationId(SelectedAccommodationReservation.Id);

            if (GuestRated == null)
            {
                GuestRated = new AccommodationRating();
                GuestRated.Cleanliness = 0;
                GuestRated.Correctness = 0;
                GuestRated.Comment = "nije ocenjen";
            }

        }

        #region COMMANDS
        public RelayCommand CloseWindowCommand { get; }

        public void CloseWindowCommand_Execute(object? parameter)
        {
            _ratingOverviewWindow.Close();
        }
        #endregion
    }
}
