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
using VetSystemModels.Dto.Client;

namespace VetSystemWpfDesktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для PetEditPage.xaml
    /// </summary>
    public partial class PetEditPage : Page
    {
        private readonly ClientsService _clientsService;

        public PetEditPage()
        {
            InitializeComponent();
            var client = new HttpClient() { BaseAddress = new Uri("https://localhost:7146/api/") };
            _clientsService = new ClientsService(client);
            LoadClientsAsync();
        }

        public async void LoadClientsAsync()
        {
            var clients = await _clientsService.GetClientsAsync();
            OwnerListBox.ItemsSource = clients;
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PetsPage());
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OwnerComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void OwnerListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OwnerListBox.SelectedItem is ClientDto selected)
            {
                OwnerLastNameTextBox.Text = selected.LastName;
                OwnerFirstNameTextBox.Text = selected.FirstName;
                OwnerMiddleNameTextBox.Text = selected.MiddleName;
                OwnerPhoneTextBox.Text = selected.Phone;
            }
            else
            {
                OwnerLastNameTextBox.Text = "";
                OwnerFirstNameTextBox.Text = "";
                OwnerMiddleNameTextBox.Text = "";
                OwnerPhoneTextBox.Text = "";
            }
        }
    }
}
