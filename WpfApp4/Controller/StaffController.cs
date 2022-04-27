using WpfApp4.Database;
using WpfApp4.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WpfApp4.Controller
{
    class StaffController : ObservableObject
    {
        public List<Staff> Staff { get; set; }
        public ObservableCollection<Staff> ViewableStaff { get; set; }

        public StaffController()
        {
            
            Staff = DBAdapter.GetFullStaffDetails();
            ViewableStaff = new ObservableCollection<Staff>(Staff);
            
        }

        public ObservableCollection<Staff> GetViewableList()
        {
            return ViewableStaff;
        }
        public void Add(int id, string title, string photo, string campus, 
            string email, int phone, string room)
        {
            DBAdapter.AddStaff(id, title, photo,campus,email,phone,room);
        }
        
        public void AddCons(int sid, string day, int start, int end)
        {
            DBAdapter.AddConsultation(sid, day, start, end);
        }

        public void EditCons(int sid, string day, int start, int end, string newday, int newstart, int newend)
        {
            DBAdapter.EditConsultation(sid, day, start, end, newday, newstart, newend);

        }

        public void RemoveCons(int id, string day, int start, int end)
        {
            DBAdapter.RemoveConsultation(id, day, start, end);
        }
    }
}