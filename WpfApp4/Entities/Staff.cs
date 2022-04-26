using System.Collections.Generic;

namespace WpfApp4.Entities
{
    public enum Campus { Hobart, Launceston, Any };

    public enum Category { Academic, Technical, Admin, Casual };


    public class Staff
    {

        public int ID { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Title { get; set; }
        public Campus Campus { get; set; }
        public string Phone { get; set; }
        public string Room { get; set; }
        public string Email { get; set; }
        public string Photo { get; set; }
        public Category Category { get; set; }



        // Consultations staff member is registered in
        public List<Consultation> Consultations { get; set; }

        // Classes staff member is coordinating
        public List<UnitClass> Classes { get; set; }

        // Units staff member is teaching
        public List<Unit> Units { get; set; }

        public override string ToString()
        {
            return Title + ". " + GivenName + " " + FamilyName;
        }

    }
}
