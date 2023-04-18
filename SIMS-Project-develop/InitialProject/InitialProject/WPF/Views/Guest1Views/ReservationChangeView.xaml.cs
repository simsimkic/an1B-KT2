using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
using InitialProject.WPF.ViewModels;
using InitialProject.WPF.ViewModels.Guest1ViewModels;

namespace InitialProject.WPF.Views.Guest1Views
{
    /// <summary>
    /// Interaction logic for ReservationChangeView.xaml
    /// </summary>
    public partial class ReservationChangeView : Window
    {
        public ReservationChangeView(AccommodationReservation selectedReservation)
        {
            InitializeComponent();
            this.DataContext = new ReservationChangeViewModel(this, selectedReservation);
        }
    }
}
