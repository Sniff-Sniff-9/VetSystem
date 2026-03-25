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
using VetSystemModels.Dto.Client;
using VetSystemModels.Dto.Pet;
using VetSystemModels.Entities;
using VetSystemWpfDesktop.Services;

namespace VetSystemWpfDesktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для PetEditPage.xaml
    /// </summary>
    public partial class PetEditPage : Page
    {
        private readonly ClientsService _clientsService;
        private readonly PetsService _petsService;
        private readonly PetDto? _pet;
        private readonly string _mode;
        public bool IsReadOnlyMode => _mode == "check";

        public PetEditPage(PetDto? pet, string mode)
        {
            InitializeComponent();
            _pet = pet;
            _mode = mode;
            DataContext = _pet;
            var client = new HttpClient() { BaseAddress = new Uri("https://localhost:7146/api/") };
            _clientsService = new ClientsService(client);
            _petsService = new PetsService(client);
            if (_mode == "check")
            {
                SaveButton.Visibility = Visibility.Collapsed;
            }
            LoadClientsAsync();
            LoadPetAsync();
        }

        public async void LoadClientsAsync()
        {
            var clients = await _clientsService.GetClientsAsync();

            OwnerListBox.ItemsSource = clients;

            if (_pet != null)
            {
                var client = clients?.FirstOrDefault(c => c.ClientId == _pet.ClientId);
                OwnerListBox.SelectedItem = client;
            }
        }

        public async void LoadPetAsync()
        {
           
            var species = await _petsService.GetSpeciesAsync();

            SpeciesComboBox.ItemsSource = species;
            SpeciesComboBox.DisplayMemberPath = "SpeciesName";

            var genders = await _petsService.GetGendersAsync();

            GendersComboBox.ItemsSource = genders;
            GendersComboBox.DisplayMemberPath = "GenderName";
            if (_pet != null)
            {
                var sp = species?.FirstOrDefault(c => c.SpeciesId == _pet.SpeciesId);
                SpeciesComboBox.SelectedItem = sp;

                BirthDatePicker.SelectedDate = _pet.BirthDate.ToDateTime(new TimeOnly(0, 0, 0));

                var gender = genders?.FirstOrDefault(c => c.GenderId == _pet.GenderId);
                GendersComboBox.SelectedItem = gender;
            }
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
