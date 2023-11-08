using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Egzaminas_Restoranas
{
    public class TableData : MenuData, ICheck
    {
        private int CurrentTable { get; set; }
        private int TableAmount { get; set; }
        private int[] TableStatus = new int[15];
        private string[] ReservationTime = new string[15];
        private Dictionary<int, List<int>> TableOrder = new Dictionary<int, List<int>>();

        public TableData(){
            using (var reader = new StreamReader("Tables.txt"))
            {
                CurrentTable = Convert.ToInt32(reader.ReadLine());
                TableAmount = Convert.ToInt32(reader.ReadLine());
                for (int i = 0; i < TableAmount; i++)
                {
                    string fileline = reader.ReadLine();
                    string[] line = fileline.Split(',', ' ');
                    TableStatus[i] = Convert.ToInt32(line[0]);
                    ReservationTime[i] = line[1];
                    if (line.Length > 2)
                    {
                        List<int> OrderList = new List<int>();
                        for (int j = 3; j < line.Length; j++)
                        {
                            OrderList.Add(Convert.ToInt32(line[j]));
                        }
                        TableOrder.Add(Convert.ToInt32(line[2]), OrderList);
                    }
                    else TableOrder.Add(Convert.ToInt32(line[2]), new List<int>());
                }
            }
        }
        public void SaveData()
        {
            using (var writer = new StreamWriter("Tables.txt"))
            {
                writer.WriteLine(CurrentTable);
                writer.WriteLine(TableAmount);
                int i = 0;
                foreach (var item in TableOrder)
                {
                    writer.Write(TableStatus[i] + " " + ReservationTime[i] + " " + item.Key);
                    for(int h = 0; h < item.Value.Count; h++)
                    {
                        writer.Write(" " + item.Value[h]);
                    }
                    writer.WriteLine();
                    i++;
                }
            }
        }
        public void ChangeCurrentTable(int currentTable)
        {
            CurrentTable = currentTable;
        }
        public void ChangeTableStatus(int Status)
        {
            TableStatus[CurrentTable] = Status;
        }
        public void ChangeTableReservation(int status, string time)
        {
            ReservationTime[CurrentTable] = time;
            TableStatus[CurrentTable] = status;
        }
        public void GetCheck(int Amount, string MealName, double MealPrice)
        {
            if (Amount > 0) Console.WriteLine($"{Amount} x {MealName}; {Amount * MealPrice} Eur");
        }
        public override void ShowMenu()
        {
            int i = 0;
            int j = 0;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Meniu:");
            foreach (var item in Meal)
            {
                if (j < MealNamePos.Count() && i == MealNamePos[j])
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(MealName[j]);
                    j++;
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(i + 1 + ") " + item.Key + " " + item.Value + " Eur");
                i++;
            }
        }

        public string GetReservationTime() => ReservationTime[CurrentTable];
        public int GetTableStatus() => TableStatus[CurrentTable];
        public int GetCurrentTable() => CurrentTable;
        public int GetTableAmount() => TableAmount;
        public Dictionary<int, List<int>> GetOrderData() => TableOrder;
        public Dictionary<string, double> GetMenuData() => Meal;
    }
}
