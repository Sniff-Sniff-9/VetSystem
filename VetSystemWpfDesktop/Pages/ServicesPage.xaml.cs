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

namespace VetSystemWpfDesktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для ServicesPage.xaml
    /// </summary>
    public partial class ServicesPage : Page
    {
        private readonly ServicesService _servicesService;
        public ServicesPage()
        {
            InitializeComponent();
            var client = new HttpClient() { BaseAddress = new Uri("https://localhost:7146/api/") };
            _servicesService = new ServicesService(client);
            LoadServicesAsync();
        }

        public async void LoadServicesAsync()
        {
            var services = await _servicesService.GetServicesAsync();
            ServicesListView.ItemsSource = services;
            AllServicesTextBlock.Text = services?.Count.ToString();
        }

        private void AddServiceButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
