using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Models;
using InitialProject.Repositories;
using InitialProject.WPF.Views.OwnerViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.WPF.ViewModels.OwnerViewModels
{
    public class RatedGuestsOverviewViewModel : ViewModelBase
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

        private readonly Window _ratedGuestsOverview;
        private readonly AccommodationReservationService _accommodationReservationService;
        private readonly int _ownerId;

        public ObservableCollection<AccommodationReservation> RatedReservations { get; set; }
        #endregion

        public RatedGuestsOverviewViewModel(Window ratedGuestsOverview, int ownerId)
        {
            _ratedGuestsOverview = ratedGuestsOverview;
            _accommodationReservationService = new AccommodationReservationService();
            _ownerId = ownerId;

            RatedReservations = new ObservableCollection<AccommodationReservation>();

            SeeRatingCommand = new RelayCommand(SeeRatingCommand_Execute, SeeRatingCommand_CanExecute);
            CloseWindowCommand = new RelayCommand(CloseWindowCommand_Execute);

            LoadRatedReservations();
        }

        public void LoadRatedReservations()
        {
            RatedReservations.Clear();

            foreach (var reservation in _accommodationReservationService.GetRatedReservations(_ownerId))
            {
                RatedReservations.Add(reservation);
            }
        }

        #region COMMANDS
        public RelayCommand SeeRatingCommand { get; }
        public RelayCommand CloseWindowCommand { get; }

        public bool SeeRatingCommand_CanExecute(object? parameter)
        {
            return SelectedAccommodationReservation is not null;
        }

        public void SeeRatingCommand_Execute(object? parameter)
        {
            RatingOverviewWindow ratingOverviewWindow = new RatingOverviewWindow(SelectedAccommodationReservation);
            ratingOverviewWindow.Show();
        }

        public void CloseWindowCommand_Execute(object? parameter)
        {
            _ratedGuestsOverview.Close();
        }
        #endregion
    }
}
