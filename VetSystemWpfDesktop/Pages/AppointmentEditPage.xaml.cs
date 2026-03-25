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
        //private DoctorsService _doctorsService;

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
            //_doctorsService = new EmployeeService(client);

            LoadServices();
        }

        private async void LoadServices()
        {
            var services = await _servicesService.GetServicesAsync();
            AdditionalServicesListView.ItemsSource = services;
        }

        private void ClientSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //var text = ClientSearchTextBox.Text;

            //var clients = await _clientsService.Search(text);

            //ClientsListBox.ItemsSource = clients;
        }

        private void ClientsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void PetsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ServiceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DoctorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DoctorSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void DoctorsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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
