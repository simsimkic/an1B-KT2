using InitialProject.Commands;
using InitialProject.WPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.WPF.ViewModels
{
    public class TourStatisticsSelectionViewModel : ViewModelBase
    {
        #region PROPERTIES
        private bool _isMostVisitedToursChecked;
        public bool IsMostVisitedToursChecked
        {
            get
            {
                return _isMostVisitedToursChecked;
            }
            set
            {
                if (_isMostVisitedToursChecked != value)
                {
                    _isMostVisitedToursChecked = value;
                    OnPropertyChanged(nameof(IsMostVisitedToursChecked));
                }
            }
        }

        private bool _isTourAttendanceChecked;
        public bool IsTourAttendanceChecked
        {
            get
            {
                return _isTourAttendanceChecked;
            }
            set
            {
                if (_isTourAttendanceChecked != value)
                {
                    _isTourAttendanceChecked = value;
                    OnPropertyChanged(nameof(IsTourAttendanceChecked));
                }
            }
        }

        private readonly TourStatisticsSelectionView _tourStatisticsSelectionView; 
        #endregion
        public TourStatisticsSelectionViewModel(TourStatisticsSelectionView tourStatisticsSelectionView)
        {
            _tourStatisticsSelectionView = tourStatisticsSelectionView;

            IsMostVisitedToursChecked = false;
            IsTourAttendanceChecked = false;

            OpenStatsCommand = new RelayCommand(OpenStatsCommand_Execute, OpenStatsCommand_CanExecute);
            CloseWindowCommand = new RelayCommand(CloseWindowCommand_Execute);
        }

        #region COMMANDS
        public RelayCommand OpenStatsCommand { get; }
        public RelayCommand CloseWindowCommand { get; }

        public void OpenStatsCommand_Execute(object? parameter)
        {

        }

        public bool OpenStatsCommand_CanExecute(object? parameter)
        {
            return IsMostVisitedToursChecked || IsTourAttendanceChecked;
        }

        public void CloseWindowCommand_Execute(object? parameter)
        {
            _tourStatisticsSelectionView.Close();
        } 
        #endregion
    }
}
