using WpfApp4.Database;
using WpfApp4.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WpfApp4.Controller
{
    class StaffController :ObservableObject
    {
        private List<Staff> staff { get; set; }
        public List<Staff> Employees { get { return staff; } set { } }

        private ObservableCollection<Staff> viewableStaff { get; set; }
        public ObservableCollection<Staff> VisibleStaff { get { return viewableStaff; } set { } }

        public StaffController()
        {
            
            staff = DBAdapter.GetStaffDetails();
            viewableStaff = new ObservableCollection<Staff>(staff);
            
        }

        public ObservableCollection<Staff> GetViewableList()
        {
            return VisibleStaff;
        }
        public void Add(int id,string title, string campus, int phone, string room,
            string email, string photo)
        {
            DBAdapter.AddStaff(id,title,campus, phone, room, email,photo );
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