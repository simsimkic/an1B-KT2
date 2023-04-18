using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Models;
using InitialProject.WPF.Views;
using System.Collections.ObjectModel;
using System.Windows;

namespace InitialProject.WPF.ViewModels
{
    public class TourReviewsViewModel : ViewModelBase
    {
        #region PROPERTIES
        private Tour? _selectedTour;
        public Tour? SelectedTour
        {
            get
            {
                return _selectedTour;
            }
            set
            {
                if (_selectedTour != value)
                {
                    _selectedTour = value;
                    OnPropertyChanged(nameof(SelectedTour));
                }
            }
        }

        public ObservableCollection<Tour> PastTours { get; set; }

        private readonly Window _tourReviewsView;
        private readonly TourService _tourService;
        #endregion

        public TourReviewsViewModel(Window tourReviewsView)
        {
            _tourReviewsView = tourReviewsView;
            _tourService = new TourService();

            PastTours = new();

            OpenReviewListCommand = new RelayCommand(OpenReviewListCommand_Execute, OpenReviewListCommand_CanExecute);
            CloseWindowCommand = new RelayCommand(CloseWindowCommand_Execute);

            LoadPastTours();
        }

        public void LoadPastTours()
        {
            foreach (var tour in _tourService.GetPastTours())
            {
                PastTours.Add(tour);
            }
        }

        #region COMMANDS
        public RelayCommand OpenReviewListCommand { get; }
        public RelayCommand CloseWindowCommand { get; }

        public void OpenReviewListCommand_Execute(object? parameter)
        {
            var toursUserReviewsView = new ToursUserReviewsView(SelectedTour);
            toursUserReviewsView.Show();
        }

        public bool OpenReviewListCommand_CanExecute(object? parameter)
        {
            return SelectedTour is not null && SelectedTour.Status != TourStatus.CANCELED;
        }

        public void CloseWindowCommand_Execute(object? parameter)
        {
            _tourReviewsView.Close();
        }
        #endregion
    }
}
