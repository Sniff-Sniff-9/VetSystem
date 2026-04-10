using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VetSystemModels.Dto.Appointment;
using VetSystemModels.Entities;
using VetSystemWpfDesktop.Services;
using VetSystemWpfDesktop.ViewModels;

namespace VetSystemWpfDesktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для DashboardPage.xaml
    /// </summary>
    public partial class DashboardPage : Page
    {
        public SnackbarMessageQueue MainSnackbarMessageQueue { get; } = new SnackbarMessageQueue(TimeSpan.FromSeconds(3));
        private readonly AppointmentsService _appointmentsService;
        private readonly EmployeesService _employeesService;
        public DashboardViewModel _viewModel;
        public DashboardPage()
        {
            InitializeComponent();
            var client = new HttpClient { BaseAddress = new Uri("https://localhost:7146/api/") };
            MainSnackbar.MessageQueue = MainSnackbarMessageQueue;
            _appointmentsService = new AppointmentsService(client);
            _employeesService = new EmployeesService(client);
            _viewModel = new DashboardViewModel(_appointmentsService, _employeesService);
            DataContext = _viewModel;
        }

        private async void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is AppointmentDto appointment)
            {
                var newAppointment = new CreateUpdateAppointmentDto
                {
                    EmployeeId = appointment.EmployeeId,
                    ServiceId = appointment.ServiceId,
                    PetId = appointment.PetId,
                    AppointmentDate = appointment.AppointmentDate,
                    AppointmentStatusId = 2,
                    StartTime = appointment.StartTime,
                };

                var updatedAppointment = await _appointmentsService.UpdateAppointmentAsync(appointment.AppointmentId, newAppointment);
                ShowSuccessMessage("Запись на прием подтверждена");
                await _viewModel.LoadClinicLabelsAsync();
            }
        }

        private async void RejectButton_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is AppointmentDto appointment)
            {
                var newAppointment = new CreateUpdateAppointmentDto
                {
                    EmployeeId = appointment.EmployeeId,
                    ServiceId = appointment.ServiceId,
                    PetId = appointment.PetId,
                    AppointmentDate = appointment.AppointmentDate,
                    AppointmentStatusId = 5,
                    StartTime = appointment.StartTime,
                };

                var updatedAppointment = await _appointmentsService.UpdateAppointmentAsync(appointment.AppointmentId, newAppointment);

                ShowSuccessMessage("Запись на прием отклонена");
                await _viewModel.LoadClinicLabelsAsync();
            }

        }

        private void ShowSuccessMessage(string message)
        {
            MainSnackbarMessageQueue.Enqueue(message);
        }

        private void ShowErrorMessage(string message)
        {
            MainSnackbarMessageQueue.Enqueue(message, "Закрыть", () => { }, true); 
        }

        private void CreateAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AppointmentEditPage(null, "create"));
        }

        private void CreateClientButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ClientEditPage());
        }

        private void CreatePetButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PetEditPage(null, "create"));
        }
    }
}
