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
using System.Windows.Shapes;
using VetSystemModels.Dto.Service;
using VetSystemWpfDesktop.Services;

namespace VetSystemWpfDesktop.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddAdditionalServicesWindow.xaml
    /// </summary>
    public partial class AddAdditionalServicesWindow : Window
    {
        public ServiceDto? SelectedService { get; private set; }

        private List<ServiceDto> _services;

        public AddAdditionalServicesWindow(List<ServiceDto> services)
        {
            InitializeComponent();

            _services = services;
            ServicesListView.ItemsSource = _services;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            SelectedService = ServicesListView.SelectedItem as ServiceDto;

            if (SelectedService == null) return;
            
            DialogResult = true;
        }


        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = SearchTextBox.Text.ToLower();

            ServicesListView.ItemsSource = _services
                .Where(s => s.ServiceName.ToLower().Contains(text))
                .ToList();
        }
    }
}
