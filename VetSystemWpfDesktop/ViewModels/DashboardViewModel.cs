using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Media;
using VetSystemModels.Dto.Appointment;
using VetSystemModels.Dto.Employee;
using VetSystemWpfDesktop.Services;

namespace VetSystemWpfDesktop.ViewModels
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        private readonly AppointmentsService _appointmentService;
        private readonly EmployeesService _employeesService;

        private int _todayAppointmentsCount;
        public int TodayAppointmentsCount
        {
            get => _todayAppointmentsCount;
            set
            {
                _todayAppointmentsCount = value;
                OnPropertyChanged(nameof(TodayAppointmentsCount));
            }
        }

        private decimal _workloadPercent;
        public decimal WorkloadPercent
        {
            get => _workloadPercent;
            set
            {
                _workloadPercent = value;
                OnPropertyChanged(nameof(WorkloadPercent));
            }
        }

        private decimal _incomePerMonth;
        public decimal IncomePerMonth
        {
            get => _incomePerMonth;
            set
            {
                _incomePerMonth = value;
                OnPropertyChanged(nameof(IncomePerMonth));
            }
        }

        private List<AppointmentDto>? _appointmentsNear;
        public List<AppointmentDto>? AppointmentsNear
        {
            get => _appointmentsNear;
            set
            {
                _appointmentsNear = value;
                OnPropertyChanged(nameof(AppointmentsNear));
            }
        }

        private List<AppointmentDto>? _appointmentsExpect;
        public List<AppointmentDto>? AppointmentsExpect
        {
            get => _appointmentsExpect;
            set
            {
                _appointmentsExpect = value;
                OnPropertyChanged(nameof(AppointmentsExpect));
            }
        }


        private SeriesCollection _clinicLoadSeries = new SeriesCollection();
        public SeriesCollection ClinicLoadSeries
        {
            get => _clinicLoadSeries;
            set
            {
                _clinicLoadSeries = value;
                OnPropertyChanged(nameof(ClinicLoadSeries));
            }
        }

        private List<string> _clinicLoadLabels = new List<string>();
        public List<string> ClinicLoadLabels
        {
            get => _clinicLoadLabels;
            set
            {
                _clinicLoadLabels = value;
                OnPropertyChanged(nameof(ClinicLoadLabels));
            }
        }

        public DashboardViewModel(AppointmentsService appointmentService, EmployeesService employeesService)
        {
            _appointmentService = appointmentService;
            _employeesService = employeesService;
            _ = LoadClinicLabelsAsync();
            _ = LoadWorkloadPercentAsync();
        }

        public async Task LoadClinicLabelsAsync()
        {
            var appointments = await _appointmentService.GetAppointmentsAsync() ?? new List<AppointmentDto>();

            IncomePerMonth = appointments.Where(a => a.AppointmentStatusId == 4).Sum(a => a.TotalPriceAtMoment);
            TodayAppointmentsCount = appointments.Count(a => a.AppointmentDate.ToDateTime(new TimeOnly(0, 0, 0)) == DateTime.Today);
            AppointmentsNear = appointments.Where(a => a.StartTime.AddHours(2) == TimeOnly.FromDateTime(DateTime.Now)).ToList();
            AppointmentsExpect = appointments.Where(a => a.AppointmentStatusId == 1).ToList();

            var startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + 1); 
            var dailyCounts = new List<double>();
            var labels = new List<string>();

            for (int i = 0; i < 7; i++)
            {
                var day = startOfWeek.AddDays(i);
                // Считаем количество записей на этот день
                var count = appointments.Count(a => a.AppointmentDate.ToDateTime(new TimeOnly(0,0,0)) == day.Date);
                dailyCounts.Add(count);
                var culture = new CultureInfo("ru-RU");
                labels.Add(culture.TextInfo.ToTitleCase(day.ToString("dddd", culture)));
            }

            // Обновляем свойства, чтобы UI перерисовал график
            ClinicLoadLabels = labels;

            ClinicLoadSeries = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title="Записей",
                    Values = new ChartValues<double>(dailyCounts),
                    Fill = new SolidColorBrush(Color.FromRgb(3,169,244)) 
                }
            };
        }

        public async Task LoadWorkloadPercentAsync()
        {
            var employees = await _employeesService.GetEmployeessAsync() ?? new();
            var date = DateOnly.FromDateTime(DateTime.Today);
            var availableSlots = new List<TimeOnly>();
            var allSlots = new List<TimeOnly>();

            foreach (EmployeeDto e in employees)
            {
                var formattedDate = date.ToString("yyyy-MM-dd");
                var employeeAvailableSlots = await _appointmentService.GetAvailableSlotsAsync(e.EmployeeId, date) ?? new();
                var employeeAllSlots = await _appointmentService.GetAllSlotsAsync(e.EmployeeId, date) ?? new();
                availableSlots.AddRange(employeeAvailableSlots);
                allSlots.AddRange(employeeAllSlots);
            }

            WorkloadPercent = allSlots.Count == 0 ? 0 : 1 - ((decimal)availableSlots.Count / allSlots.Count);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}