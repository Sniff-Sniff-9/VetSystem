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
using VetSystemWpfDesktop.Services;

namespace VetSystemWpfDesktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для AppointmentsPage.xaml
    /// </summary>
    public partial class AppointmentsPage : Page
    {
        private readonly AppointmentsService _appointmentsService;
        public AppointmentsPage()
        {
            InitializeComponent();
            var client = new HttpClient() { BaseAddress = new Uri("https://localhost:7146/api/") };
            _appointmentsService = new AppointmentsService(client);
            LoadClientsAsync();
        }

        public async void LoadClientsAsync()
        {
            var clients = await _appointmentsService.GetAppointmentsAsync() ?? new();
            AppointmentsListView.ItemsSource = clients;
            AllAppontmentsTextBlock.Text = clients.Count.ToString();
            NewAppontmentsTextBlock.Text = clients.Where(a => a.AppointmentDate.Month == DateTime.Today.Month).ToList().Count.ToString();
        }
        

        private void AddAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AppointmentEditPage(null, "create"));
        }

        private void EditAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is AppointmentDto appointment)
            {
                NavigationService.Navigate(new AppointmentEditPage(appointment, "edit"));
            }
        }

        private void CheckAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is AppointmentDto appointment)
            {
                NavigationService.Navigate(new AppointmentEditPage(appointment, "check"));
            }
        }
    }
}
