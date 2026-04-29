using Syncfusion.Windows.Controls.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using VetSystemModels.Dto.Appointment;
using VetSystemWpfDesktop.Services;

namespace VetSystemWpfDesktop.Pages
{
    public partial class AppointmentsPage : Page
    {
        private readonly AppointmentsService _appointmentsService;

        private List<AppointmentDto> _allAppointments = new();

        public AppointmentsPage()
        {
            InitializeComponent();

            var client = new HttpClient()
            {
                BaseAddress = new Uri("https://localhost:7146/api/")
            };

            _appointmentsService = new AppointmentsService(client);

            InitFilters();
            LoadClientsAsync();
        }

        public async void LoadClientsAsync()
        {
            _allAppointments = await _appointmentsService.GetAppointmentsAsync() ?? new();

            FillFilters();
            RefreshList();

            AllAppontmentsTextBlock.Text = _allAppointments.Count.ToString();

            NewAppontmentsTextBlock.Text = _allAppointments
                .Where(a => a.AppointmentDate.Month == DateTime.Today.Month &&
                            a.AppointmentDate.Year == DateTime.Today.Year)
                .Count()
                .ToString();
        }

        private void RefreshList()
        {
            AppointmentsListView.ItemsSource = ApplyFilters();
        }

        private List<AppointmentDto> ApplyFilters()
        {
            IEnumerable<AppointmentDto> query = _allAppointments.OrderBy(a => a.AppointmentDate);

            if (!string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                var search = SearchTextBox.Text.ToLower();

                query = query.Where(a =>
                    (a.ClientName?.ToLower().Contains(search) ?? false) ||
                    (a.PetName?.ToLower().Contains(search) ?? false) ||
                    (a.EmployeeName?.ToLower().Contains(search) ?? false) ||
                    (a.ServiceName?.ToLower().Contains(search) ?? false));
            }

            if (FromDatePicker.SelectedDate.HasValue)
            {
                var from = FromDatePicker.SelectedDate.Value;

                query = query.Where(a =>
                    a.AppointmentDate.ToDateTime(new TimeOnly(0, 0)) >= from);
            }

            if (ToDatePicker.SelectedDate.HasValue)
            {
                var to = ToDatePicker.SelectedDate.Value;

                query = query.Where(a =>
                    a.AppointmentDate.ToDateTime(new TimeOnly(0, 0)) <= to);
            }

            if (StatusComboBox.SelectedItem is string status && !string.IsNullOrEmpty(status))
            {
                query = query.Where(a => a.AppointmentStatusName == status);
            }

            if (DoctorComboBox.SelectedItem is string doctor && !string.IsNullOrEmpty(doctor))
            {
                query = query.Where(a => a.EmployeeName == doctor);
            }

            if (ClientComboBox.SelectedItem is string client && !string.IsNullOrEmpty(client))
            {
                query = query.Where(a => a.ClientName == client);
            }

            if (PetComboBox.SelectedItem is string pet && !string.IsNullOrEmpty(pet))
            {
                query = query.Where(a => a.PetName == pet);
            }

            return query.ToList();
        }

        private void InitFilters()
        {
            SearchTextBox.TextChanged += (s, e) => RefreshList();

            FromDatePicker.SelectedDateChanged += (s, e) => RefreshList();
            ToDatePicker.SelectedDateChanged += (s, e) => RefreshList();

            StatusComboBox.SelectionChanged += (s, e) => RefreshList();
            DoctorComboBox.SelectionChanged += (s, e) => RefreshList();
            ClientComboBox.SelectionChanged += (s, e) => RefreshList();
            PetComboBox.SelectionChanged += (s, e) => RefreshList();
        }

        private void FillFilters()
        {
            StatusComboBox.ItemsSource = _allAppointments
                .Select(x => x.AppointmentStatusName)
                .Distinct()
                .ToList();

            DoctorComboBox.ItemsSource = _allAppointments
                .Select(x => x.EmployeeName)
                .Distinct()
                .ToList();

            ClientComboBox.ItemsSource = _allAppointments
                .Select(x => x.ClientName)
                .Distinct()
                .ToList();

            PetComboBox.ItemsSource = _allAppointments
                .Select(x => x.PetName)
                .Distinct()
                .ToList();
        }

        private void AddAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AppointmentEditPage(null, "create", "appointmentsPage"));
        }

        private void EditAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is AppointmentDto appointment)
            {
                NavigationService.Navigate(new AppointmentEditPage(appointment, "edit", "appointmentsPage"));
            }
        }

        private void CheckAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is AppointmentDto appointment)
            {
                NavigationService.Navigate(new AppointmentEditPage(appointment, "check", "appointmentsPage"));
            }
        }

        private void DropFilterButton_Click(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Text = string.Empty;

            FromDatePicker.SelectedDate = null;
            ToDatePicker.SelectedDate = null;

            StatusComboBox.SelectedItem = null;
            DoctorComboBox.SelectedItem = null;
            ClientComboBox.SelectedItem = null;
            PetComboBox.SelectedItem = null;

            StatusComboBox.Text = string.Empty;
            DoctorComboBox.Text = string.Empty;
            ClientComboBox.Text = string.Empty;
            PetComboBox.Text = string.Empty;

            RefreshList();
        }

        private async void DeleteAppointemntButton_Click(object sender, RoutedEventArgs e)
        {
            var result = await DialogService.ShowConfirm("Удалить запись?");
            if (!result) return;

            if ((sender as Button)?.DataContext is not AppointmentDto appointment)
                return;

            try
            {
                await _appointmentsService.DeleteAppointmentAsync(appointment.AppointmentId);

                _allAppointments.RemoveAll(a => a.AppointmentId == appointment.AppointmentId);

                RefreshList();
            }
            catch (Exception ex)
            {
                await DialogService.ShowMessage($"Ошибка при удалении: {ex}");
            }
        }
    }
}