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
using VetSystemWpfDesktop.Services;
using VetSystemModels.Dto.Service;
using VetSystemModels.Dto.Employee;

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
        private TimeOnly _selectedSlot;

        private List<ServiceDto> _additionalServices = new();

        private decimal _basePrice = 0;

        public AppointmentEditPage()
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

            _ = LoadClientsAsync();
            _ = LoadServicesAsync();
        }

        private async Task LoadServicesAsync()
        {
            var services = await _servicesService.GetServicesAsync();
            AdditionalServicesListView.ItemsSource = services;
        }

        private async Task LoadClientsAsync()
        {
            var clients = await _clientsService.GetClientsAsync();
            ClientsListBox.ItemsSource = clients;

            await LoadEmployeesAsync();
        }

        private async Task LoadEmployeesAsync()
        {
            var employees = await _employeesService.GetEmployeessAsync();
            DoctorsListBox.ItemsSource = employees;
        }

        private async Task ShowAvailableSlots()
        {
            var employee = DoctorsListBox.SelectedItem as EmployeeDto ?? new();
            var date = DatePicker.SelectedDate ?? DateTime.Today;
            var slots = await _appointmentsService.GetAvailableSlotsAsync(employee.EmployeeId, DateOnly.FromDateTime(date));

            SlotsPanel.Children.Clear();

            if (slots == null || !slots.Any())
            {
                var noSlotsText = new TextBlock
                {
                    Text = "Нет доступных слотов.",
                    Foreground = Brushes.Gray,
                    Margin = new Thickness(4)
                };
                SlotsPanel.Children.Add(noSlotsText);
                return;
            }

            foreach (var slot in slots)
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

                SlotsPanel.Children.Add(btn);
            }
        }

        private void SlotButton_Click(object sender, RoutedEventArgs e)
        {
            // Сброс всех кнопок в обычное состояние
            foreach (var child in SlotsPanel.Children.OfType<Button>())
            {
                child.Background = Brushes.LightGray;
                child.Foreground = Brushes.Black;
                child.BorderBrush = Brushes.LightGray;
            }

            // Отмечаем выбранный слот
            var btn = sender as Button;
            if (btn == null) return;

            _selectedSlot = (TimeOnly)btn.Tag;
            btn.Background = Brushes.DodgerBlue;
            btn.Foreground = Brushes.White;
            btn.BorderBrush = Brushes.DodgerBlue;
        }

        private void ClientSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
          
        }

        private void ClientsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void SelectSlotButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddServiceButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RemoveServiceButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

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

        private void ServicesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ServiceSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void PetsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void PetSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
