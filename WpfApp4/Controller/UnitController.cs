using WpfApp4.Database;
using WpfApp4.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WpfApp4.Controller
{
    class UnitController
    {

        public List<Unit> UnitList { get; set; }
        public ObservableCollection<Unit> viewableUnits { get; set; }
        public ObservableCollection<Unit> VisibleUnits { get { return viewableUnits; } set { } }

        public UnitController()
        {
            UnitList = DBAdapter.GetUnitDetails();
            viewableUnits = new ObservableCollection<Unit>(UnitList);
        }

        public ObservableCollection<Unit> GetViewableUnits()
        {
            return VisibleUnits;
        }
    }
}
