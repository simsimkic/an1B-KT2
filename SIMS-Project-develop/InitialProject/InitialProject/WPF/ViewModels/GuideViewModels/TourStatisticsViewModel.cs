using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Models;
using InitialProject.WPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.WPF.ViewModels
{
    public class TourStatisticsViewModel : ViewModelBase
    {
        private int? _youngerThanEighteen;
        public int? YoungerThanEighteen
        {
            get
            {
                return _youngerThanEighteen;
            }
            set
            {
                if (_youngerThanEighteen != value)
                {
                    _youngerThanEighteen = value;
                    OnPropertyChanged(nameof(YoungerThanEighteen));
                }
            }
        }

        private int? _eighteenToFifty;
        public int? EighteenToFifty
        {
            get
            {
                return _eighteenToFifty;
            }
            set
            {
                if (_eighteenToFifty != value)
                {
                    _eighteenToFifty = value;
                    OnPropertyChanged(nameof(EighteenToFifty));
                }
            }
        }

        private int? _olderThanFifty;
        public int? OlderThanFifty
        {
            get
            {
                return _olderThanFifty;
            }
            set
            {
                if (_olderThanFifty != value)
                {
                    _olderThanFifty = value;
                    OnPropertyChanged(nameof(OlderThanFifty));
                }
            }
        }

        private readonly Window _tourStatisticsView;
        private readonly TourStatisticsService _tourStatisticsService;

        public TourStatisticsViewModel(Window tourStatisticsView, Tour tour)
        {
            _tourStatisticsView = tourStatisticsView;

            _tourStatisticsService = new TourStatisticsService();

            YoungerThanEighteen = _tourStatisticsService.GetNumberOfGusetsInAgeRange(tour, 0, 18);
            EighteenToFifty = _tourStatisticsService.GetNumberOfGusetsInAgeRange(tour, 18, 50);
            OlderThanFifty = _tourStatisticsService.GetNumberOfGusetsInAgeRange(tour, 18, 200);

            CloseWindowCommand = new RelayCommand(CloseWindowCommand_Execute);
        }

        public RelayCommand CloseWindowCommand { get; }

        public void CloseWindowCommand_Execute(object? parameter)
        {
            _tourStatisticsView.Close();
        }
    }
}
