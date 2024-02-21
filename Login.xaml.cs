using Semaforo.Models;
using System.Windows;

namespace Semaforo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = UserNameTextBox.Text;
            string password = PasswordTextBox.Password;

            if (LoginDataIsValid(username.Trim(), password.Trim()))
            {
                if (username.Equals("Araneda") && password.Equals("uMTf$qINx24H"))
                {
                    LoadExcel loadExcel = new LoadExcel();
                    loadExcel.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("Usuario o contraseña incorrectos", "Error de inicio de sesión", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Asegurese de rellenar todos los campos", "Alerta de inicio de sesión", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private bool LoginDataIsValid(string username, string password)
        {
            return !username.Equals("") && !password.Equals("");
        }
    }
}
