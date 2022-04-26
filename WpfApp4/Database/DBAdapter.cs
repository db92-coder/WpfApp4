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
                
                string connectionString = String.Format("Database={0};Data Source={1};User Id={2};Password={3}", db, server, user, pass);
                conn = new MySqlConnection(connectionString);
            }
            return conn;
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

        public static void AddConsultation(int sid, string day, int start, int end)
        {
            MySqlDataReader check2 = null;
            MySqlDataReader check4 = null;
            MySqlDataReader check6 = null;
            MySqlConnection conn = GetConnection();
            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {

            }
            var check3 = new MySqlCommand("SELECT id FROM staff where id=" + sid, conn);
            check4 = check3.ExecuteReader();
            if (!check4.Read())
            {
                MessageBox.Show("invalid coordinator id", "Error");
                check4.Close();
                conn.Close();
            }
            else
            {
                check4.Close();
                var check = new MySqlCommand("Select * from consultation where " + start + " < end and start < " + end + " and staff_id = " + sid + " and day = " + '"' + day + '"', conn);
                check2 = check.ExecuteReader();
                if (check2.Read())
                {
                    MessageBox.Show("Timetable overlap with consultation", "error");
                    check2.Close();
                    conn.Close();
                }
                else
                {

                    check2.Close();
                    var check5 = new MySqlCommand("Select * from class where " + start + " < end and start < " + end + " and staff = " + sid + " and day = " + '"' + day + '"', conn);
                    check6 = check5.ExecuteReader();
                    if (check6.Read())
                    {
                        MessageBox.Show("Timetable overlap with class", "error");
                        check6.Close();
                        conn.Close();
                    }
                    else
                    {
                        try
                        {
                            check6.Close();
                            var command = new MySqlCommand("INSERT INTO consultation (staff_id, day, start, end) VALUES('" + sid + "','" + day + "','" + start + "','" + end + "')", conn);

                            command.ExecuteNonQuery();

                        }
                        finally
                        {
                            check6.Close();
                            conn.Close();
                        }
                    }
                }
            }
        }
        public static void EditConsultation(int sid, string day, int start, int end, string newday, int newstart, int newend)
        {
            {
                MySqlDataReader check2 = null;
                MySqlDataReader check4 = null;
                MySqlDataReader check6 = null;

                MySqlConnection conn = GetConnection();
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {

                }
                var check3 = new MySqlCommand("SELECT id FROM staff where id=" + sid, conn);

                check4 = check3.ExecuteReader();
                if (!check4.Read())
                {
                    MessageBox.Show("invalid coordinator id", "Error");
                    check4.Close();
                    conn.Close();
                }
                else
                {
                    check4.Close();
                    var check = new MySqlCommand("SELECT * from consultation where " + newstart + " < end and start < " + newend + " and staff_id = " + sid + " and day = " + '"' + newday + '"', conn);
                    check2 = check.ExecuteReader();
                    if (check2.Read())
                    {
                        MessageBox.Show("Timetable overlap or consultation does not exist to be edited", "error");
                        check2.Close();
                        conn.Close();
                    }
                    else
                    {
                        check2.Close();
                        var check5 = new MySqlCommand("SELECT * FROM consultation where staff_id=@id AND day=@day AND start=@start AND end=@end", conn);
                        check5.Parameters.AddWithValue("@id", sid);
                        check5.Parameters.AddWithValue("@day", day);
                        check5.Parameters.AddWithValue("@start", start);
                        check5.Parameters.AddWithValue("@end", end);
                        check6 = check5.ExecuteReader();

                        if (!check6.Read())
                        {
                            MessageBox.Show("Consultation does not exist", "error");
                            check6.Close();
                            conn.Close();
                        }

                        else
                        {
                            try
                            {
                                check6.Close();
                                var command = new MySqlCommand("UPDATE consultation SET day=@newday, start=@newstart, end=@newend WHERE staff_id=@sid AND day=@day AND start=@start AND end=@end", conn);
                                command.Parameters.AddWithValue("@day", day);
                                command.Parameters.AddWithValue("@start", start);
                                command.Parameters.AddWithValue("@end", end);
                                command.Parameters.AddWithValue("@newday", newday);
                                command.Parameters.AddWithValue("@newstart", newstart);
                                command.Parameters.AddWithValue("@newend", newend);
                                command.Parameters.AddWithValue("@sid", sid);


                                command.ExecuteNonQuery();

                            }
                            finally
                            {
                                check6.Close();
                                conn.Close();
                            }
                        }


                    }
                }
            }
        }

        public static void RemoveConsultation(int id, string day, int start, int end)
        {
            MySqlDataReader check2 = null;
            MySqlDataReader check4 = null;

            MySqlConnection conn = GetConnection();
            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {

            }
            var check3 = new MySqlCommand("SELECT id FROM staff where id=" + id, conn);
            check4 = check3.ExecuteReader();
            if (!check4.Read())
            {
                MessageBox.Show("invalid coordinator id", "Error");
                check4.Close();
                conn.Close();
            }
            else
            {
                check4.Close();
                var check = new MySqlCommand("SELECT * FROM consultation where staff_id=@id AND day=@day AND start=@start AND end=@end", conn);
                check.Parameters.AddWithValue("@id", id);
                check.Parameters.AddWithValue("@day", day);
                check.Parameters.AddWithValue("@start", start);
                check.Parameters.AddWithValue("@end", end);


                check2 = check.ExecuteReader();
                if (!check2.Read())
                {
                    MessageBox.Show("Consultation does not exist", "Error");
                    check2.Close();
                    conn.Close();
                }
                else
                {

                    try
                    {
                        check2.Close();
                        var command = new MySqlCommand("DELETE FROM consultation WHERE staff_id=@id AND day=@day AND start=@start AND end=@end", conn);
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@day", day);
                        command.Parameters.AddWithValue("@start", start);
                        command.Parameters.AddWithValue("@end", end);
                        command.ExecuteNonQuery();

                    }
                    finally
                    {
                        check2.Close();
                        conn.Close();
                    }
                }
            }
        }

        public static void AddClass(string code, string campus, string day, int start, int end, string type, string room, int staff)
        {
            MySqlDataReader check2 = null;
            MySqlDataReader check4 = null;
            MySqlDataReader check6 = null;
            MySqlDataReader check8 = null;
            MySqlConnection conn = GetConnection();
            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {

            }
            var check = new MySqlCommand("SELECT id FROM staff where id=" + staff, conn);
            check2 = check.ExecuteReader();

            if (!check2.Read())
            {
                MessageBox.Show("invalid staff id", "Error");
                check2.Close();
                conn.Close();
            }
            else
            {
                check2.Close();
                var check3 = new MySqlCommand("SELECT code FROM unit where code=" + '"' + code + '"', conn);
                check4 = check3.ExecuteReader();
                if (!check4.Read())
                {
                    MessageBox.Show("invalid unit code", "Error");
                    check4.Close();
                    conn.Close();
                }
                else
                {
                    check4.Close();
                    var check5 = new MySqlCommand("Select * from class where " + start + " < end and start < " + end + " and staff = " + staff + " and day = " + '"' + day + '"', conn);
                    check6 = check5.ExecuteReader();
                    if (check6.Read())
                    {
                        MessageBox.Show("Timetable overlap with class", "error");
                        check6.Close();
                        conn.Close();
                    }
                    else
                    {
                        check6.Close();
                        var check7 = new MySqlCommand("Select * from consultation where " + start + " < end and start < " + end + " and staff_id = " + staff + " and day = " + '"' + day + '"', conn);
                        check8 = check7.ExecuteReader();
                        if (check8.Read())
                        {
                            MessageBox.Show("Timetable overlap with consultation", "error");
                            check8.Close();
                            conn.Close();
                        }
                        else
                        {
                            try
                            {
                                check6.Close();
                                var command = new MySqlCommand("INSERT INTO class (unit_code, campus, day, start, end, type, room, staff) VALUES('" + code + "','" + campus + "','" + day + "','" + start + "','" + end + "','" + type + "','" + room + "','" + staff + "')", conn);

                                command.ExecuteNonQuery();

                            }
                            finally
                            {
                                check6.Close();
                                conn.Close();
                            }
                        }
                    }
                }
            }
        }

        public static void AddUnit(string code, string title, int coordinator)
        {
            MySqlDataReader check2 = null;
            MySqlConnection conn = GetConnection();
            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {

            }
            var check = new MySqlCommand("SELECT id FROM staff where id=" + coordinator, conn);
            check2 = check.ExecuteReader();
            if (!check2.Read())
            {
                MessageBox.Show("invalid coordinator id", "Error");
                check2.Close();
                conn.Close();
            }
            else
            {
                try
                {
                    check2.Close();
                    var command = new MySqlCommand("INSERT INTO unit (code, title, coordinator) VALUES('" + code + "','" + title + "','" + coordinator + "')", conn);

                    command.ExecuteNonQuery();

                }
                finally
                {
                    check2.Close();
                    conn.Close();
                }

            }


        }
    }
}

