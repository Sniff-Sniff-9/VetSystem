using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
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
using VetSystemModels.Dto.Client;
using VetSystemModels.Dto.Employee;
using VetSystemModels.Dto.Pet;
using VetSystemModels.Dto.Service;
using VetSystemModels.Entities;
using VetSystemWpfDesktop.Services;
using VetSystemModels.Dto.AppointmentService;

namespace VetSystemWpfDesktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для AppointmentEditPage.xaml
    /// </summary>
    public partial class AppointmentEditPage : Page
    {
        private ClientsService _clientsService;
        private PetsService _petsService;
        private ServicesService _servicesService;
        private AppointmentsService _appointmentsService;
        private EmployeesService _employeesService;
        private List<AppointmentServiceDto> _appointmentServices = new();
        private TimeOnly _selectedSlot;
        private AppointmentDto? _appointment;
        private string _mode;



        public AppointmentEditPage(AppointmentDto? appointment, string mode)
        {
            InitializeComponent();

            var client = new HttpClient()
            {
                BaseAddress = new Uri("https://localhost:7146/api/")
            };

            _clientsService = new ClientsService(client);
            _petsService = new PetsService(client);
            _servicesService = new ServicesService(client);
            _appointmentsService = new AppointmentsService(client);
            _employeesService = new EmployeesService(client);
            _mode = mode;
            _appointment = appointment;

            _ = LoadClientsAsync();
            _ = LoadServicesAsync();
            if (_appointment != null)
            {
                _ = LoadAdditionalServicesAsync(_appointment.AppointmentId);
                _ = LoadAppointmentDataForEditAsync();
            }
            if (_mode == "check")
            {
              
            }
        }

        private async Task LoadServicesAsync()
        {
            var services = await _servicesService.GetServicesAsync() ?? new();
            ServicesListBox.ItemsSource = services;
            if (_appointment != null)
            {
                ServicesListBox.SelectedItem = services.FirstOrDefault(c => c.ServiceId == _appointment.ServiceId);
            }
        }

        private async Task LoadAdditionalServicesAsync(int id)
        {
            _appointmentServices = await _appointmentsService.GetServicesByAppointmentIdAsync(id) ?? new();
            AdditionalServicesListView.ItemsSource = _appointmentServices.OrderByDescending(aps => aps.IsMain);
            if (_appointmentServices != null)
            {
                TotalPriceText.Text = $"{_appointmentServices.Sum(s => s.PriceAtMoment).ToString()} ₽";
            }
            
        }

        private async Task LoadClientsAsync()
        {
            var clients = await _clientsService.GetClientsAsync() ?? new();
            ClientsListBox.ItemsSource = clients;
            if (_appointment != null)
            {
                ClientsListBox.SelectedItem = clients.FirstOrDefault(c => c.ClientId == _appointment.ClientId);
            }
        }

        private async Task LoadEmployeesAsync(ServiceDto serviceDto)
        {
            var employees = await _servicesService.GetEmployeesByServiceIdAsync(serviceDto.ServiceId) ?? new();
            DoctorsListBox.ItemsSource = employees;
            if (_appointment != null)
            {
                DoctorsListBox.SelectedItem = employees.FirstOrDefault(c => c.EmployeeId == _appointment.EmployeeId);
            }
        }

        private async Task LoadPetsAsync(ClientDto clientDto)
        {
            var pets = await _clientsService.GetPetsByClientIdAsync(clientDto.ClientId) ?? new();
            PetsListBox.ItemsSource = pets;
            if (_appointment != null)
            {
                PetsListBox.SelectedItem = pets.FirstOrDefault(c => c.PetId == _appointment.PetId);
            }
        }

        private async Task ShowAvailableSlots()
        {
            SelectedSlotText.Text = "";

            var employee = DoctorsListBox.SelectedItem as EmployeeDto;
            if (employee == null)
            {
                SelectedSlotText.Text = "Выберите специалиста.";
                SlotsPanel.Children.Clear();
                return;
            } 

            var date = DatePicker.SelectedDate;
            if (date == null)
            {
                SelectedSlotText.Text = "Выберите дату.";
                return;
            }

            var slots = await _appointmentsService.GetAvailableSlotsAsync(
                employee.EmployeeId,
                DateOnly.FromDateTime(date.Value));
            if (slots == null) return;

            if (_appointment != null && !slots.Contains(_appointment.StartTime))
            {
                slots.Add(_appointment.StartTime);
            }

            SlotsPanel.Children.Clear();

            if (slots == null || !slots.Any())
            {
                SelectedSlotText.Text = "Нет доступных слотов.";
                return;
            }
            

            foreach (var slot in slots.Order())
            {
                var btn = new Button
                {
                    Content = slot.ToString("HH:mm"),
                    Tag = slot,
                    Background = Brushes.LightGray,
                    Foreground = Brushes.Black,
                    Margin = new Thickness(4),
                    Padding = new Thickness(12, 6, 12, 6),
                    BorderBrush = Brushes.LightGray
                };

                btn.Click += SlotButton_Click;

                if (_appointment != null && slot == _appointment.StartTime)
                {
                    _selectedSlot = slot;

                    btn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#03a9f4"));
                    btn.Foreground = Brushes.White;
                    btn.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#03a9f4"));
                }

                SlotsPanel.Children.Add(btn);
            }
        }

        private void SlotButton_Click(object sender, RoutedEventArgs e)
        {
    
            foreach (var child in SlotsPanel.Children.OfType<Button>())
            {
                child.Background = Brushes.LightGray;
                child.Foreground = Brushes.Black;
                child.BorderBrush = Brushes.LightGray;
            }
   
            var btn = sender as Button;
            if (btn == null) return;

            _selectedSlot = (TimeOnly)btn.Tag;
            btn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#03a9f4"));
            btn.Foreground = Brushes.White;
            btn.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#03a9f4"));

        }

        private async Task LoadAppointmentDataForEditAsync()
        {
            if (_appointment == null) return;

            foreach (var rb in StatusButtonStackPanel.Children.OfType<RadioButton>())
            {
                if (rb.Tag != null && int.Parse(rb.Tag.ToString() ?? "0") == _appointment.AppointmentStatusId)
                {
                    rb.IsChecked = true;
                    break;
                }
            }

            DatePicker.SelectedDate = _appointment.AppointmentDate.ToDateTime(new TimeOnly(0, 0, 0));            

            await ShowAvailableSlots();
        }


        private void ClientSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
          
        }

        private async void ClientsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PetCard.IsEnabled = true;
            await LoadPetsAsync(ClientsListBox.SelectedItem as ClientDto ?? new());
        }

        private async void AddServiceButton_Click(object sender, RoutedEventArgs e)
        {
            var doctor = DoctorsListBox.SelectedItem as EmployeeDto;

            if (doctor == null)
            {
                MessageBox.Show("Выберите специалиста.");
                return;
            }

            var services = await _employeesService.GetServicesByEmployeeIdAsync(doctor.EmployeeId);

            var window = new AddAdditionalServicesWindow(services ?? new());

            if (window.ShowDialog() == true)
            {
                var service = window.SelectedService ?? new();

                if (service == null) return;

                if (services != null) services.Remove(service);

                var appointmentService = new AppointmentServiceDto
                {
                    ServiceId = service.ServiceId,
                    ServiceName = service.ServiceName,
                    PriceAtMoment = service.Price,
                    IsMain = false
                };

                _appointmentServices.Add(appointmentService);

                AdditionalServicesListView.ItemsSource = null;
                AdditionalServicesListView.ItemsSource = _appointmentServices;

                TotalPriceText.Text = $"{_appointmentServices.Sum(s => s.PriceAtMoment)} ₽";
            }
        }

        private void RemoveServiceButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AppointmentsPage());
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var date = DatePicker.SelectedDate
        ?? throw new InvalidOperationException("Не выбрана дата");

            var doctor = DoctorsListBox.SelectedItem as EmployeeDto
                ?? throw new InvalidOperationException("Не выбран врач");

            var pet = PetsListBox.SelectedItem as PetDto
                ?? throw new InvalidOperationException("Не выбран питомец");

            var service = ServicesListBox.SelectedItem as ServiceDto
                ?? throw new InvalidOperationException("Не выбрана услуга");

            var selectedStatus = StatusButtonStackPanel.Children
    .OfType<RadioButton>()
    .FirstOrDefault(r => r.IsChecked == true)
    ?? throw new InvalidOperationException("Не выбран статус");

            var statusId = Convert.ToInt32(selectedStatus.Tag);

            var appointment = await _appointmentsService.CreateAppointmentAsync(
                DateOnly.FromDateTime(date),
                doctor,
                service,
                pet,
                _selectedSlot, int.Parse(selectedStatus.Tag.ToString() ?? "0")
            );

            foreach (var s in _appointmentServices)
            {
                if (s.IsMain == true)
                {
                    continue;
                }

                var tempAppointmentService = new CreateAppointmentServiceDto
                {
                    AppointmentId = appointment.AppointmentId,
                    ServiceId = s.ServiceId,
                    IsMain = false
                };
                await _appointmentsService.CreateAppointmentServiceAsync(tempAppointmentService);
            }

            var updateAppointment = new CreateUpdateAppointmentDto
            {
                AppointmentDate = appointment.AppointmentDate,
                AppointmentStatusId = appointment.AppointmentStatusId,
                EmployeeId = appointment.EmployeeId,
                PetId = appointment.PetId,
                ServiceId = appointment.ServiceId,
                StartTime = appointment.StartTime,
            };

            await _appointmentsService.UpdateAppointmentAsync(appointment.AppointmentId, updateAppointment);

            NavigationService.Navigate(new AppointmentsPage());
        }

        private async void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            await ShowAvailableSlots();
        }

        private void DoctorSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private async void DoctorsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await ShowAvailableSlots();
        }

        private async void ServicesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DoctorCard.IsEnabled = true;
            var selectedService = ServicesListBox.SelectedItem as ServiceDto;
            if (selectedService == null) return;

            var appointmentService = new AppointmentServiceDto
            {
                ServiceId = selectedService.ServiceId,
                ServiceName = selectedService.ServiceName,
                PriceAtMoment = selectedService.Price,
                IsMain = true
            };

            if (_appointmentServices.Count == 0)
            {
                _appointmentServices.Add(appointmentService);
            }
            else
            {
                _appointmentServices[0] = appointmentService;
            }

            AdditionalServicesListView.ItemsSource = null;
            AdditionalServicesListView.ItemsSource = _appointmentServices;
            TotalPriceText.Text = $"{_appointmentServices.Sum(s => s.PriceAtMoment).ToString()} ₽";

            await LoadEmployeesAsync(ServicesListBox.SelectedItem as ServiceDto ?? new());
        }

        private void ServiceSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private  void PetsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void PetSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
