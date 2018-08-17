using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrossSolar.Domain
{
    public class OneDayElectricity
    {
        public OneDayElectricityModel TransformToOneDayModel(DateTime day, List<OneHourElectricity> oneHour)
        {
            return new OneDayElectricityModel
            {
                DateTime = day,
                //Average per Day
                Average = oneHour.Sum(x => x.KiloWatt) / 24,
                Maximum = oneHour.Max(x => x.KiloWatt),
                Minimum = oneHour.Min(x => x.KiloWatt),
                Sum = oneHour.Sum(x => x.KiloWatt)
            };
        }
    }
}
