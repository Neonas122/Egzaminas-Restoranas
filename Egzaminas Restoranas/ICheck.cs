using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Egzaminas_Restoranas
{
    public interface ICheck
    {
        void GetCheck(int Amount, string MealName, double MealPrice)
        {
            Console.WriteLine($"{Amount} x {MealName}; {Amount * MealPrice} Eur");
        }
    }
}
