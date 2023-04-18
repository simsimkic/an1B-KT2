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
    public class TourReviewDetailsViewModel : ViewModelBase
    {
        #region PROPERTIES
        public TourReview Review { get; set; }

        public string UserInfo { get; set; }
        public string GuideKnowledgeGrade { get; set; }
        public string GuideLanguageGrade { get; set; }
        public string InterestingnessGrade { get; set; }
        public string Comment { get; set; }

        private readonly Window _tourReviewDetailsView;
        #endregion

        public TourReviewDetailsViewModel(Window tourReviewDetailsView, TourReview review)
        {
            _tourReviewDetailsView = tourReviewDetailsView;

            Review = review;

            UserInfo = $"User \"{Review.Arrival.Reservation.User.Username}\" arrived at checkpoint \"{Review.Arrival.Checkpoint.Name}\"";
            GuideKnowledgeGrade = $"{Review.GuideKnowledgeGrade}";
            GuideLanguageGrade = $"{Review.GuideLanguageGrade}";
            InterestingnessGrade = $"{Review.InterestingnessGrade}";
            Comment = $"{Review.Comment}";

            CloseWindowCommand = new RelayCommand(CloseWindowCommand_Execute);
        }

        #region COMMANDS
        public RelayCommand CloseWindowCommand { get; }

        public void CloseWindowCommand_Execute(object? parameter)
        {
            _tourReviewDetailsView.Close();
        }
        #endregion
    }
}
