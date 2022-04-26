using WpfApp4.Entities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Windows;

namespace WpfApp4.Database
{
    class DBAdapter
    {

        private const string db = "hris";
        private const string user = "kit206g14a";
        private const string pass = "group14a";
        private const string server = "alacritas.cis.utas.edu.au";

        private static MySqlConnection conn = null;

        private static bool reportingErrors = false;
        private static MySqlConnection GetConnection()
        {
            if (conn == null)
            {
                //Note: This approach is not thread-safe
                string connectionString = String.Format("Database={0};Data Source={1};User Id={2};Password={3}", db, server, user, pass);
                conn = new MySqlConnection(connectionString);
            }
            return conn;
        }

        public static void AddStaff(string ID, string GivenName, string FamilyName)
        {
            MySqlConnection conn = GetConnection();

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO staff (id, givenname, familyname) VALUES ('" + ID + "','" + GivenName + "','" + FamilyName + "')", conn);
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                ReportError("Adding staff", e);
            }
            finally
            {
                conn.Close();

            }
        }
        //Reporting error method
        private static void ReportError(string msg, Exception e)
        {
            if (reportingErrors)
            {
                MessageBox.Show("An error occurred while " + msg + ". Try again later.\n\nError Details:\n" + e,
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        // Convert string to enum
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Staff id of unit coordinator</param>
        /// <returns>List of units coordinated by given staff member</returns>
        public static List<Unit> GetUnitDetails(int id)
        {
            MySqlDataReader rdr = null;
            MySqlConnection conn = GetConnection();
            var unitDetails = new List<Unit>();

            try
            {
                conn.Open();
                var command = new MySqlCommand("SELECT code, title, coordinator FROM unit WHERE coordinator=?id", conn);
                command.Parameters.AddWithValue("id", id);
                rdr = command.ExecuteReader();

                while (rdr.Read())
                {

                    // fill in additional data
                    unitDetails.Add(new Unit
                    {
                        Code = rdr.GetString(0),
                        Title = rdr.GetString(1),

                    });


                }

            }
            catch (MySqlException e)
            {
                Console.WriteLine("Error Generated. Details: " + e.ToString());
            }
            finally
            {
                rdr.Close();
                conn.Close();
            }

            return unitDetails;
        }

        /// <summary>
        ///  Gets basic staff details to populate the staff list view of the GUI
        /// </summary>
        /// <returns></returns>
        public static List<Staff> GetStaffDetails()
        {
            MySqlDataReader rdr = null;
            MySqlConnection conn = GetConnection();

            var staff = new List<Staff>();

            try
            {
                conn.Open();
                var command = new MySqlCommand("SELECT id, given_name, family_name, title, campus FROM staff", conn);
                rdr = command.ExecuteReader();

                while (rdr.Read())
                {

                    // fill in additional data
                    staff.Add(new Staff
                    {
                        ID = rdr.GetInt32(0),
                        GivenName = rdr.GetString(1),
                        FamilyName = rdr.GetString(2),
                        Title = rdr.GetString(3),
                        Campus = ParseEnum<Campus>(rdr.GetString(4))
                    });

                }

            }

            catch (MySqlException e)
            {
                Console.WriteLine("Error Generated. Details: " + e.ToString());
            }

            finally
            {
                rdr.Close();
                conn.Close();
            }

            return staff;


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">ID number of given staff member</param>
        /// <returns>Staff object with all information necessary for add and edit staff detail functions</returns>
        public static Staff GetFullStaffDetails(Staff staff)
        {

            MySqlDataReader rdr = null;
            MySqlConnection conn = GetConnection();

            Staff staffDetails = null; // This will be the staff member returned

            try
            {
                conn.Open();
                var command = new MySqlCommand("SELECT * FROM staff where id=?id", conn);
                command.Parameters.AddWithValue("id", staff.ID);

                rdr = command.ExecuteReader();
                rdr.Read();

                staffDetails = new Staff
                {
                    ID = staff.ID,
                    GivenName = rdr.GetString(1),
                    FamilyName = rdr.GetString(2),
                    Title = rdr.GetString(3),
                    Campus = ParseEnum<Campus>(rdr.GetString(4)),
                    Room = rdr.GetString(5),
                    Phone = rdr.GetString(6),
                    Email = rdr.GetString(7),
                    Photo = rdr.GetString(8),
                    Category = ParseEnum<Category>(rdr.GetString(9))


                };

            }

            catch (MySqlException e)
            {
                Console.WriteLine("Error: Cannot connect to database " + e);
            }

            finally
            {
                rdr.Close();
                conn.Close();
            }

            return staffDetails;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of staff member attending consultation</param>
        /// <returns>List of consultations of staff members based on id</returns>
        public static List<Consultation> GetConsultationDetails(int id)
        {
            MySqlDataReader rdr = null;
            MySqlConnection conn = GetConnection();
            var consultations = new List<Consultation>();

            try
            {
                conn.Open();
                var command = new MySqlCommand("SELECT staff_id, day, start, end FROM consultation where staff_id=?id", conn);
                command.Parameters.AddWithValue("id", id);
                rdr = command.ExecuteReader();


                while (rdr.Read())
                {

                    // fill in additional data
                    consultations.Add(new Consultation
                    {
                        Day = ParseEnum<DayOfWeek>(rdr.GetString(1)),
                        Start = rdr.GetTimeSpan(2),
                        End = rdr.GetTimeSpan(3)

                    });


                }

            }
            finally
            {
                rdr.Close();
                conn.Close();
            }

            return consultations;
        }

        public static List<UnitClass> GetClassDetails(int id)
        {
            MySqlDataReader rdr = null;
            MySqlConnection conn = GetConnection();

            var classList = new List<UnitClass>();

            try
            {
                conn.Open();
                var command = new MySqlCommand("SELECT unit_code, campus, day, start, end, type, room FROM class where staff=?id", conn);
                command.Parameters.AddWithValue("id", id);
                rdr = command.ExecuteReader();

                while (rdr.Read())
                {

                    // fill in additional data
                    classList.Add(new UnitClass
                    {
                        UnitCode = rdr.GetString(0),
                        Campus = ParseEnum<Campus>(rdr.GetString(1)),
                        Day = ParseEnum<DayOfWeek>(rdr.GetString(2)),
                        Start = rdr.GetTimeSpan(3),
                        End = rdr.GetTimeSpan(4),
                        Type = ParseEnum<ClassType>(rdr.GetString(5)),
                        Room = rdr.GetString(6)

                    });

                }

            }
            finally
            {
                rdr.Close();
                conn.Close();
            }

            return classList;


        }
    }
}

