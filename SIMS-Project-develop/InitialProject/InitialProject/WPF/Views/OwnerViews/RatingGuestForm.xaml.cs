using InitialProject.Domain.Models;
using InitialProject.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InitialProject.WPF.Views
{
    /// <summary>
    /// Interaction logic for RatingGuestForm.xaml
    /// </summary>
    public partial class RatingGuestForm : Window
    {
        public User LogedInUser { get; set; }

        public AccommodationReservation SelectedReservation { get; set; }

        private int _theOneWhoIsRatedId;
        private int _raterId;
        private int _reservationId;

        private readonly RatingRepository _ratingRepository;
        private string _cleanliness;
        public string Cleanliness
        {
            get => _cleanliness;
            set
            {
                if (value != _cleanliness)
                {
                    _cleanliness = value;
                    OnPropertyChanged("Cleanliness");
                }
            }
        }

        private string _followingTheRules;
        public string FollowingTheRules
        {
            get => _followingTheRules;
            set
            {
                if (value != _followingTheRules)
                {
                    _followingTheRules = value;
                    OnPropertyChanged("FollowingTheRules");
                }
            }
        }

        private string _comment;
        public string Comment
        {
            get => _comment;
            set
            {
                if (value != _comment)
                {
                    _comment = value;
                    OnPropertyChanged("Comment");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public RatingGuestForm(RatingRepository ratingRepository, AccommodationReservation selectedReservation, int ownerId)
        {
            InitializeComponent();
            DataContext = this;
            _ratingRepository = ratingRepository;
            SelectedReservation = selectedReservation;
            _theOneWhoIsRatedId = SelectedReservation.GuestId;
            _reservationId = SelectedReservation.Id;
            _raterId = ownerId;
        }

        public bool IsValid
        {
            get
            {
                if (Cleanliness == null)
                {
                    return false;
                } 
                else if (FollowingTheRules == null)
                {
                    return false;
                }
                else if (Comment == null || Comment == "")
                {
                    return false;
                }

                return true;
            }
        }

        public void EnableButtonIfValid()
        {
            if (IsValid)
            {
                ButtonRate.IsEnabled = true;
            }
            else
            {
                ButtonRate.IsEnabled = false;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnableButtonIfValid();
        }

        private void TextBoxComment_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableButtonIfValid();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonRate_Click(object sender, RoutedEventArgs e)
        {
            _ratingRepository.Save(Cleanliness, FollowingTheRules, Comment, _theOneWhoIsRatedId, _raterId, _reservationId);
            this.Close();
        }
    }
}
