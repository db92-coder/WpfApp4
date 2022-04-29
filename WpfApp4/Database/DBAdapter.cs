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

                string connectionString = String.Format("Database={0};Data Source={1};User Id={2};Password={3}"
                    , db, server, user, pass);
                conn = new MySqlConnection(connectionString);
            }
            return conn;
        }

        //ADDING STAFF DETAILS TO DB
        public static void AddStaff(int id, string title, string campus,
            int phone, string room, string email, string photo)
        {

            MySqlConnection conn = GetConnection();
            //  researcher_id = "1234671";
            //  researcher_type = "staff";
            //  researcher_given_name = "Jack";
            //  researcher_family_name = "Wilson";

            try
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("UPDATE hris.staff SET title='" + title + "',campus='" + campus + "', phone='" + phone + "',room='" + room + "',email='" + email + "',photo='" + photo + "'WHERE staff.id=" + id, conn);
                MessageBox.Show("Staff Details Added");
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
                        Title = rdr.IsDBNull(3) ? null : rdr.GetString(3),
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
            if(value != "")
            {
                return (T)Enum.Parse(typeof(T), value);
            }
            else { return default(T); }
        }


        //RETRIEVE UNIT DETAILS FROM DB
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


        //EDIT STAFF DETAILS IN DB
        internal static void EditStaff(int id, string title, string photo)
        {
            {
                
                MySqlDataReader check4 = null;
                

                MySqlConnection conn = GetConnection();
                try
                {
                    conn.Open();
                }
                catch (Exception)
                {

                }
                var check3 = new MySqlCommand("SELECT id="+ id+ " FROM staff", conn);
                MessageBox.Show("Staff " + id + " selected");
                check4 = check3.ExecuteReader();
                if (!check4.Read())
                {
                    MessageBox.Show("invalid staff id", "Error");
                    check4.Close();
                    conn.Close();
                }
                else
                {
                    check4.Close();
                    var command = new MySqlCommand("UPDATE hris.staff SET title='"+title+"', photo='"+photo+"'WHERE id="+id, conn);
                    //command.Parameters.AddWithValue("@title=", title);
                    //command.Parameters.AddWithValue("@photo=", photo);
                    

                    command.ExecuteNonQuery();
                }

                check4.Close();
                conn.Close();
            }
        }

        //EDIT UNIT COORDINATOR

        public static void EditUnit(string code, int newCoordinator)
        {
            MySqlDataReader check2 = null;
            MySqlDataReader check4 = null;

            MySqlConnection conn = GetConnection();

            try
            {
                conn.Open();
            }

            catch (Exception)
            {

            }

            var check3 = new MySqlCommand("SELECT id FROM staff WHERE id=@id", conn);
            check3.Parameters.AddWithValue("@id", newCoordinator);
            check4 = check3.ExecuteReader();

            if (!check4.Read())
            {
                MessageBox.Show("Invalid coordinator ID", "Error");
            }
            else
            {
                check4.Close();
                var check = new MySqlCommand("SELECT * FROM unit WHERE code=@code", conn);
                check.Parameters.AddWithValue("@code", code);

                check2 = check.ExecuteReader();

                if (!check2.Read())
                {
                    MessageBox.Show("Unit does not exist", "Error");
                    check2.Close();
                    conn.Close();
                }

                else
                {
                    try
                    {
                        check2.Close();
                        var command = new MySqlCommand("UPDATE unit SET coordinator=@coordinator WHERE code=@code", conn);
                        command.Parameters.AddWithValue("@coordinator", newCoordinator);
                        command.Parameters.AddWithValue("@code", code);
                        command.ExecuteNonQuery();
                    }

                    finally
                    {
                        MessageBox.Show("Unit Coordinator successfully changed");
                        check2.Close();
                        conn.Close();

                    }
                }
            }


        }

        //EDIT CLASS 
        public static void EditClass(string code,string campus, string day, int start, int end, string type, string room,string newCampus,
            string newDay, int newStart,int newEnd, string newType,string newRoom,int staff)
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
                catch (Exception)
                {

                }
                var check3 = new MySqlCommand("SELECT staff="+staff+"FROM class", conn);

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
                    var check = new MySqlCommand("SELECT * from class where " + newStart + " < end and start < " + newEnd + " and" +
                        " staff_id = " + staff + " and day = " + '"' + newDay + '"', conn);
                    MessageBox.Show("Consultation Changed");
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
                        var check5 = new MySqlCommand("SELECT * FROM class where staff_id=@staff AND day=@day AND start=@start AND end=@end", conn);
                        check5.Parameters.AddWithValue("@staff", staff);
                        check5.Parameters.AddWithValue("@day", day);
                        check5.Parameters.AddWithValue("@start", start);
                        check5.Parameters.AddWithValue("@end", end);
                        check6 = check5.ExecuteReader();

                        if (!check6.Read())
                        {
                            MessageBox.Show("Class does not exist", "error");
                            check6.Close();
                            conn.Close();
                        }

                        else
                        {
                            try
                            {
                                check6.Close();
                                var command = new MySqlCommand("UPDATE consultation SET day=@newDay, start=@newStart, end=@newEnd WHERE staff_id=@staff AND day=@day AND start=@start AND end=@end", conn);
                                command.Parameters.AddWithValue("@day", day);
                                command.Parameters.AddWithValue("@start", start);
                                command.Parameters.AddWithValue("@end", end);
                                command.Parameters.AddWithValue("@newday", newDay);
                                command.Parameters.AddWithValue("@newstart", newStart);
                                command.Parameters.AddWithValue("@newend", newEnd);
                                command.Parameters.AddWithValue("@staff", staff);

                                MessageBox.Show("Class Details Updated");
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
        //REMOVE STAFF MEMBER FROM DB
        internal static void RemoveStaff(string id)
        {
            {
                
                MySqlDataReader check4 = null;
                
                MySqlConnection conn = GetConnection();
                try
                {
                    conn.Open();
                }
                catch (Exception )
                {

                }
                var check3 = new MySqlCommand("SELECT id FROM staff where id=" + id, conn);

                check4 = check3.ExecuteReader();
                if (!check4.Read())
                {
                    MessageBox.Show("invalid staff id", "Error");
                    check4.Close();
                    conn.Close();
                }
                else
                {
                    check4.Close();
                    var command = new MySqlCommand("DELETE FROM staff WHERE id="+id, conn);
                    
                    command.Parameters.AddWithValue("@id=", id);

                    command.ExecuteNonQuery();
                }

                check4.Close();
                conn.Close();
            }
        }

        //GET ALL STAFF DETAILS FROM DB
        public static List<Staff> GetFullStaffDetails()
        {
            MySqlDataReader rdr = null;
            MySqlConnection conn = GetConnection();

            var staff = new List<Staff>();

            try
            {
                conn.Open();
                var command = new MySqlCommand("SELECT id, given_name, family_name,title,campus,phone,room,email,category FROM staff", conn);
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
                        Campus = ParseEnum<Campus>(rdr.GetString(4)),
                        Room = rdr.IsDBNull(5) ? null : rdr.GetString(5),
                        Phone = rdr.GetString(6),
                        Email =  rdr.GetString(7),
                        //Photo = rdr.IsDBNull(8) ? null : rdr.GetString(8)
                        Category = ParseEnum<Category>(rdr.GetString(8))
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

        //RETRIEVE CONSULT DETAILS FROM DB
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


        //GET CLASS DETAILS FROM DB
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
            catch (Exception)
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
                            MessageBox.Show("Consultation Added");
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
                catch (Exception)
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
                    MessageBox.Show("Consultation Changed");
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
            catch (Exception)
            {

            }
            var check3 = new MySqlCommand("SELECT id FROM staff where id=" + id, conn);
            MessageBox.Show("Consultation Removed");
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
            MySqlConnection conn = GetConnection();
            try
            {
                conn.Open();
            }
            catch (Exception)
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
                        MessageBox.Show("Timetable overlap", "error");
                        check6.Close();
                        conn.Close();
                    }
                    else
                    {
                        try
                        {
                            check6.Close();
                            var command = new MySqlCommand("INSERT INTO class (unit_code, campus, day, start, end, type, room, staff)" +
                                "VALUES('" + code + "','" + campus + "','" + day + "','" + start + "','" + end + "','" + type + "','" + room + "','" + staff + "')", conn);
                            MessageBox.Show("Class Details Added");
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

        public static void AddUnit(string code, string title, int coordinator)
        {
            MySqlDataReader check2 = null;
            MySqlConnection conn = GetConnection();
            try
            {
                conn.Open();
            }
            catch (Exception)
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
                    MessageBox.Show("Unit Added");
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

