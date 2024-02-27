using CsvHelper;
using Microsoft.Win32;
using Semaforo.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Semaforo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly List<Item> itemsCollection = new List<Item>();
        private List<Item> filteredItems = null;
        public MainWindow(List<Item> itemsCollection)
        {
            InitializeComponent();
            this.itemsCollection = itemsCollection;
            lotNumbersTable.ItemsSource = this.itemsCollection;
            filteredItems = this.itemsCollection;
            itemsComboBox.ItemsSource = this.itemsCollection.Select(item => item.ItemName).Distinct().ToList();
            colorsComboBox.ItemsSource = new List<string>() { "Blanco", "Morado", "Rojo", "Tomate", "Verde" };
        }

        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            Close();
            login.Show();
        }

        private void filterBtn_Click(object sender, RoutedEventArgs e)
        {
            string selectedItem = itemsComboBox.SelectedItem as string;
            string selectedColor = colorsComboBox.SelectedItem as string;

            var filteredItems = itemsCollection.AsQueryable();

            if (selectedItem != null)
            {
                filteredItems = filteredItems.Where(item => item.ItemName == selectedItem);
            }

            if (selectedColor != null)
            {
                filteredItems = filteredItems.Where(item => item.Color == selectedColor);
            }

            if (initialDate.SelectedDate != null)
            {
                DateTime initialDateSelected = (DateTime)initialDate.SelectedDate;
                filteredItems = filteredItems.Where(item => item.ExpirationDate >= initialDateSelected);
            }
            if (finalDate.SelectedDate != null)
            {
                DateTime finalDateSelected = (DateTime)finalDate.SelectedDate;
                filteredItems = filteredItems.Where(item => item.ExpirationDate <= finalDateSelected);
            }
            searchTextBox.Text = "";
            this.filteredItems = filteredItems.ToList();
            lotNumbersTable.ItemsSource = this.filteredItems;
        }
        private void CleanFilter_Btn_Click(object sender, RoutedEventArgs e)
        {
            itemsComboBox.SelectedItem = null;
            colorsComboBox.SelectedItem = null;
            initialDate.SelectedDate = null;
            finalDate.SelectedDate = null;
            searchTextBox.Text = "";
            lotNumbersTable.ItemsSource = itemsCollection;
            filteredItems = itemsCollection;
        }

        private void ExportExcel_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFile.Title = "Guardar archivo CSV";
            saveFile.DefaultExt = "csv";
            saveFile.Filter = "CSV (*.csv)|*.csv";
            bool? result = saveFile.ShowDialog();
            if (result == true)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(saveFile.FileName, false, Encoding.GetEncoding("ISO-8859-1")))
                    using (CsvWriter csv = new CsvWriter(writer, CultureInfo.CurrentCulture))
                    {
                        csv.WriteRecords(filteredItems);
                    }
                    MessageBox.Show("El archivo csv fue guardado exitosamente", "Operación exitosa", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }

        private void searchTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            string textToSearch = searchTextBox.Text;
            if (textToSearch.Length >= 0)
            {
                List<Item> foundItems = filteredItems.Where(item => item.ItemName.Contains(textToSearch) || item.LotNumber.Contains(textToSearch)).ToList();
                lotNumbersTable.ItemsSource = foundItems;
            }
        }

        private void lotNumbersTable_LoadingRow(object sender, System.Windows.Controls.DataGridRowEventArgs e)
        {
            try
            {
                Item item = e.Row.Item as Item;
                if (item != null)
                {
                    if (item.Color == "Blanco")
                    {
                        e.Row.Background = Brushes.White;
                    }
                    else if (item.Color == "Morado")
                    {
                        e.Row.Background = new SolidColorBrush(Color.FromRgb(195, 158, 255));
                    }
                    else if (item.Color == "Rojo")
                    {
                        e.Row.Background = new SolidColorBrush(Color.FromRgb(255, 116, 82));
                    }
                    else if (item.Color == "Tomate")
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
