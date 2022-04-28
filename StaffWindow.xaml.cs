﻿using System;
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
using WpfApp4.Database;
using WpfApp4.Entities;
using WpfApp4.Properties;

namespace WpfApp4
{
    /// <summary>
    /// Interaction logic for StaffWindow.xaml
    /// </summary>
    public partial class StaffWindow : Window
    {

        private const string STAFF_KEY = "viewableStaff";
        private StaffController staffController;


        public StaffWindow()
        {
            InitializeComponent();
            staffController = (StaffController)
                (Application.Current.FindResource(STAFF_KEY)
                as ObjectDataProvider).ObjectInstance;
        }

        //MINIMISE WINDOW BUTTON
        private void ButtonMinimise_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        //MAXMISE WINDOW BUTTON
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

        private void CloseAllWindows()
        {
            for (int intCounter = App.Current.Windows.Count - 1; intCounter >= 0; intCounter--)
                App.Current.Windows[intCounter].Close();
        }

        

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void addStaff_Click(object sender, RoutedEventArgs e)
        {
            int Id = Int32.Parse(textbox_id.Text);
           
            string Title = textbox_id.Text;
            string Photo = textbox_photo.Text;
            string Campus = textbox_campus.Text;
            string Email = textbox_email.Text;
            int Phone = Int32.Parse(textbox_phone.Text);
            string Room = textbox_phone.Text;
            DBAdapter.AddStaff(Id, Title, Photo, Campus, Email, Phone, Room);
            MessageBox.Show("Updated successully!");
        
        }

        private void editStaff_Click(object sender, RoutedEventArgs e)
        {
            string ID = textbox_id.Text;
            string Title = textbox_title.Text;
            string Photo = textbox_photo.Text;
            DBAdapter.EditStaff(ID, Title, Photo);

        }

        
    }
}
