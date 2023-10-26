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

namespace Museum
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MuseumEntities1 database = new MuseumEntities1();
        AuthorizedWindow aw;

        public MainWindow()
        {
            InitializeComponent();

            PasswordTextBox.Visibility = Visibility.Hidden;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string log = string.Empty;
            string pass = string.Empty;

            log = LoginTextBox.Text;
            if (PassCheck.IsChecked == true)
                pass = PasswordTextBox.Text;
            else
                pass = PasswordPassBox.Password;

            var quary = database.Workers.Where(c => c.login == log).Select(c => c).FirstOrDefault();
            if (quary != null)
            {
                if (quary.password == pass)
                {

                    User us = new User(quary.id, quary.surname.ToString(), quary.name.ToString(), quary.role_id);


                    this.Hide();
                    aw = new AuthorizedWindow();
                    aw.Show();
                }
                else
                    MessageBox.Show("Неверный пароль");
            }
            else
                MessageBox.Show("Пользователь не найден");
        }

        private void PassCheck_Click(object sender, RoutedEventArgs e)
        {
            IsPassVix();
        }

        private void IsPassVix()
        {
            if (PassCheck.IsChecked == true)
            {
                PasswordTextBox.Visibility = Visibility.Visible;
                PasswordPassBox.Visibility = Visibility.Hidden;
                PasswordTextBox.Text = PasswordPassBox.Password;
            }
            else
            {
                PasswordTextBox.Visibility = Visibility.Hidden;
                PasswordPassBox.Visibility = Visibility.Visible;
                PasswordPassBox.Password = PasswordTextBox.Text;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void OnlyNumbers_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || e.Key == Key.Tab)
            {
                e.Handled = false;
            }
            else
                e.Handled = true;
        }

    }
}
