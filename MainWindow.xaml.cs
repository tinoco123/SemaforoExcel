using Semaforo.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Semaforo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Item> itemsCollection = new List<Item>();
        public MainWindow(List<Item> itemsCollection)
        {
            InitializeComponent();
            this.itemsCollection = itemsCollection;
            lotNumbersTable.ItemsSource = itemsCollection;
        }

        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            Close();
            login.Show();
        }

        private void filterBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CleanFilter_Btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExportExcel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void searchTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void lotNumbersTable_LoadingRow(object sender, System.Windows.Controls.DataGridRowEventArgs e)
        {
            try
            {
                Item item = e.Row.Item as Item;
                if (item != null)
                {
                    if (item.ExpirationDateView.Equals("Sin Caducidad"))
                    {
                        e.Row.Background = Brushes.White;
                    }
                    else if (item.ExpirationDateView.Equals("No definido"))
                    {
                        e.Row.Background = new SolidColorBrush(Color.FromRgb(195, 158, 255));
                    }
                    else if (item.DaysUntilExpirationDate <= 30)
                    {
                        e.Row.Background = new SolidColorBrush(Color.FromRgb(255, 116, 82));
                    }
                    else if (item.DaysUntilExpirationDate <= 90)
                    {
                        e.Row.Background = new SolidColorBrush(Color.FromRgb(255, 160, 82));
                    }
                    else
                    {
                        e.Row.Background = new SolidColorBrush(Color.FromRgb(20, 255, 149));
                    }
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
