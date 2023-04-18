using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.DTOs;
using InitialProject.Domain.Models;
using InitialProject.WPF.Views.Guest2Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace InitialProject.WPF.ViewModels.Guest2ViewModels
{
    public class ReservedToursViewModel : ViewModelBase
    {
        #region PROPERTIES
        public User LoggedUser { get; set; }

        private TourCheckpoint? _selectedReservedTour;
        public TourCheckpoint? SelectedReservedTour
        {
            get
            {
                return _selectedReservedTour;
            }
            set
            {
                if (_selectedReservedTour != value)
                {
                    _selectedReservedTour = value;
                    OnPropertyChanged(nameof(SelectedReservedTour));
                }
            }
        }
        private bool _isEnabled;
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    OnPropertyChanged(nameof(IsEnabled));
                }
            }
        }

        public ObservableCollection<TourCheckpoint> ReservedTours { get; set; }
        private readonly Window _reservedToursView;
        private readonly TourService _tourService;
        private readonly TourReviewService _tourReviewService;
        #endregion

        public ReservedToursViewModel(Window reservedToursView, User user)
        {
            _reservedToursView = reservedToursView;
            _tourService = new TourService();
            LoggedUser = user;
            ReservedTours = new ObservableCollection<TourCheckpoint>();
            _tourReviewService = new TourReviewService();

            CloseWindowCommand = new RelayCommand(CloseWindowCommand_Execute);
            OpenRateTourAndGuideWindowCommand = new RelayCommand(OpenRateTourAndGuideWindowCommand_Execute);
            //IsEnabled = false;
            LoadReservedTours();
        }

        public void LoadReservedTours()
        {
            ReservedTours = _tourService.GetReservedTours(LoggedUser);
        }

        public bool HasSelectedValidReservation()
        {
            if(SelectedReservedTour != null && SelectedReservedTour.Status == TourStatus.FINISHED)
            {
                return true;
            }
            return false;
        }

        public bool IsAlreadyRated(int tourId, int userId)
        {
            return _tourReviewService.HasAlreadyBeenRated(tourId, userId);
        }

        #region COMMANDS

        public RelayCommand CloseWindowCommand { get; }
        public RelayCommand OpenRateTourAndGuideWindowCommand { get; }
        public RelayCommand SelectionChangedCommand { get; }

        public void CloseWindowCommand_Execute(object? parameter)
        {
            _reservedToursView.Close();
        }
        public void OpenRateTourAndGuideWindowCommand_Execute(object? parameter)
        {
            if (!HasSelectedValidReservation())
            {
                MessageBox.Show("Selected tour has to be FINISHED in order to be rated.");
            }
            else if (IsAlreadyRated(SelectedReservedTour.TourId, LoggedUser.Id))
            {
                MessageBox.Show("This tour has already been rated.");
            }
            else
            {
                RateTourAndGuideForm rateTourAndGuideForm = new RateTourAndGuideForm(SelectedReservedTour.TourId, LoggedUser);
                rateTourAndGuideForm.Show();
                _reservedToursView.Close();
            }
        }
        #endregion
    }
}
