namespace Egzaminas_Restoranas
{
    public class Program
    {
        static void Main(string[] args)
        {
            int Menu = 0;
            start();
            while (true)
            {
                if (Menu == 0) ShowTableList(ref Menu);
                if (Menu == 1) ShowSpecificTableOptions(ref Menu);

            }

        }
        static void start()
        {
            int TableAmount = ParseInput("15", 1, Int32.MaxValue);
            using (var writer = new StreamWriter("Tables.txt"))
            {
                writer.WriteLine("0");
                writer.WriteLine(TableAmount);
                for (int i = 0; i < TableAmount; i++)
                {
                    writer.WriteLine("0 0 " + i);
                }
            }
        }
        static void ShowTableList(ref int Menu)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Staliuku informacija");
            var tableData = new TableData();
            for (int i = 0; i < 15; i++)
            {
                tableData.ChangeCurrentTable(i);
                if (tableData.GetTableStatus() == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(i + 1 + " Staliukas; laisvas " + tableData.GetTableStatus());
                }
                if (tableData.GetTableStatus() == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(i + 1 + " Staliukas; Uzimtas " + tableData.GetTableStatus());
                }
                if (tableData.GetTableStatus() == 2)
                {
                    if (CheckTableReservation(tableData.GetReservationTime()) == true)
                    {
                        tableData.ChangeTableStatus(3);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(i + 1 + " Staliukas; Rezervuotas; Laikas: " + tableData.GetReservationTime());
                    }
                }
                if (tableData.GetTableStatus() == 3)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine(i + 1 + " Staliukas; Rezervuotas; laukiama uzsakymo");
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Pasirinkite staliuka");
            tableData.ChangeCurrentTable(ParseInput(Console.ReadLine(), 1, 15) - 1);
            tableData.SaveData();
            Menu = 1;

        }
        static void ShowSpecificTableOptions(ref int Menu)
        {
            var tableData = new TableData();
            string Input;
            Console.Clear();
            Console.WriteLine(tableData.GetCurrentTable() + 1 + " staliukas");
            Console.ForegroundColor = ConsoleColor.White;
            switch (tableData.GetTableStatus())
            {
                case 0:
                    Console.WriteLine("1) Sukurti uzsakyma");
                    Console.WriteLine("2) Rezervuoti");
                    Console.WriteLine("q) Grizti");
                    Input = Console.ReadLine();
                    if (Input == "1")
                    {
                        AddOrder();
                        Menu = 0;
                    }
                    if (Input == "2")
                    {
                        AddReservation();
                        Menu = 0;
                    }
                    if (Input == "q") Menu = 0;
                    break;
                case 1:
                    Console.WriteLine("1) Papildyti uzsakyma");
                    Console.WriteLine("2) Saskaita");
                    Console.WriteLine("q) Grizti");
                    Input = Console.ReadLine();
                    if (Input == "1")
                    {
                        AddOrder();
                        Menu = 0;
                    }
                    if (Input == "2")
                    {
                        ShowCheck();
                        Menu = 0;
                    }
                    if (Input == "q") Menu = 0;
                    break;
                case 2:
                    Console.WriteLine("1) Atsaukti rezervacija");
                    Console.WriteLine("q) Grizti");
                    Input = Console.ReadLine();
                    if (Input == "1")
                    {
                        tableData.ChangeTableReservation(0, "0");
                        tableData.SaveData();
                        Menu = 0;
                    }
                    if (Input == "q") Menu = 0;
                    break;
                case 3:
                    Console.WriteLine("1) Sukurti uzsakyma");
                    Console.WriteLine("2) Atsaukti rezervacija");
                    Console.WriteLine("q) Grizti");
                    Input = Console.ReadLine();
                    if (Input == "1")
                    {
                        AddOrder();
                        Menu = 0;
                    }
                    if (Input == "2")
                    {
                        tableData.ChangeTableReservation(0, "0");
                        Menu = 0;
                    }
                    if (Input == "q") Menu = 0;
                    break;
            }
        }
        static void AddOrder()
        {
            string input = "";
            var tableData = new TableData();
            tableData.ChangeTableStatus(1);
            while (input != "q")
            {
                Console.Clear();
                Console.WriteLine(tableData.GetCurrentTable() + " staliukas");
                tableData.ShowMenu();
                Console.WriteLine("q) Grizti");
                input = Console.ReadLine();
                if (input == "q") return;
                int MealID = -1;
                while (MealID <= 0) MealID = ParseInput(input, 1, tableData.Meal.Count);
                Console.WriteLine("Kiekis:");
                int MealAmount = -1;
                input = Console.ReadLine();
                while (MealAmount <= 0) MealAmount = ParseInput(input, 1, Int32.MaxValue);
                for (int i = 0; i < MealAmount; i++)
                {
                    foreach (var Table in tableData.GetOrderData())
                    {
                        if (Table.Key == tableData.GetCurrentTable())
                        {
                            Table.Value.Add(MealID);
                        }

                    }
                }
                tableData.SaveData();
                Console.Clear();
            }
        }
        static void AddReservation()
        {
            Console.Clear();
            var tableData = new TableData();
            Console.WriteLine("Rezervacijos laikas");
            tableData.ChangeTableReservation(2, Console.ReadLine());
            Console.Clear();
            Console.WriteLine("Rezervuota");
            Console.WriteLine($"staliukas: {tableData.GetCurrentTable()}; Rezervacijos laikas: {tableData.GetReservationTime()}");
            Thread.Sleep(2000);
            tableData.SaveData();
        }
        static void ShowCheck()
        {
            var tableData = new TableData();
            List<double> Mealprice = new List<double>();
            List<string> Mealname = new List<string>();
            int i = 0;
            foreach (var meal in tableData.GetMenuData())
            {
                Mealname.Add(meal.Key);
                Mealprice.Add(meal.Value);
                i++;
            }
            int[] OrderAmount = new int[Mealname.Count];
            foreach (var Table in tableData.GetOrderData())
            {
                if (Table.Key == tableData.GetCurrentTable())
                {
                    foreach (var item in Table.Value)
                    {
                        for (i = 0; i < Mealname.Count; i++)
                        {
                            if (item == i) OrderAmount[i]++;
                        }
                    }
                }
            }
            Console.Clear();
            double TotalPrice = 0;
            Console.WriteLine("Saskaita");
            Console.WriteLine(DateTime.Now.ToString("yyyy.MM.dd HH:mm"));
            for (i = 0; i< OrderAmount.Length; i++)
            {
                tableData.GetCheck(OrderAmount[i], Mealname[i], Mealprice[i]);
                TotalPrice = ReturnTotalPrice(OrderAmount[i], Mealprice[i]);
            }
            Console.WriteLine($"Bendra suma: {TotalPrice} Eur");
            Console.ReadLine();
        }
        public static bool CheckTableReservation(string ReservationTimeString)
        {
            var tableData = new TableData();
            string[] SplitTimeString = ReservationTimeString.Split(":");
            int[] ReservationTime = new int[2];
            ReservationTime[0] = ParseInput(SplitTimeString[0], 0, 23);
            ReservationTime[1] = ParseInput(SplitTimeString[1], 0, 59);

            string[] CurrentTimeString = DateTime.Now.ToString("H:mm").Split(":");
            int[] CurrentTime = new int[2];
            CurrentTime[0] = ParseInput(CurrentTimeString[0], 0, 23);
            CurrentTime[1] = ParseInput(CurrentTimeString[1], 0, 59);
            if ((CurrentTime[0] * 60 + CurrentTime[1]) >= (ReservationTime[0] * 60 + ReservationTime[1]))
            {
                return true;
            }
            else return false;
        }
        public static int ParseInput(string Input, int MinValue, int MaxValue)
        {
            bool success = false;
            int TableNumber = -1;
                success = int.TryParse(Input, out TableNumber);
                if (success == true)
                {
                    if (TableNumber <= MinValue && TableNumber >= MaxValue)
                    {
                        TableNumber = -1;
                        success = false;
                    }
                    else
                    {
                        success = true;
                    }
                }
                else TableNumber = -1;
            return TableNumber;
        }
        public static double ReturnTotalPrice(int Amount, double Price)
        {
            return Amount * Price;
        }

    }
}