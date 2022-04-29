using WpfApp4.Entities;
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
    /// Interaction logic for UnitClass.xaml
    /// </summary>
    public partial class UnitWindow : Window
    {
        public UnitWindow()
        {
            InitializeComponent();
        }

        public string UnitCode { get; internal set; }
        public Campus Campus { get; internal set; }
        public DayOfWeek Day { get; internal set; }
        public TimeSpan Start { get; internal set; }
        public TimeSpan End { get; internal set; }
        public ClassType Type { get; internal set; }
        public string Room { get; internal set; }
        public Staff Staff { get; internal set; }


        // Check if class times overlap for a given day
        public bool Overlap(DateTime dateTime)
        {
            return dateTime.DayOfWeek == Day && dateTime.TimeOfDay >= Start && dateTime.TimeOfDay < End;
        }

        public override string ToString()
        {
            return UnitCode + ", " + Campus + " campus, " + Day + " " + Start + " until " + End + ", " + Type + " in room " + Room;
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

        private void AddUnit_Button_Click(object sender, RoutedEventArgs e)
        {

            if (String.IsNullOrEmpty(textbox_unitcode.Text) || String.IsNullOrEmpty(textbox_title.Text))
            {
                MessageBox.Show("Please fill all information to add a unit", "Error");
            }
            else if(String.IsNullOrEmpty(textbox_coordinator.Text))
            {
                MessageBox.Show("Please enter a valid staff ID", "Error");
            }
            else
            {
                try
                {
                    string code = textbox_unitcode.Text;
                    string title = textbox_title.Text;
                    int coordinator = Int32.Parse(textbox_coordinator.Text);
                    DBAdapter.AddUnit(code, title, coordinator);
                    UnitGrid.Items.Refresh();
                }
                catch
                {
                    MessageBox.Show("Please check all inputs are correct and try again");
                }
            }
            
        }

        private void EditUnit_Button_Click(object sender, RoutedEventArgs e)
        {   
            if (String.IsNullOrEmpty(textbox_NewCode.Text))
            {
                MessageBox.Show("Please enter a unit code to change unit coordinator", "Error");
            }
            else if(String.IsNullOrEmpty(textbox_newCoordinator.Text))
            {
                MessageBox.Show("Please enter a valid ID for new coordinator");
            }
            else
            {
                try
                {
                    string code = textbox_NewCode.Text;
                    int coordinator = Int32.Parse(textbox_newCoordinator.Text);

                    DBAdapter.EditUnit(code, coordinator);
                    
                }

                catch
                {
                    MessageBox.Show("Please check all inputs are correct and try again", "Error");
                }
                
            }
           
        }



        //private void EditUnit_Button_Click(object sender, RoutedEventArgs e)
        //{
        //    string code = textbox_unitcode.Text;
        //    string title = textbox_title.Text;
        //    int id = Int32.Parse(textbox_coordinator.Text);
        //    int newId = Int32.Parse(textbox_newCoordinator.Text);
        //}
    }
}

