using System;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using VetSystemModels.Dto.Client;
using VetSystemModels.Dto.Pet;
using VetSystemModels.Entities;
using VetSystemWpfDesktop.Services;

namespace VetSystemWpfDesktop.Pages
{
    public partial class ClientAddPage : Page
    {
        private readonly ClientDto _client;
        private readonly ClientsService _clientsService;
        private readonly PetsService _petsService;

        private readonly string _mode;

        public ClientAddPage(ClientDto client, string mode)
        {
            InitializeComponent();

            _client = client;
            DataContext = client;

            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://localhost:7146/api/")
            };

            _clientsService = new ClientsService(httpClient);
            _petsService = new PetsService(httpClient);

            Loaded += ClientAddPage_Loaded;
            _mode = mode;
            if (_mode == "edit")
            {
                LastnameCard.IsHitTestVisible = true;
                FirstNameCard.IsHitTestVisible = true;
                MiddleNameCard.IsHitTestVisible = true;
                PhoneCard.IsHitTestVisible = true;
                ClientBirthDateCard.IsHitTestVisible = true;
                
            }
            if (_mode == "check")
            {
                SaveButton.Visibility = Visibility.Collapsed;
            }
        }

        private async void ClientAddPage_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadLookupsAsync();
            await LoadPetsAsync();
        }

        private async Task LoadLookupsAsync()
        {
            var species = await _petsService.GetSpeciesAsync();
            var genders = await _petsService.GetGendersAsync();

            SpeciesComboBox.ItemsSource = species;
            SpeciesComboBox.DisplayMemberPath = "SpeciesName";

            GendersComboBox.ItemsSource = genders;
            GendersComboBox.DisplayMemberPath = "GenderName";

            BirthDateDatePicker.SelectedDate = _client.BirthDate.ToDateTime(new TimeOnly(0,0,0));
        }

        private async Task LoadPetsAsync()
        {
            var pets = await _clientsService.GetPetsByClientIdAsync(_client.ClientId) ?? new();

            PetListBox.ItemsSource = pets;

            if (pets.Any())
            {
                PetListBox.SelectedIndex = 0; 
            }
            else
            {
                ClearPetFields();
            }
        }

        private void PetListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PetListBox.SelectedItem is not PetDto pet)
            {
                ClearPetFields();
                return;
            }

            FillPetFields(pet);
        }

        private void FillPetFields(PetDto pet)
        {
            NameTextBox.Text = pet.Name;
            BreedTextBox.Text = pet.Breed;

            BirthDatePicker.SelectedDate =
                pet.BirthDate.ToDateTime(new TimeOnly(0, 0));

            SpeciesComboBox.SelectedItem =
                SpeciesComboBox.Items
                .Cast<Species>()
                .FirstOrDefault(x => x.SpeciesId == pet.SpeciesId);

            GendersComboBox.SelectedItem =
                GendersComboBox.Items
                .Cast<Gender>()
                .FirstOrDefault(x => x.GenderId == pet.GenderId);
        }

        private void ClearPetFields()
        {

            NameTextBox.Text = "";
            BreedTextBox.Text = "";

            BirthDatePicker.SelectedDate = null;

            SpeciesComboBox.SelectedItem = null;
            GendersComboBox.SelectedItem = null;
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService?.CanGoBack == true)
                NavigationService.GoBack();
        }

        private async void PetSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var pets = await _clientsService.GetPetsByClientIdAsync(_client.ClientId) ?? new();

            if (string.IsNullOrWhiteSpace(PetSearchTextBox.Text) || PetSearchTextBox.Text.Length == 0)
            {
                PetListBox.ItemsSource = pets;
                return;
            }

            var search = PetSearchTextBox.Text.ToLower();

            var filtered = pets.Where(p =>
                (p.Name?.ToLower().Contains(search) ?? false)
            ).ToList();

            PetListBox.ItemsSource = filtered;
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!await ValidateAsync())
                return;

            try
            {

                var newClient = await _clientsService.UpdateClientAsync(new UpdateClientDto
                {
                    LastName = LastNameTextBox.Text,
                    FirstName = FirstNameTextBox.Text,
                    MiddleName = MiddleNameTextBox.Text,
                    Phone = PhoneTextBox.Text,
                    BirthDate = DateOnly.FromDateTime(BirthDateDatePicker.SelectedDate ?? new())
                }, _client.ClientId);



                    NavigationService.Navigate(new ClientsPage());
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task<bool> ValidateAsync()
        {

            if (string.IsNullOrWhiteSpace(LastNameTextBox.Text))
            {
                await DialogService.ShowMessage("Введите фамилию клиента.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(FirstNameTextBox.Text))
            {
                await DialogService.ShowMessage("Введите имя клиента.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(MiddleNameTextBox.Text))
            {
                await DialogService.ShowMessage("Введите отчество клиента.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(PhoneTextBox.Text))
            {
                await DialogService.ShowMessage("Введите телефон клиента.");
                return false;
            }

            if (!BirthDateDatePicker.SelectedDate.HasValue)
            {
                await DialogService.ShowMessage("Выберите дату рождения клиента.");
                return false;
            }

            return true;
        }
    }
}