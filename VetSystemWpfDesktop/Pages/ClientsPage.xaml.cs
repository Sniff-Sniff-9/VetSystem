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
    /// Логика взаимодействия для ClientsPage.xaml
    /// </summary>
    public partial class ClientsPage : Page
    {
        private readonly ClientsService _clientsService;
        public ClientsPage()
        {
            InitializeComponent();
            var client = new HttpClient() { BaseAddress = new Uri("https://localhost:7146/api/") };
            _clientsService = new ClientsService(client);
            LoadClientsAsync();
        }

        public async void LoadClientsAsync()
        {
            var clients = await _clientsService.GetClientsAsync();
            ClientsListView.ItemsSource = clients;
        }

        private void AddClientButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
