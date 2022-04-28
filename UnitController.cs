using WpfApp4.Database;
using WpfApp4.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WpfApp4.Controller
{
    class UnitController
    {

        public List<Unit> UnitList { get; set; }
        public ObservableCollection<Unit> ViewableUnits { get; set; }

        public UnitController()
        {
            UnitList = DBAdapter.GetUnitDetails(123456);
            ViewableUnits = new ObservableCollection<Unit>(UnitList);
        }

        public ObservableCollection<Unit> GetViewableUnits()
        {
            return ViewableUnits;
        }
    }
}
