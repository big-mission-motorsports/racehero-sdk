using BigMission.RaceHeroSdk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigMission.RaceHeroSdk.Status
{
    public class PositionHelper
    {
        public static Racer GetCarBehindOverall(Racer car, Racer[] field)
        {
            var orderedField = field.OrderBy(r => r.PositionInRun).ToArray();
            var index = Array.IndexOf(orderedField, car);

            if ((index + 1) < orderedField.Length)
            {
                return orderedField[index + 1];
            }
            return null;
        }
    }
}
