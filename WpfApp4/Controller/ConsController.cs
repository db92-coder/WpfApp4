using WpfApp4.Database;
using WpfApp4.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WpfApp4.Controller
{
    class ConsController
    {

        public List<Consultation> ConsList { get; set; }
        public ObservableCollection<Consultation> viewableConsultations { get; set; }
        public ObservableCollection<Consultation> VisibleConsultations { get { return viewableConsultations; } set { } }

        public ConsController()
        {
            ConsList = DBAdapter.GetConsultationDetails();
            viewableConsultations = new ObservableCollection<Consultation>(ConsList);
        }

        public ObservableCollection<Consultation> GetViewableConsultations()
        {
            return VisibleConsultations;
        }
    }
}
