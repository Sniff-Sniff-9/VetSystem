using Syncfusion.UI.Xaml.Scheduler;
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
using VetSystemModels.Dto.Workday;
using VetSystemWpfDesktop.Services;

namespace VetSystemWpfDesktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для SchedulePage.xaml
    /// </summary>
    public partial class SchedulePage : Page
    {
        private readonly EmployeesService _employeesService;

        public SchedulePage()
        {
            InitializeComponent();
            var client = new HttpClient() { BaseAddress = new Uri("https://localhost:7146/api/") };
            _employeesService = new EmployeesService(client);
            LoadEmployeesAsync();
        }

        public async void LoadEmployeesAsync()
        {
            var clients = await _employeesService.GetEmployeessAsync();
            DoctorsList.ItemsSource = clients;
            DoctorsList.SelectedIndex = 0;

            await LoadWorkdaysAsync();
        }

        public async Task LoadWorkdaysAsync()
        {
            var employee = DoctorsList.SelectedItem as EmployeeDto;
            if (employee == null)
                return;

            var workdays = await _employeesService
                .GetWorkdaysByEmployeeIdAsync(employee.EmployeeId);

            var result = new List<WorkdayDto>();

            foreach (var engDay in _weekDaysEng)
            {
                var dbDay = workdays?.FirstOrDefault(w => w.DayOfWeek == engDay);

                if (dbDay != null)
                {
                    // Переводим день на русский
                    dbDay.DayOfWeek = _dayTranslations[engDay];
                    result.Add(dbDay);
                }
                else
                {
                    result.Add(new WorkdayDto
                    {
                        DayOfWeek = _dayTranslations[engDay],
                        StartTime = TimeOnly.MinValue,
                        EndTime = TimeOnly.MinValue,
                        LunchStart = TimeOnly.MinValue,
                        LunchEnd = TimeOnly.MinValue
                    });
                }
            }

            ScheduleList.ItemsSource = result;
        }

        private readonly List<string> _weekDaysEng = new()
        {
            "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"
        };

        
        private readonly Dictionary<string, string> _dayTranslations = new()
        {
            ["Monday"] = "Понедельник",
            ["Tuesday"] = "Вторник",
            ["Wednesday"] = "Среда",
            ["Thursday"] = "Четверг",
            ["Friday"] = "Пятница",
            ["Saturday"] = "Суббота",
            ["Sunday"] = "Воскресенье"
        };

        private async void DoctorsList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            await LoadWorkdaysAsync();
        }
    }
}
