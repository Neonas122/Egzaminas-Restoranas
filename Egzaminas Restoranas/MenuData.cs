using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Egzaminas_Restoranas
{
    public abstract class MenuData
    {
        public List<string> MealName = new List<string>();
        public List<int> MealNamePos = new List<int>();
        public Dictionary<string, double> Meal = new Dictionary<string, double>();

        public MenuData()
        {
            int i = 0;
            using (var reader = new StreamReader("Menu.txt"))
            {
                string line = "";
                while (line != null)
                {
                    line = reader.ReadLine();
                    if (line == null) break;
                    else
                    {
                        if (line.Contains('.'))
                        {
                            string[] parts = line.Split(' ');
                            string name = null;
                            double number = 0;
                            foreach (var part in parts)
                            {
                                bool success = double.TryParse(part, out number);
                                if (success) ;
                                else
                                {
                                    name += part;
                                }
                            }
                            Meal.Add(name, number);
                        }
                        else
                        {
                            MealName.Add(line);
                            MealNamePos.Add(i - MealNamePos.Count());
                        }
                        i++;
                    }
                }
            }
        }
        abstract public void ShowMenu();
    }
}
