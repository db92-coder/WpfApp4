using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp4.Controller;

namespace WpfApp4
{
    /// <summary>
    /// Interaction logic for ConsultationWindow.xaml
    /// </summary>
    public partial class ConsultationWindow : Window
    {
        private const string STAFF_LIST_KEY = "viewableStaff";
        private StaffController staffController;
        public ConsultationWindow()
        {
            InitializeComponent();
            staffController = (StaffController)(Application.Current.FindResource(STAFF_LIST_KEY) as ObjectDataProvider).ObjectInstance;
        }

        //MINIMISE WINDOW BUTTON
        private void ButtonMinimise_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        //MAXIMISE WINDOW BUTTON
        private void WindowStateButton_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow.WindowState != WindowState.Maximized)
            {
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            }
            else
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
        }

        //CLOSE WINDOW BUTTON
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            App.Current.MainWindow = main;
            this.Close();
            //main.Show();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void addCons_Click(object sender, RoutedEventArgs e)
        {

        }

        private void removeCons_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(textbox_ID3.Text) || String.IsNullOrEmpty(textbox_Day3.Text) || String.IsNullOrEmpty(textbox_Start3.Text) || String.IsNullOrEmpty(textbox_Finish3.Text))
            {
                MessageBox.Show("Please fill all textboxes for valid removal");
            }

            else
            {
                int id = Int32.Parse(textbox_ID3.Text);
                string day = textbox_Day3.Text;
                int start = Int32.Parse(textbox_Start3.Text);
                int finish = Int32.Parse(textbox_Finish3.Text);
                staffController.RemoveCons(id, day, start, finish);
            }
            
        }
    }
}
