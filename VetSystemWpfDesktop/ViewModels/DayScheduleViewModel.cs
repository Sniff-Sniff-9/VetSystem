using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;
using VetSystemModels.Dto.Appointment;

namespace VetSystemWpfDesktop.ViewModels
{
    public class DayScheduleViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<TimeSlot> TimeSlots { get; } = new();

        public DayScheduleViewModel()
        {
            GenerateTimeSlots();
        }

 
        private void GenerateTimeSlots()
        {
            TimeSlots.Clear();
            var currentTime = new DateTime(1, 1, 1, 7, 0, 0);

            for (int i = 0; i < 30; i++) 
            {

                TimeSlots.Add(new TimeSlot
                {
                    TimeLabel = currentTime.ToString("HH:mm"),
                    Appointments = new ObservableCollection<AppointmentSlot>(),
                    BackgroundBrush = currentTime == DateTime.Now ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#c2ecff")) : Brushes.White
                });

                currentTime = currentTime.AddMinutes(30);
            }
        }

        public void LoadAppointments(List<AppointmentDto> appointmentsDto)
        {
            foreach (var slot in TimeSlots)
                slot.Appointments.Clear();

            foreach (var dto in appointmentsDto)
            {
                var startTime = dto.AppointmentDate.ToDateTime(dto.StartTime);
                var endTime = dto.AppointmentDate.ToDateTime(dto.EndTime);


                var appointmentSlot = new AppointmentSlot
                {
                    AppointmentId = dto.AppointmentId,
                    StartTime = startTime,
                    EndTime = endTime,
                    PetName = dto.PetName,
                    ServiceName = dto.ServiceName,
                    ClientName = dto.ClientName,
                    StatusName = dto.AppointmentStatusName
                };

               
                foreach (var slot in TimeSlots)
                {
                    var slotTime = TimeSpan.Parse(slot.TimeLabel);
                    var slotStart = new DateTime(startTime.Year, startTime.Month, startTime.Day,
                                                 slotTime.Hours, slotTime.Minutes, 0);
                    var slotEnd = slotStart.AddMinutes(30); 
                    
                    if (startTime < slotEnd && endTime >= slotStart)
                    {
                        slot.Appointments.Add(appointmentSlot);
                    }
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class TimeSlot
    {
        public string TimeLabel { get; set; } = string.Empty;

        public Brush BackgroundBrush = Brushes.White;
        public ObservableCollection<AppointmentSlot> Appointments { get; set; } = new();
    }

    public class AppointmentSlot
    {
        public int AppointmentId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public string PetName { get; set; } = string.Empty;
        public string ServiceName { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public string StatusName { get; set; } = string.Empty;

        public string DisplayText => $"{PetName} ({ServiceName})";
        public string TimeRange => $"{StartTime:HH:mm} — {EndTime:HH:mm}";

        public Brush BackgroundBrush
        {
            get => StatusName switch
            {
                "В процессе" or "Запланирован" => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#81D4FA")), // голубой
                "Завершен" => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A5D6A7")), // зелёный
                "Отменен" => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFAB91")), // оранжевый
                "Ожидает" => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF59D")), // жёлтый
                _ => new SolidColorBrush(Colors.LightGray)
            };
        }
    }
}