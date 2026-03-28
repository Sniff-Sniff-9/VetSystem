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
using VetSystemModels.Dto.Employee;
using VetSystemModels.Dto.Pet;
using VetSystemWpfDesktop.Services;

namespace VetSystemWpfDesktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для EmployeesPage.xaml
    /// </summary>
    public partial class EmployeesPage : Page
    {
        private readonly EmployeesService _employeesService;
        public EmployeesPage()
        {
            InitializeComponent();
            var client = new HttpClient() { BaseAddress = new Uri("https://localhost:7146/api/") };
            _employeesService = new EmployeesService(client);
            LoadEmployeesAsync();
        }

        public async void LoadEmployeesAsync()
        {
            var clients = await _employeesService.GetEmployeessAsync();
            ClientsListView.ItemsSource = clients;
        }

        private void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CheckEmpployeeSchedule_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is EmployeeDto employee)
            {
                NavigationService.Navigate(new EmployeeSchedulePage(employee));
            }
        }
    }
}
