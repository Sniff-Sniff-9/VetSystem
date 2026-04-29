using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using VetSystemModels.Dto.Client;
using VetSystemWpfDesktop.Services;

namespace VetSystemWpfDesktop.Pages
{
    public partial class ClientsPage : Page
    {
        private readonly ClientsService _clientsService;

        private List<ClientDto> _allClients = new();

        public ClientsPage()
        {
            InitializeComponent();

            var client = new HttpClient()
            {
                BaseAddress = new Uri("https://localhost:7146/api/")
            };

            _clientsService = new ClientsService(client);

            InitFilters();
            LoadClientsAsync();
        }

        public async void LoadClientsAsync()
        {
            _allClients = await _clientsService.GetClientsAsync() ?? new();

            RefreshList();

            AllClientsTextBlock.Text = _allClients.Count.ToString();
        }

        private void RefreshList()
        {
            ClientsListView.ItemsSource = ApplyFilters();
        }

        private List<ClientDto> ApplyFilters()
        {
            IEnumerable<ClientDto> query = _allClients;

            if (!string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                var search = SearchTextBox.Text.ToLower();

                query = query.Where(c =>
                    (c.LastName?.ToLower().Contains(search) ?? false) ||
                    (c.FirstName?.ToLower().Contains(search) ?? false) ||
                    (c.MiddleName?.ToLower().Contains(search) ?? false) ||
                    (c.Phone?.ToLower().Contains(search) ?? false));
            }

            return query.ToList();
        }

        private void InitFilters()
        {
            SearchTextBox.TextChanged += (s, e) => RefreshList();
        }

        private void AddClientButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ClientEditPage("clientsPage"));
        }

        private void CheckClientButton_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is ClientDto client)
            {
                NavigationService.Navigate(new ClientAddPage(client, "check"));
            }
        }

        private void EditClientButton_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is ClientDto client)
            {
                NavigationService.Navigate(new ClientAddPage(client, "edit"));
            }
        }

        private async void DeleteClientButton_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is not ClientDto client)
                return;

            var confirm = await DialogService.ShowConfirm("Удалить клиента?");
            if (!confirm)
                return;

            try
            {
                await _clientsService.DeleteClientAsync(client.ClientId);

                _allClients.RemoveAll(c => c.ClientId == client.ClientId);

                RefreshList();
            }
            catch (Exception ex)
            {
                await DialogService.ShowMessage($"Ошибка удаления: {ex.Message}");
            }
        }
    }
}