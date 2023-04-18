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
using System.Windows.Media.Imaging;

namespace InitialProject.WPF.ViewModels
{
    public class MostVisitedTourViewModel : ViewModelBase
    {
		#region PROPERTIES
		private bool _isAllTimeRBChecked;
		public bool IsAllTimeRBChecked
		{
			get
			{
				return _isAllTimeRBChecked;
			}
			set
			{
				if (_isAllTimeRBChecked != value)
				{
					_isAllTimeRBChecked = value;
					OnPropertyChanged(nameof(IsAllTimeRBChecked));
					LoadData();
				}
			}
		}

		private bool _isYearlyRBChecked;
		public bool IsYearlyRBChecked
		{
			get
			{
				return _isYearlyRBChecked;
			}
			set
			{
				if (_isYearlyRBChecked != value)
				{
					_isYearlyRBChecked = value;
					OnPropertyChanged(nameof(IsYearlyRBChecked));
				}
			}
		}

		private BitmapImage _selectedImage;
		public BitmapImage SelectedImage
		{
			get
			{
				return _selectedImage;
			}
			set
			{
				if (_selectedImage != value)
				{
					_selectedImage = value;
					OnPropertyChanged(nameof(SelectedImage));
				}
			}
		}

		private Tour _displayedTour;
		public Tour DisplayedTour
		{
			get
			{
				return _displayedTour;
			}
			set
			{
				if (_displayedTour != value)
				{
					_displayedTour = value;
					OnPropertyChanged(nameof(DisplayedTour));
				}
			}
		}

		private int _selectedYear;
		public int SelectedYear
		{
			get
			{
				return _selectedYear;
			}
			set
			{
				if (_selectedYear != value)
				{
					_selectedYear = value;
					OnPropertyChanged(nameof(SelectedYear));
					LoadData();
				}
			}
		}

		public ObservableCollection<DateTime> StartTimes { get; set; }
		public ObservableCollection<Checkpoint> Checkpoints { get; set; }
		public ObservableCollection<BitmapImage> Images { get; set; }
		public ObservableCollection<int> PossibleYears { get; set; }

		private readonly Window _mostVisitedToursView;
		private readonly MostVisitedTourService _mostVisitedTourService;
		private readonly CheckpointService _checkpointService;
		private readonly TourService _tourService;
		#endregion

		public MostVisitedTourViewModel(Window mostVisitedToursView)
        {
            _mostVisitedToursView = mostVisitedToursView;

            _mostVisitedTourService = new MostVisitedTourService();
            _checkpointService = new CheckpointService();
			_tourService = new TourService();


            PossibleYears = new ObservableCollection<int>(_mostVisitedTourService.GetYearsThatHaveTours());
            Checkpoints = new ObservableCollection<Checkpoint>();
			StartTimes = new ObservableCollection<DateTime>();


            IsAllTimeRBChecked = true;
            IsYearlyRBChecked = false;

            LoadData();

			OpenStatsCommand = new RelayCommand(OpenStatsCommand_Execute);
            CloseWindowCommand = new RelayCommand(CloseWindowCommand_Execute);
        }

        private void LoadData()
        {
            LoadDisplayedTour();
            LoadCheckpoints();
			LoadStartTimes();
        }

        private void LoadStartTimes()
        {
			StartTimes.Clear();
			foreach (var startTime in _tourService.GetAllStartTimesForTour(DisplayedTour))
			{
				StartTimes.Add(startTime);
			}
        }

        private void LoadDisplayedTour()
		{
			if (IsAllTimeRBChecked)
			{
				DisplayedTour = _mostVisitedTourService.GetAllTimeMostVisitedTour();
			}
			else
			{
				DisplayedTour = _mostVisitedTourService.GetMostVisitedTourByYear(SelectedYear);
			}
		}

		private void LoadCheckpoints()
		{
			Checkpoints.Clear();
			foreach (var checkpoint in _checkpointService.GetAllByTour(DisplayedTour))
			{
				Checkpoints.Add(checkpoint);
			}
		}

		#region COMMANDS
		public RelayCommand OpenStatsCommand { get; }
		public RelayCommand CloseWindowCommand { get; }

		public void OpenStatsCommand_Execute(object? parameter)
		{
			var tourStatisticsView = new TourStatisticsView(DisplayedTour);
			tourStatisticsView.Show();
		}

		public void CloseWindowCommand_Execute(object? parameter)
		{
			_mostVisitedToursView.Close();
		} 
		#endregion
	}
}
