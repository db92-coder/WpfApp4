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
using WpfApp4.Database;

namespace WpfApp4
{
    /// <summary>
    /// Interaction logic for ConsultationWindow.xaml
    /// </summary>
    public partial class ConsultationWindow : Window
    {
        public ConsultationWindow()
        {
            InitializeComponent();
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

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            int sid = Int32.Parse(textbox_id.Text);
            string day = textbox_day.Text;
            int start = Int32.Parse(textbox_start.Text);
            int finish = Int32.Parse(textbox_finish.Text);

            DBAdapter.AddConsultation(sid, day, start, finish);
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            int sid = Int32.Parse(textbox_id.Text);
            string day = textbox_day.Text;
            int start = Int32.Parse(textbox_start.Text);
            int finish = Int32.Parse(textbox_finish.Text);
            string newDay = textbox_newDay.Text;
            int newStart = Int32.Parse(textbox_newStart.Text);
            int newEnd = Int32.Parse(textbox_newEnd.Text);
            DBAdapter.EditConsultation(sid, day, start, finish, newDay,newStart,newEnd);
        }

        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(textbox_id.Text) || String.IsNullOrEmpty(textbox_day.Text) || String.IsNullOrEmpty(textbox_start.Text) || String.IsNullOrEmpty(textbox_finish.Text))
            {
                MessageBox.Show("Please fill all textboxes for valid removal");
            }

            else
            {
                int sid = Int32.Parse(textbox_id.Text);
                string day = textbox_day.Text;
                int start = Int32.Parse(textbox_start.Text);
                int finish = Int32.Parse(textbox_finish.Text);
                DBAdapter.RemoveConsultation(sid, day, start, finish);
            }
        }
    }
}
