using Microsoft.AspNetCore.Identity;
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
    /// Логика взаимодействия для ClientEditPage.xaml
    /// </summary>
    public partial class ClientEditPage : Page
    {
        private readonly ClientsService _clientsService;
        private readonly PetsService _petsService;
        private readonly string _initialPage;

        public ClientEditPage(string initialPage)
        {
            InitializeComponent();

            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://localhost:7146/api/")
            };

            _clientsService = new ClientsService(httpClient);
            _petsService = new PetsService(httpClient);
            _initialPage = initialPage;
            Loaded += ClientEditPage_Loaded;
        }

        private async void ClientEditPage_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadLookupsAsync();
        }

        private async Task LoadLookupsAsync()
        {
            var species = await _petsService.GetSpeciesAsync();
            var genders = await _petsService.GetGendersAsync();

            SpeciesComboBox.ItemsSource = species;
            SpeciesComboBox.DisplayMemberPath = "SpeciesName";

            GenderComboBox.ItemsSource = genders;
            GenderComboBox.DisplayMemberPath = "GenderName";
        }


        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!await ValidateAsync())
                return;

            try
            {

                    var newClient = await _clientsService.CreateClientAsync(new CreateClientDto
                    {
                        LastName = LastNameTextBox.Text,
                        FirstName = FirstNameTextBox.Text,
                        MiddleName = MiddleNameTextBox.Text,
                        Phone = PhoneTextBox.Text,
                        BirthDate = DateOnly.FromDateTime(BirthDateDatePicker.SelectedDate ?? new())
                    });


                bool isPetStarted =
                        !string.IsNullOrWhiteSpace(PetNameTextBox.Text) ||
                        !string.IsNullOrWhiteSpace(BreedTextBox.Text) ||
                        SpeciesComboBox.SelectedItem != null ||
                        GenderComboBox.SelectedItem != null ||
                        BirthDateDatePicker.SelectedDate != null;

                if (isPetStarted)
                {
                    var pet = new CreateUpdatePetDto
                    {
                        Name = PetNameTextBox.Text,
                        Breed = BreedTextBox.Text,
                        ClientId = newClient.ClientId,
                        SpeciesId = (SpeciesComboBox.SelectedItem as Species)?.SpeciesId ?? new(),
                        GenderId = (GenderComboBox.SelectedItem as Gender)?.GenderId ?? new(),
                        BirthDate = PetBirthDatePicker.SelectedDate.HasValue
                            ? DateOnly.FromDateTime(PetBirthDatePicker.SelectedDate.Value)
                            : new()
                    };

                    await _petsService.CreatePetAsync(pet);
                }

                if (_initialPage == "dashboardPage")
                {
                    NavigationService.Navigate(new DashboardPage());
                }
                else
                {
                    NavigationService.Navigate(new ClientsPage());
                }
                   
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
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
                NavigationService.Navigate(new ClientsPage());
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

            bool isPetStarted =
                !string.IsNullOrWhiteSpace(PetNameTextBox.Text) ||
                !string.IsNullOrWhiteSpace(BreedTextBox.Text) ||
                SpeciesComboBox.SelectedItem != null ||
                GenderComboBox.SelectedItem != null ||
                BirthDateDatePicker.SelectedDate != null;

            if (!isPetStarted)
                return true;

            if (string.IsNullOrWhiteSpace(PetNameTextBox.Text))
            {
                await DialogService.ShowMessage("Введите имя питомца.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(BreedTextBox.Text))
            {
                await DialogService.ShowMessage("Введите породу питомца.");
                return false;
            }

            if (SpeciesComboBox.SelectedItem == null)
            {
                await DialogService.ShowMessage("Выберите вид питомца.");
                return false;
            }

            if (GenderComboBox.SelectedItem == null)
            {
                await DialogService.ShowMessage("Выберите пол питомца.");
                return false;
            }

            if (!PetBirthDatePicker.SelectedDate.HasValue)
            {
                await DialogService.ShowMessage("Выберите дату рождения питомца.");
                return false;
            }

            return true;
        }

    }
}
