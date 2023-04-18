using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.DTOs;
using InitialProject.Domain.Models;
using InitialProject.WPF.Views.Guest2Views;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace InitialProject.WPF.ViewModels.Guest2ViewModels
{
    public class RateTourAndGuideFormViewModel : ViewModelBase
    {
        #region PROPERTIES
        public User LoggedUser { get; set; }
        public int TourId { get; set; }
        public int GuideId { get; set; }

        private string guidesKnowledgeGrade;
        public string GuidesKnowledgeGrade
        {
            get
            {
                return guidesKnowledgeGrade;
            }
            set
            {
                if (guidesKnowledgeGrade != value)
                {
                    guidesKnowledgeGrade = value;
                    OnPropertyChanged(nameof(GuidesKnowledgeGrade));
                }
            }
        }

        private string guidesLanguageGrade;
        public string GuidesLanguageGrade
        {
            get
            {
                return guidesLanguageGrade;
            }
            set
            {
                if (guidesLanguageGrade != value)
                {
                    guidesLanguageGrade = value;
                    OnPropertyChanged(nameof(GuidesLanguageGrade));
                }
            }
        }

        private string interestingGrade;
        public string InterestingGrade
        {
            get
            {
                return interestingGrade;
            }
            set
            {
                if (interestingGrade != value)
                {
                    interestingGrade = value;
                    OnPropertyChanged(nameof(InterestingGrade));
                }
            }
        }

        private string additionalComment;
        public string AdditionalComment
        {
            get
            {
                return additionalComment;
            }
            set
            {
                if (additionalComment != value)
                {
                    additionalComment = value;
                    OnPropertyChanged(nameof(AdditionalComment));
                }
            }
        }

        private string imageUrl;
        public string ImageUrl
        {
            get
            {
                return imageUrl;
            }
            set
            {
                if (imageUrl != value)
                {
                    imageUrl = value;
                    OnPropertyChanged(nameof(ImageUrl));
                }
            }
        }

        private bool _nextButtonisEnabled;
        public bool NextButtonIsEnabled
        {
            get
            {
                return _nextButtonisEnabled;
            }
            set
            {
                if (_nextButtonisEnabled != value)
                {
                    _nextButtonisEnabled = value;
                    OnPropertyChanged(nameof(NextButtonIsEnabled));
                }
            }
        }

        private bool _previousButtonIsEnabled;
        public bool PreviousButtonIsEnabled
        {
            get
            {
                return _previousButtonIsEnabled;
            }
            set
            {
                if (_previousButtonIsEnabled != value)
                {
                    _previousButtonIsEnabled = value;
                    OnPropertyChanged(nameof(PreviousButtonIsEnabled));
                }
            }
        }

        private bool _removeImageIsEnabled;
        public bool RemoveImageIsEnabled
        {
            get
            {
                return _removeImageIsEnabled;
            }
            set
            {
                if (_removeImageIsEnabled != value)
                {
                    _removeImageIsEnabled = value;
                    OnPropertyChanged(nameof(RemoveImageIsEnabled));
                }
            }
        }

        private BitmapImage _selectedImage;
        public BitmapImage SelectedImage
        {
            get => _selectedImage;
            set
            {
                if (_selectedImage != value)
                {
                    _selectedImage = value;
                    OnPropertyChanged(nameof(SelectedImage));
                }
            }
        }

        private BitmapImage _previouslySelectedImage;
        public BitmapImage PreviouslySelectedImage
        {
            get => _previouslySelectedImage;
            set
            {
                if (_previouslySelectedImage != value)
                {
                    _previouslySelectedImage = value;
                    OnPropertyChanged(nameof(PreviouslySelectedImage));
                }
            }
        }

        public List<BitmapImage> Images { get; set; }
        private readonly Window _rateTourAndGuideForm;
        private readonly TourService _tourService;
        private readonly TourReviewService _tourReviewService;

        #endregion
        public RateTourAndGuideFormViewModel(Window rateTourAndGuideForm, int tourId, User user)
        {
            _rateTourAndGuideForm = rateTourAndGuideForm;
            _tourService = new TourService();
            _tourReviewService = new TourReviewService();
            LoggedUser = user;
            TourId = tourId;
            Images = new List<BitmapImage>();

            RateTourAndGuideCommand = new RelayCommand(RateTourAndGuideCommand_Execute);
            CancelRatingCommand = new RelayCommand(CancelRatingCommand_Execute);
            AddImageCommand = new RelayCommand(AddImageCommand_Execute);
            NextImageCommand = new RelayCommand(NextImageCommand_Execute);
            PreviousImageCommand = new RelayCommand(PreviousImageCommand_Execute);
            RemoveImageCommand = new RelayCommand(RemoveImageCommand_Execute);

        }

        public bool IsEligibleForRating()
        {
            if (GuidesKnowledgeGrade == null || GuidesLanguageGrade == null || InterestingGrade == null || string.IsNullOrEmpty(AdditionalComment))
            {
                return false;
            }
            return true;
        }

        public void SetGuideIdProperty()
        {
            GuideId = _tourService.GetGuideId(TourId);
        }

        private void AddImage(string fileName)
        {
            Uri uri = new Uri(fileName);
            if (SelectedImage == null)
            {
                SelectedImage = new BitmapImage(uri);
                Images.Add(SelectedImage);
                ImageUrl = SelectedImage.ToString();
            }
            else
            {
                var currentIndex = GetImageIndex();
                SelectedImage = new BitmapImage(uri);
                var isLastImage = currentIndex + 1 == Images.Count;
                if (!isLastImage)
                {
                    NextButtonIsEnabled = true;
                }
                PreviousButtonIsEnabled = true;
                Images.Insert(currentIndex + 1, SelectedImage);
                ImageUrl = SelectedImage.ToString();
            }
            RemoveImageIsEnabled = true;
        }

        private int GetImageIndex()
        {
            return Images.IndexOf(SelectedImage);
        }

        public void ShowUrlAfterRemovingImage()
        {
            if (Images.Count - 2 == 0)
            {
                ImageUrl = Images[0].ToString();
            }
            else if (Images.Count - 2 < 0)
            {
                ImageUrl = "";
            }
            else
            {
                ImageUrl = Images[Images.Count - 2].ToString();
            }
        }

        #region COMMANDS

        public RelayCommand RateTourAndGuideCommand { get; }
        public RelayCommand CancelRatingCommand { get; }
        public RelayCommand AddImageCommand { get; }
        public RelayCommand NextImageCommand { get; }
        public RelayCommand PreviousImageCommand { get; }
        public RelayCommand RemoveImageCommand { get; }
        public void RemoveImageCommand_Execute(object? parameter)
        {
            var currentIndex = GetImageIndex();
            var isLastImage = currentIndex + 1 == Images.Count;
            var isSecondToLastImage = currentIndex + 2 == Images.Count;

            ShowUrlAfterRemovingImage();
            Images.Remove(SelectedImage);

            if (isLastImage)
            {
                if (Images.Count == 0)
                {
                    SelectedImage = null;
                    RemoveImageIsEnabled = false;
                    return;
                }

                SelectedImage = Images[currentIndex - 1];

                if (Images.Count == 1)
                {
                    NextButtonIsEnabled = false;
                    PreviousButtonIsEnabled = false;
                }
            }
            else
            {
                SelectedImage = Images[currentIndex];

                if (isSecondToLastImage)
                {
                    NextButtonIsEnabled = false;
                }
            }
        }
        public void PreviousImageCommand_Execute(object? parameter)
        {
            var currentIndex = GetImageIndex();
            var isSecondImage = currentIndex == 1;
            if (isSecondImage)
            {
                PreviousButtonIsEnabled = false;
            }
            NextButtonIsEnabled = true;

            SelectedImage = Images[currentIndex - 1];
            ImageUrl = SelectedImage.ToString();
        }
        public void NextImageCommand_Execute(object? parameter)
        {
            var currentIndex = GetImageIndex();
            var isSecondToLastImage = currentIndex == Images.Count - 2;

            if (isSecondToLastImage)
            {
                NextButtonIsEnabled= false;
            }
            PreviousButtonIsEnabled= true;

            SelectedImage = Images[currentIndex + 1];
            ImageUrl = SelectedImage.ToString();
        }

        public void AddImageCommand_Execute(object? parameter)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.Filter = "JPG Files (*.jpg)|*.jpg|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|GIF Files (*.gif)|*.gif";

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                AddImage(dlg.FileName);
            }
        }
        public void CancelRatingCommand_Execute(object? parameter)
        {
            ReservedToursView reservedToursView = new ReservedToursView(LoggedUser);
            reservedToursView.Show();
            _rateTourAndGuideForm.Close();
        }
        public void RateTourAndGuideCommand_Execute(object? parameter)
        {
            if (!IsEligibleForRating())
            {
                MessageBox.Show("You have to fill every field and combo box!");    
            }
            else
            {
                SetGuideIdProperty();
                _tourReviewService.SaveRating(LoggedUser.Id, TourId, GuideId, int.Parse(GuidesKnowledgeGrade), int.Parse(GuidesLanguageGrade), int.Parse(InterestingGrade), AdditionalComment, Images);
                _rateTourAndGuideForm.Close();
            }
        }

        #endregion
    }
}
