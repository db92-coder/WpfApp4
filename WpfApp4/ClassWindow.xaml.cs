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
    /// Interaction logic for ClassWindow.xaml
    /// </summary>
    public partial class ClassWindow : Window
    {
        public ClassWindow()
        {
            InitializeComponent();
        }

        //CLOSE WINDOW METHOD
        private void ButtonMinimise_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        //MAXIMISE WINDOW
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
        
        //CLOSE WINDOW
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            App.Current.MainWindow = main;
            this.Close();
            //main.Show();
        }

        private void classAdd_button_Click(object sender, RoutedEventArgs e)
        {
            string Code = textbox_unit.Text;
            string Campus = textbox_campus.Text;
            string Day = textbox_day.Text;
            int Start = Int32.Parse(textbox_StartTime.Text);
            int End = Int32.Parse(textbox_EndTime.Text);
            string Type = textbox_type.Text;
            string Room = textbox_room.Text;
            int Staff = Int32.Parse(textbox_StaffClass.Text);
            DBAdapter.AddClass(Code, Campus, Day, Start, End, Type, Room,Staff);
            
        }

        private void classEdit_Button_Click(object sender, RoutedEventArgs e)
        {
            string Code = textbox_unit.Text;
            string Campus = textbox_campus.Text;
            string Day = textbox_day.Text;
            int Start = Int32.Parse(textbox_StartTime.Text);
            int End = Int32.Parse(textbox_EndTime.Text);
            string Type = textbox_type.Text;
            string Room = textbox_room.Text;
            string newCampus = textbox_newCampus.Text;
            string newDay = textbox_newDay.Text;
            int newStart = Int32.Parse(textbox_newStart.Text);
            int newEnd = int.Parse(textbox_newEnd.Text);
            string newType = textbox_newType.Text;
            string newRoom = textbox_newRoom.Text;
            int Staff = Int32.Parse(textbox_StaffClass.Text);

            DBAdapter.EditClass(Code, Campus, Day, Start, End, Type, Room,
                newCampus, newDay,newStart,newEnd,newType,newRoom,Staff);
        }
    }
}
