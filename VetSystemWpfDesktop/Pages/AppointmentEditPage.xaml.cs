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

        private List<ClientDto> _clients = new();
        private List<ServiceDto> _services = new();
        private List<EmployeeDto> _employees = new();
        private List<PetDto> _pets = new();

        private bool _isPageInitializing = true;
        private bool _isUpdatingSelection;

        private ServiceDto? _previousSelectedService;
        private EmployeeDto? _initialEmployee;
        private List<AppointmentServiceDto> _initialAppointmentServices = new();

        private TimeOnly _selectedSlot;

        private AppointmentDto? _appointment;
        private string _mode;
        private string _initialPage;

        public AppointmentEditPage(AppointmentDto? appointment, string mode, string initialPage)
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
            _initialPage = initialPage;
            _appointment = appointment;

            _ = InitializeAsync(); 

            if (_mode == "check")
            {
                SaveButton.Visibility = Visibility.Collapsed;
                AdditionalServicesListView.IsHitTestVisible = false;
                AddServiceButton.Visibility = Visibility.Collapsed;
                DateSlotCard.IsHitTestVisible = false;
                ClientCard.IsHitTestVisible = false;
                DoctorCard.IsHitTestVisible = false;
                PetCard.IsHitTestVisible = false;
                ServiceCard.IsHitTestVisible = false;
            }

        }

        private async Task LoadServicesAsync()
        {
            _services = await _servicesService.GetServicesAsync() ?? new();
            ServicesListBox.ItemsSource = _services;

            if (_appointment != null)
            {
                ServicesListBox.SelectedItem =
                    _services.FirstOrDefault(c => c.ServiceId == _appointment.ServiceId);
            }
        }

        private async Task LoadAdditionalServicesAsync(int id)
        {
            _appointmentServices = await _appointmentsService.GetServicesByAppointmentIdAsync(id) ?? new();

            _initialAppointmentServices = _appointmentServices
                .Select(s => new AppointmentServiceDto
                {
                    AppointmentServiceId = s.AppointmentServiceId,
                    ServiceId = s.ServiceId,
                    ServiceName = s.ServiceName,
                    PriceAtMoment = s.PriceAtMoment,
                    IsMain = s.IsMain
                })
                .ToList();

            AdditionalServicesListView.ItemsSource = _appointmentServices.OrderByDescending(aps => aps.IsMain);

            TotalPriceText.Text = $"{_appointmentServices.Sum(s => s.PriceAtMoment)} ₽";
        }

        private async Task LoadClientsAsync()
        {
            _clients = await _clientsService.GetClientsAsync() ?? new();
            ClientsListBox.ItemsSource = _clients;

            if (_appointment != null)
            {
                ClientsListBox.SelectedItem =
                    _clients.FirstOrDefault(c => c.ClientId == _appointment.ClientId);
            }

        }

        private async Task LoadEmployeesAsync(ServiceDto serviceDto)
        {
            _employees = await _servicesService
                .GetEmployeesByServiceIdAsync(serviceDto.ServiceId) ?? new();

            if (_appointment != null)
            {
                var employeeFromAppointment = _employees
                    .FirstOrDefault(e => e.EmployeeId == _appointment.EmployeeId);

                if (employeeFromAppointment == null && _initialEmployee == null)
                {
                    var _oldEmployee =
                        await _employeesService.GetClientAsync(_appointment.EmployeeId)
                        ?? new EmployeeDto();

                    _employees.Add(_oldEmployee);
                }
            }

            DoctorsListBox.ItemsSource = _employees;

            if (_appointment != null)
            {
                DoctorsListBox.SelectedItem = _employees
                    .FirstOrDefault(e => e.EmployeeId == _appointment.EmployeeId);

                if (_isPageInitializing)
                {
                    _initialEmployee = DoctorsListBox.SelectedItem as EmployeeDto;
                }
            }
        }

        private async Task LoadPetsAsync(ClientDto clientDto)
        {
            _pets = await _clientsService
        .GetPetsByClientIdAsync(clientDto.ClientId) ?? new();

            PetsListBox.ItemsSource = _pets;

            if (_appointment != null)
            {
                PetsListBox.SelectedItem =
                    _pets.FirstOrDefault(c => c.PetId == _appointment.PetId);
            }
        }

        private async Task ShowAvailableSlots()
        {
            SelectedSlotText.Text = "";

            var employee = DoctorsListBox.SelectedItem as EmployeeDto;

            if (employee == null || !_employees.Any(e => e.EmployeeId == employee.EmployeeId))
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

            var employeeId = employee.EmployeeId;

            var slots = await _appointmentsService.GetAvailableSlotsAsync(
                employeeId,
                DateOnly.FromDateTime(date.Value));

            var currentEmployee = DoctorsListBox.SelectedItem as EmployeeDto;

            if (currentEmployee == null || currentEmployee.EmployeeId != employeeId)
            {
                return; // просто выходим — не трогаем UI
            }

            if (slots == null)
                return;

            if (_appointment != null && employeeId == _appointment.EmployeeId)
            {
                if (!slots.Contains(_appointment.StartTime))
                {
                    slots.Add(_appointment.StartTime);
                }
            }

            if (_appointment != null && _selectedSlot == default && slots.Contains(_appointment.StartTime))
            {
                _selectedSlot = _appointment.StartTime;
            }

            SlotsPanel.Children.Clear();

            if (!slots.Any())
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

                if (slot == _selectedSlot)
                {
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
            ApplySearch(
                  ClientSearchTextBox,
                  ClientsListBox,
                  _clients,
                  x => $"{x.LastName} {x.FirstName} {x.MiddleName}"
              );
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
                await DialogService.ShowMessage("Выберите специалиста.");
                return;
            }

            var services = await _employeesService.GetServicesByEmployeeIdAsync(doctor.EmployeeId);

            var availableServices = services?.Where(s => !_appointmentServices.Any(a => a.ServiceId == s.ServiceId)).ToList();

            var window = new AddAdditionalServicesWindow(availableServices ?? new());

            if (window.ShowDialog() == true)
            {
                var service = window.SelectedService ?? new();

                if (service == null) return;

                if (services != null) services.Remove(service);

                if (_appointmentServices.Any(s => s.ServiceId == service.ServiceId))
                {
                    await DialogService.ShowMessage("Услуга уже добавлена.");
                    return;
                }

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
            if ((sender as Button)?.DataContext is AppointmentServiceDto service)
            {
                _appointmentServices.Remove(service);
                AdditionalServicesListView.ItemsSource = null;
                AdditionalServicesListView.ItemsSource = _appointmentServices;
                TotalPriceText.Text = $"{_appointmentServices.Sum(s => s.PriceAtMoment)} ₽";
            }

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (_initialPage == "dashboardPage")
            {
                NavigationService.Navigate(new DashboardPage());
            }
            else
            {
                NavigationService.Navigate(new AppointmentsPage());
            }
            
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!await ValidateForm())
                return;

            var date = DatePicker.SelectedDate;

            var doctor = DoctorsListBox.SelectedItem as EmployeeDto;
            var pet = PetsListBox.SelectedItem as PetDto;
            var service = ServicesListBox.SelectedItem as ServiceDto;

            var selectedStatus = StatusButtonStackPanel.Children
                .OfType<RadioButton>()
                .First(r => r.IsChecked == true);

            var statusId = Convert.ToInt32(selectedStatus.Tag);

            if (statusId == 5)
            {
                var confirm = await DialogService.ShowConfirm(
                    "После отмены записи редактирование станет недоступным. Продолжить?");

                if (!confirm)
                    return;
            }

            AppointmentDto appointment;

            if (_appointment == null)
            {
                appointment = await _appointmentsService.CreateAppointmentAsync(
                    DateOnly.FromDateTime(date ?? DateTime.MinValue),
                    doctor ?? new(),
                    service ?? new(),
                    pet ?? new(),
                    _selectedSlot,
                    statusId
                );
            }
            else
            {
                appointment = _appointment;

                var updateAppointment = new CreateUpdateAppointmentDto
                {
                    AppointmentDate = DateOnly.FromDateTime(date ?? DateTime.MinValue),
                    AppointmentStatusId = statusId,
                    EmployeeId = doctor?.EmployeeId ?? 0,
                    PetId = pet?.PetId ?? 0,
                    ServiceId = service?.ServiceId ?? 0,
                    StartTime = _selectedSlot,
                };

                await _appointmentsService.UpdateAppointmentAsync(
                    appointment.AppointmentId,
                    updateAppointment);
            }

            var existingServices = await _appointmentsService
                        .GetServicesByAppointmentIdAsync(appointment.AppointmentId)
                        ?? new List<AppointmentServiceDto>();

            foreach (var s in existingServices)
            {
                if (s.AppointmentServiceId > 0)
                {
                    await _appointmentsService.DeleteAppointmentServiceAsync(s.AppointmentServiceId);
                }
            }

            var currentServices = _appointmentServices ?? new List<AppointmentServiceDto>();

            foreach (var s in currentServices)
            {
                var dto = new CreateAppointmentServiceDto
                {
                    AppointmentId = appointment.AppointmentId,
                    ServiceId = s.ServiceId,
                    IsMain = s.IsMain
                };

                await _appointmentsService.CreateAppointmentServiceAsync(dto);
            }

            appointment = await _appointmentsService.GetAppointmentByIdAsync(appointment.AppointmentId) ?? new();

            await _appointmentsService.UpdateAppointmentAsync(
                        appointment.AppointmentId,
                        new CreateUpdateAppointmentDto
                        {
                            AppointmentDate = appointment.AppointmentDate,
                            AppointmentStatusId = statusId,
                            EmployeeId = appointment.EmployeeId,
                            PetId = appointment.PetId,
                            ServiceId = appointment.ServiceId,
                            StartTime = appointment.StartTime,
                        });

            if (_initialPage == "dashboardPage")
            {
                NavigationService.Navigate(new DashboardPage());
            }
            else
            {
                NavigationService.Navigate(new AppointmentsPage());
            }
        }

        private async void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            await ShowAvailableSlots();
        }

        private void DoctorSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplySearch(
                EmployeeSearchTextBox,
                DoctorsListBox,
                _employees,
                x => $"{x.LastName} {x.FirstName} {x.MiddleName}"
            );
        }

        private async void DoctorsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            await ShowAvailableSlots();
        }

        private async void ServicesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isUpdatingSelection)
                return;

            DoctorCard.IsEnabled = true;

            var selectedService = ServicesListBox.SelectedItem as ServiceDto;
            if (selectedService == null)
                return;

            _appointmentServices ??= new List<AppointmentServiceDto>();

            if ((_appointmentServices.Any(s => !s.IsMain)))
            {
                await DialogService.ShowMessage("При смене основной услуги список дополнительных услуг будет очищен. Продолжить?");

                _appointmentServices = _appointmentServices
                        .Where(s => s.IsMain)
                        .ToList();
            }

            _previousSelectedService = selectedService;

            var appointmentService = new AppointmentServiceDto
            {
                ServiceId = selectedService.ServiceId,
                ServiceName = selectedService.ServiceName,
                PriceAtMoment = selectedService.Price,
                IsMain = true
            };

            if (_appointmentServices.Count == 0)
                _appointmentServices.Add(appointmentService);
            else
                _appointmentServices[0] = appointmentService;

            AdditionalServicesListView.ItemsSource = null;
            AdditionalServicesListView.ItemsSource = _appointmentServices;

            TotalPriceText.Text = $"{_appointmentServices.Sum(s => s.PriceAtMoment)} ₽";

            await LoadEmployeesAsync(selectedService);
        }

        private void ServiceSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplySearch(
                ServiceSearchTextBox,
                ServicesListBox,
                _services,
                x => x.ServiceName
            );
        }

        private  void PetsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void PetSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplySearch(
                PetSearchTextBox,
                PetsListBox,
                _pets,
                x => x.Name
            );
        }

        private void ApplySearch<T>(
                        TextBox searchBox,
                        ListBox listBox,
                        IEnumerable<T> source,
                        Func<T, string> selector)
        {
            var text = searchBox.Text?.ToLower() ?? "";

            if (string.IsNullOrWhiteSpace(text))
            {
                listBox.ItemsSource = source;
                return;
            }

            listBox.ItemsSource = source
                .Where(x => selector(x)
                .ToLower()
                .Contains(text))
                .ToList();
        }

        private async Task<bool> ValidateForm()
        {
            if (ClientsListBox.SelectedItem == null)
            {
                await DialogService.ShowMessage("Выберите клиента.");
                return false;
            }

            if (PetsListBox.SelectedItem == null)
            {
                await DialogService.ShowMessage("Выберите питомца.");
                return false;
            }

            if (ServicesListBox.SelectedItem == null)
            {
                await DialogService.ShowMessage("Выберите услугу.");
                return false;
            }

            if (DoctorsListBox.SelectedItem == null)
            {
                await DialogService.ShowMessage("Выберите врача.");
                return false;
            }

            if (DatePicker.SelectedDate == null)
            {
                await DialogService.ShowMessage("Выберите дату приема.");
                return false;
            }

            if (_selectedSlot == default)
            {
                await DialogService.ShowMessage("Выберите временной слот.");
                return false;
            }

            var status = StatusButtonStackPanel.Children
                .OfType<RadioButton>()
                .FirstOrDefault(r => r.IsChecked == true);

            if (status == null)
            {
                await DialogService.ShowMessage("Выберите статус записи.");
                return false;
            }

            return true;
        }

        private async Task InitializeAsync()
        {
            _isPageInitializing = true;

            await LoadClientsAsync();
            await LoadServicesAsync();

            if (_appointment != null)
            {
                await LoadAdditionalServicesAsync(_appointment.AppointmentId);
                await LoadAppointmentDataForEditAsync();
            }

            _isPageInitializing = false;
        }

        private async Task RestoreSelectionAsync(int? doctorId, int? serviceId)
        {
            _isUpdatingSelection = true;

            try
            {
                if (serviceId != null)
                {
                    var service = _services.FirstOrDefault(s => s.ServiceId == serviceId);
                    if (service != null)
                        ServicesListBox.SelectedItem = service;
                }

                if (doctorId != null)
                {
                    var doctor = _employees.FirstOrDefault(d => d.EmployeeId == doctorId);

                    if (doctor != null)
                    {
                        DoctorsListBox.SelectedItem = doctor;
                        _selectedSlot = default;
                        await ShowAvailableSlots();
                    }
                }
            }
            finally
            {
                _isUpdatingSelection = false;
            }
        }
    }
}
