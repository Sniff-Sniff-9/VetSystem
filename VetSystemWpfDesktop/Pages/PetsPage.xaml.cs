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
using VetSystemModels.Dto.Pet;
using VetSystemWpfDesktop.Services;

namespace VetSystemWpfDesktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для PetsPage.xaml
    /// </summary>
    public partial class PetsPage : Page
    {
        private readonly PetsService _petsService;

        public PetsPage()
        {
            InitializeComponent();
            var client = new HttpClient() {BaseAddress = new Uri("https://localhost:7146/api/")};
            _petsService = new PetsService(client);
            LoadPetsAsync();
        }

        public async void LoadPetsAsync()
        {
            var pets = await _petsService.GetPetsAsync();
            PetsListView.ItemsSource = pets;
        }

        private void AddPetButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PetEditPage(null, "create"));
        }

        private void EditPetButton_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is PetDto pet)
            {
                NavigationService.Navigate(new PetEditPage(pet, "edit"));
            }
        }

        private void DeletePetButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CheckPetButton_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is PetDto pet)
            {
                NavigationService.Navigate(new PetEditPage(pet, "check"));
            }
        }
    }
}
