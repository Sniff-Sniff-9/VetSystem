using System;
using System.Collections.Generic;
using System.Linq;
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
using VetSystemModels.Dto.Appointment;
using VetSystemWpfDesktop.ViewModels;

namespace VetSystemWpfDesktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для DayScheduleView.xaml
    /// </summary>
    public partial class DayScheduleView : UserControl
    {
        public DayScheduleView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Основной метод для загрузки записей из DTO
        /// </summary>
        public void LoadAppointments(List<AppointmentDto> appointmentsDto)
        {
            if (DataContext is DayScheduleViewModel vm)
            {
                vm.LoadAppointments(appointmentsDto);
            }
        }

        /// <summary>
        /// Альтернативный метод, если захочешь передавать уже готовые AppointmentSlot
        /// </summary>
        public void LoadAppointmentSlots(List<AppointmentSlot> appointmentSlots)
        {
            if (DataContext is DayScheduleViewModel vm)
            {
                // Можно добавить метод в ViewModel, если понадобится
                // Пока оставляем пустым или реализуем позже
            }
        }
    }
}
