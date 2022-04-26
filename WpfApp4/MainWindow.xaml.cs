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

namespace WpfApp4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //private const string STAFF_KEY = "viewableStaff";
        //private StaffController staffController;


        public MainWindow()
        {
            InitializeComponent();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                    DragMove();
            }
        }

        private void ButtonMinimise_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

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

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }


        //STAFF BUTTON TO OPEN POPUP
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StaffWindow staffWindow = new StaffWindow();
            staffWindow.Show();
        }

        //CLASS BUTTON TO OPEN POPUP
        private void ClassButton_Click(object sender, RoutedEventArgs e)
        {
            ClassWindow classWindow = new ClassWindow();
            classWindow.Show();
        }

        //CONSULTATION BUTTON TO OPEN POPUP
        private void ConsultationButton_Click(object sender, RoutedEventArgs e)
        {
            ConsultationWindow consultationWindow = new ConsultationWindow();
            consultationWindow.Show();
        }


        //UNIT BUTTON TO OPEN POPUP
        private void UnitButton_Click(object sender, RoutedEventArgs e)
        {
            UnitWindow unitWindow = new UnitWindow();
            unitWindow.Show();
        }




        //private void DisplayLoginScreen()
        //{
        //    LoginWindow frm = new LoginWindow();

        //    frm.Owner = this;
        //    frm.ShowDialog();
        //    if (frm.DialogResult.HasValue && frm.DialogResult.Value)
        //        MessageBox.Show("User Logged In");
        //    else
        //        this.Close();
        //}
    }
}
