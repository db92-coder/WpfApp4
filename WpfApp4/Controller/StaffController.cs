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
            
            Staff = DBAdapter.GetStaffDetails();
            ViewableStaff = new ObservableCollection<Staff>(Staff);
            
        }

        public ObservableCollection<Staff> GetViewableList()
        {
            return ViewableStaff;
        }
        public void Add(string id, string given_name, string family_name)
        {
            DBAdapter.AddStaff(id, given_name, family_name);
        }
        
    }
}
