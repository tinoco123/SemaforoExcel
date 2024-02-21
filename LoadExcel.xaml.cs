using Microsoft.Win32;
using System;
using System.IO;
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

        private void selectExcel_Click(object sender, RoutedEventArgs e)
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
                    MessageBox.Show(openFileDialog.FileName);
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show("Sin permisos necesarios para acceder al archivo", "Error al cargar archivo excel", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (FileNotFoundException ex)
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
    }
}
