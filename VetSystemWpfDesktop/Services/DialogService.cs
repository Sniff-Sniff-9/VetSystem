using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VetSystemWpfDesktop.Services
{
    public static class DialogService
    {
        public static async Task ShowMessage(string message)
        {
            var card = new Card
            {
                Width = 380,
                Padding = new Thickness(24)
            };

            var root = new StackPanel();

            var header = new TextBlock
            {
                Text = "Сообщение",
                FontSize = 18,
                FontWeight = FontWeights.SemiBold,
                Margin = new Thickness(0, 0, 0, 16)
            };

            var contentPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 0, 0, 24)
            };

            var icon = new PackIcon
            {
                Kind = PackIconKind.Information,
                Width = 32,
                Height = 32,
                Foreground = new SolidColorBrush(Color.FromRgb(33, 150, 243)), // Material Blue
                Margin = new Thickness(0, 0, 16, 0)
            };

            var text = new TextBlock
            {
                Text = message,
                FontSize = 15,
                TextWrapping = TextWrapping.Wrap,
                VerticalAlignment = VerticalAlignment.Center,
                MaxWidth = 260
            };

            contentPanel.Children.Add(icon);
            contentPanel.Children.Add(text);

            var button = new Button
            {
                Content = "ОК",
                Command = DialogHost.CloseDialogCommand,
                Foreground = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Right
            };

            root.Children.Add(header);
            root.Children.Add(contentPanel);
            root.Children.Add(button);

            card.Content = root;

            await DialogHost.Show(card, "RootDialog");
        }

        public static async Task<bool> ShowConfirm(string message)
        {
            bool result = false;

            var card = new Card
            {
                Width = 380,
                Padding = new Thickness(24)
            };

            var root = new StackPanel();

            var header = new TextBlock
            {
                Text = "Подтверждение",
                FontSize = 18,
                FontWeight = FontWeights.SemiBold,
                Margin = new Thickness(0, 0, 0, 16)
            };

            var contentPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 0, 0, 24)
            };

            var icon = new PackIcon
            {
                Kind = PackIconKind.HelpCircle,
                Width = 32,
                Height = 32,
                Foreground = new SolidColorBrush(Color.FromRgb(33, 150, 243)),
                Margin = new Thickness(0, 0, 16, 0)
            };

            var text = new TextBlock
            {
                Text = message,
                FontSize = 15,
                TextWrapping = TextWrapping.Wrap,
                VerticalAlignment = VerticalAlignment.Center,
                MaxWidth = 260
            };

            contentPanel.Children.Add(icon);
            contentPanel.Children.Add(text);

            var buttonsPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right
            };

            var yesButton = new Button
            {
                Content = "Да",
                Margin = new Thickness(0, 0, 8, 0),
                Foreground = Brushes.White
            };

            var noButton = new Button
            {
                Content = "Нет",
                Foreground = Brushes.White
            };

            yesButton.Click += (s, e) =>
            {
                result = true;
                DialogHost.CloseDialogCommand.Execute(null, null);
            };

            noButton.Click += (s, e) =>
            {
                result = false;
                DialogHost.CloseDialogCommand.Execute(null, null);
            };

            buttonsPanel.Children.Add(yesButton);
            buttonsPanel.Children.Add(noButton);

            root.Children.Add(header);
            root.Children.Add(contentPanel);
            root.Children.Add(buttonsPanel);

            card.Content = root;

            await DialogHost.Show(card, "RootDialog");

            return result;
        }
    }
}
