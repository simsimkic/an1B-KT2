using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.WPF.ViewModels.OwnerViewModels
{
    public class RequestDeclinedFormViewModel : ViewModelBase
    {
        #region PROPERTIES
        private Request _selectedRequest;
        public Request SelectedRequest
        {
            get
            {
                return _selectedRequest;
            }
            set
            {
                if (value != _selectedRequest)
                {
                    _selectedRequest = value;
                    OnPropertyChanged(nameof(SelectedRequest));
                }
            }
        }

        private readonly Window _requestDeclinedForm;
        private readonly ManageRequestService _manageRequestService;
        #endregion

        public RequestDeclinedFormViewModel(Window requestDeclinedForm, Request selectedRequest)
        {
            _requestDeclinedForm = requestDeclinedForm;

            SelectedRequest = selectedRequest;
            _manageRequestService = new ManageRequestService();

            CloseWindowCommand = new RelayCommand(CloseWindowCommand_Execute);
            ConfirmDeclineCommand = new RelayCommand(ConfirmDeclineCommand_Execute);
        }

        #region COMMANDS
        public RelayCommand CloseWindowCommand { get; }
        public RelayCommand ConfirmDeclineCommand { get; }

        public void ConfirmDeclineCommand_Execute(object? parameter)
        {
            _manageRequestService.DeclineRequest(SelectedRequest);
            _requestDeclinedForm.Close();
        }
        public void CloseWindowCommand_Execute(object? parameter)
        {
            _requestDeclinedForm.Close();
        }
        #endregion
    }
}
