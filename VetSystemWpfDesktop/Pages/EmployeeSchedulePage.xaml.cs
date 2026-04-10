using System;
using System.Linq;
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
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7146/api/")
            };
            _appointmentsService = new AppointmentsService(client);

            // ViewModel для расписания
            _scheduleVm = new DayScheduleViewModel();
            ScheduleView.DataContext = _scheduleVm;

            // ФИО сотрудника
            EmployeeFullName.Text = $"{_employee.LastName} {_employee.FirstName} {_employee.MiddleName}";

            // Загружаем данные для сегодняшнего дня
            _ = LoadDataForDateAsync(DateTime.Today);
        }

        // Универсальный метод загрузки данных для выбранной даты
        private async Task LoadDataForDateAsync(DateTime date)
        {
            await LoadAppointmentsAsync(date);
            await LoadWorkloadPercent(date);
        }

        private async Task LoadAppointmentsAsync(DateTime date)
        {
            try
            {
                var allAppointments = await _appointmentsService.GetAppointmentsByEmployeeIdAsync(_employee.EmployeeId);

                if (allAppointments != null)
                {
                    var filtered = allAppointments
                        .FindAll(a => a.AppointmentDate.ToDateTime(TimeOnly.MinValue).Date == date.Date && a.AppointmentStatusId != 5);

                    _scheduleVm.LoadAppointments(filtered);

                    AppointmentsCountTextBlock.Text = $"Записей на день: {filtered.Count}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки записей: {ex.Message}", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadWorkloadPercent(DateTime date)
        {
            try
            {
                var dateOnly = DateOnly.FromDateTime(date);

                var availableSlots = await _appointmentsService.GetAvailableSlotsAsync(_employee.EmployeeId, dateOnly) ?? new();
                var allSlots = await _appointmentsService.GetAllSlotsAsync(_employee.EmployeeId, dateOnly) ?? new();

                if (allSlots.Count == 0)
                {
                    WorkloadTextBlock.Text = "Загруженность: 0%";
                    return;
                }

                decimal workloadPercent = Math.Round((1 - ((decimal)availableSlots.Count / allSlots.Count)) * 100);
                WorkloadTextBlock.Text = $"Загруженность: {workloadPercent}%";
            }
            catch (Exception)
            {
                WorkloadTextBlock.Text = "Загруженность: —";
                // Можно оставить MessageBox, но лучше не показывать при каждой загрузке
                // MessageBox.Show($"Ошибка расчёта загруженности: {ex.Message}");
            }
        }

        // Обработчик смены даты в календаре
        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is DateTime selectedDate)
            {
                _ = LoadDataForDateAsync(selectedDate);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EmployeesPage());
        }
    }
}