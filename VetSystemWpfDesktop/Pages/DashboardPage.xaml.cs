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
using VetSystemWpfDesktop.ViewModels;
using VetSystemWpfDesktop.Services;

namespace VetSystemWpfDesktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для DashboardPage.xaml
    /// </summary>
    public partial class DashboardPage : Page
    {
        private readonly AppointmentsService _appointmentsService;
        private readonly EmployeesService _employeesService;
        public DashboardPage()
        {
            InitializeComponent();
            var client = new HttpClient { BaseAddress = new Uri("https://localhost:7146/api/") };
            _appointmentsService = new AppointmentsService(client);
            _employeesService = new EmployeesService(client);
            var vm = new DashboardViewModel(_appointmentsService, _employeesService);
            DataContext = vm;
        }
    }
}
