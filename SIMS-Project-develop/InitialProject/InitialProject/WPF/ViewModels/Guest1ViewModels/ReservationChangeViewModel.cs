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
    public class ReservationChangeViewModel : ViewModelBase
    {
        #region PROPERTIES
        public AccommodationReservation SelectedReservation { get; set; }
        private DateTime _selectedStartDate;
        public DateTime SelectedStartDate
        {
            get
            {
                return _selectedStartDate;
            }
            set
            {
                if (value != _selectedStartDate)
                {
                    _selectedStartDate = value;
                    OnPropertyChanged(nameof(SelectedStartDate));
                }
            }
        }
        private DateTime _selectedEndDate;
        public DateTime SelectedEndDate
        {
            get
            {
                return _selectedEndDate;
            }
            set
            {
                if (value != _selectedEndDate)
                {
                    _selectedEndDate = value;
                    OnPropertyChanged(nameof(SelectedEndDate));
                }
            }
        }

        private readonly Window _reservationChangeView;
        private readonly RequestService _requestService;
        private readonly AccommodationReservationService _accommodationReservationService;
        #endregion

        public ReservationChangeViewModel(Window reservationChangeView, AccommodationReservation selectedReservation)
        {
            _reservationChangeView = reservationChangeView;
            _requestService = new RequestService();
            _accommodationReservationService = new AccommodationReservationService();
            SelectedReservation = selectedReservation;

            RequestDateChangeCommand = new RelayCommand(RequestDateChangeCommand_Execute, RequestDateChangeCommand_CanExecute);
            CancelCommand = new RelayCommand(CancelCommand_Execute);
        }

        #region COMMANDS
        public RelayCommand RequestDateChangeCommand { get; }
        public RelayCommand CancelCommand { get; }
       
        public void RequestDateChangeCommand_Execute(object? parameter)
        {
            _requestService.CreateRequest(SelectedStartDate, SelectedEndDate, SelectedReservation);
            _reservationChangeView.Close();
        }

        public bool RequestDateChangeCommand_CanExecute(object? parameter)
        {
            return DateTime.Compare(SelectedStartDate, SelectedEndDate) < 0
                && DateTime.Compare(DateTime.Now.Date, SelectedStartDate.Date) <= 0
                && (SelectedEndDate.Date - SelectedStartDate.Date).Days + 1 >= SelectedReservation.Accommodation.MinDaysForStay;
        }

        public void CancelCommand_Execute(object? parameter)
        {
            _reservationChangeView.Close();
        }
        #endregion
    }
}
