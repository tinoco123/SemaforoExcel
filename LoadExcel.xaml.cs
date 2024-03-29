﻿using Microsoft.Win32;
using Semaforo.Models;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace Semaforo
{
    /// <summary>
    /// Interaction logic for LoadExcel.xaml
    /// </summary>
    public partial class LoadExcel : Window
    {
        public LoadExcel()
        {
            InitializeComponent();
        }

        private async void selectExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {

                    Title = "Buscar archivos excel",
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    CheckFileExists = true,
                    CheckPathExists = true,
                    Multiselect = false,
                    Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
                    ReadOnlyChecked = true,
                    ShowReadOnly = true
                };
                if (openFileDialog.ShowDialog() == true)
                {
                    selectExcel.IsEnabled = false;
                    StatusTB.Text = "Procesando excel";
                    CaptionTB.Text = "Espera solo unos segundos";
                    List<Item> itemsCollection = await GetItemsFromExcel(openFileDialog.FileName);
                    if (itemsCollection != null)
                    {
                        MainWindow mainWindow = new MainWindow(itemsCollection);
                        mainWindow.Show();
                        Close();
                    }
                    else
                    {
                        selectExcel.IsEnabled = true;
                        StatusTB.Text = "Seleccionar archivo excel";
                        CaptionTB.Text = "Mediante el excel seleccionado se obtendrán los datos necesarios para mostrar el semáforo.";
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Sin permisos necesarios para acceder al archivo", "Error al cargar archivo excel", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("No se encontró el archivo", "Error al cargar archivo excel", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al cargar archivo excel", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            Close();
            login.Show();
        }

        private async Task<List<Item>> GetItemsFromExcel(string path)
        {
            return await Task.Run(() =>
            {
                try
                {
                    List<Item> itemsCollection = new List<Item>();
                    SLDocument excelFile = new SLDocument(path);
                    SLWorksheetStatistics excelStatistics = excelFile.GetWorksheetStatistics();
                    int endRowIndex = excelStatistics.EndRowIndex;
                    string preItem = string.Empty;
                    string subitem = string.Empty;
                    for (int rowIndex = 6; rowIndex <= endRowIndex; rowIndex++)
                    {
                        string lotNumber = excelFile.GetCellValueAsString(rowIndex, 7);
                        if (String.IsNullOrEmpty(lotNumber)) // Check if lot number is empty
                        {
                            if (!String.IsNullOrEmpty(excelFile.GetCellValueAsString(rowIndex, 3))) // Check if preitem is not empty
                            {
                                preItem = excelFile.GetCellValueAsString(rowIndex, 3);
                                if (preItem.StartsWith("Total"))
                                {
                                    preItem = string.Empty;
                                }
                            }
                            else if (!String.IsNullOrEmpty(excelFile.GetCellValueAsString(rowIndex, 4))) // Get SubItem
                            {
                                subitem = excelFile.GetCellValueAsString(rowIndex, 4);
                                if (subitem.StartsWith("Total"))
                                {
                                    subitem = string.Empty;
                                }
                            }
                            else
                            {
                                Item newItem = CreateItem(excelFile, preItem, subitem, lotNumber, rowIndex);
                                itemsCollection.Add(newItem);
                            }
                        }
                        else
                        {
                            Item newItem = CreateItem(excelFile, preItem, subitem, lotNumber, rowIndex);
                            itemsCollection.Add(newItem);
                        }
                    }
                    excelFile.CloseWithoutSaving();
                    return itemsCollection;
                }
                catch (IOException)
                {
                    MessageBox.Show("El archivo seleccionado está siendo utilizado por otro programa", "Error al cargar archivo excel", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}", "Error inesperado", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                return null;
            });

        }

        private Item CreateItem(SLDocument excelFile, string preItem, string subitem, string lotNumber, int rowIndex)
        {
            string fullItem = string.IsNullOrEmpty(subitem) ? preItem : preItem + " " + subitem;
            double value = excelFile.GetCellValueAsDouble(rowIndex, 13);
            double quantity = excelFile.GetCellValueAsDouble(rowIndex, 15);
            int daysUntilExpirationDate = excelFile.GetCellValueAsInt32(rowIndex, 11);
            DateTime expirationDate = excelFile.GetCellValueAsDateTime(rowIndex, 9);
            Item newItem;
            if (expirationDate.Year == 1900)
            {
                string expirationDateView = lotNumber.Contains("SC") ? "Sin Caducidad" : "No Definido";
                string color = expirationDateView.Equals("Sin Caducidad") ? "Blanco" : "Morado";
                newItem = new Item(fullItem, lotNumber, expirationDate, expirationDateView, daysUntilExpirationDate, color, value, quantity);
            }
            else
            {
                string color;
                if (daysUntilExpirationDate <= 30)
                {
                    color = "Rojo";
                }
                else if (daysUntilExpirationDate <= 90)
                {
                    color = "Tomate";
                }
                else
                {
                    color = "Verde";
                }
                newItem = new Item(fullItem, lotNumber, expirationDate, expirationDate.ToShortDateString(), daysUntilExpirationDate, color, value, quantity);
            }
            return newItem;
        }
    }
}
