using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using VetSystemModels.Dto.Employee;
using VetSystemWpfDesktop.Services;
using VetSystemWpfDesktop.ViewModels;

namespace VetSystemWpfDesktop.Pages
{
    public partial class EmployeeSchedulePage : Page
    {
        private readonly AppointmentsService _appointmentsService;
        private readonly EmployeeDto _employee;
        private readonly DayScheduleViewModel _scheduleVm;

        public EmployeeSchedulePage(EmployeeDto employee)
        {
            InitializeComponent();

            _employee = employee;
            DataContext = _employee;
            // HTTP клиент
            var client = new HttpClient { BaseAddress = new Uri("https://localhost:7146/api/") };
            _appointmentsService = new AppointmentsService(client);

            // DayScheduleViewModel
            _scheduleVm = new DayScheduleViewModel();
            ScheduleView.DataContext = _scheduleVm;

            // Карточка сотрудника
            EmployeeFullName.Text = $"{_employee.LastName} {_employee.FirstName} {_employee.MiddleName}";

            // Загружаем записи на сегодня
            _ = LoadAppointmentsAsync(DateTime.Today); 
        }

        private async Task LoadAppointmentsAsync(DateTime date)
        {
            try
            {
                var allAppointments = await _appointmentsService.GetAppointmentsByEmployeeIdAsync(_employee.EmployeeId);
                if (allAppointments != null)
                {
                    // Фильтруем по выбранной дате
                    var filtered = allAppointments
                        .FindAll(a => a.AppointmentDate.ToDateTime(new TimeOnly(0, 0, 0)) == date.Date);

                    _scheduleVm.LoadAppointments(filtered);
                    AppointmentsCountTextBlock.Text = $"Записей сегодня: {filtered.Count().ToString()} ";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки расписания: {ex.Message}", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadWorkloadPercent()
        {
            var date = DateOnly.FromDateTime(ScheduleCalendar.SelectedDate ?? DateTime.Today);
            var availableSlots = await _appointmentsService.GetAvailableSlotsAsync(_employee.EmployeeId, date) ?? new();
            var allSlots = await _appointmentsService.GetAllSlotsAsync(_employee.EmployeeId, date) ?? new();
            var workloadPercent = Math.Round((1 - ((decimal)availableSlots.Count / (decimal)allSlots.Count)) * 100);
            WorkloadTextBlock.Text = $"Загруженность: {workloadPercent}%";
        }

        // При смене даты в календаре
        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is DateTime selectedDate)
            {
                _ = LoadAppointmentsAsync(selectedDate);
                _ = LoadWorkloadPercent();
            }
        }
    }
}