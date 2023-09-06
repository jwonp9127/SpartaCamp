using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace FirstProject
{
    internal class Program
    {
        struct Player
        {
            public int Level { get; set; }
            public string Name { get; set; }
            public string CharClass { get; set; }
            public double CP { get; set; } // Conbat Power
            public double DEF { get; set; } // Defense
            public int HP { get; set; } // Health Point
            public int Gold { get; set; }
        }
        struct Item
        {
            public bool IsEquip { get; set; }
            public bool IsHave { get; set; }
            public string Name { get; }
            public double CP { get; }
            public double DEF { get; }
            public string Description { get; }
            public int Gold { get; }
            public Item(bool IsEquip, bool IsHave, string Name, double CP, double DEF, string Description, int Gold)
            {
                this.IsEquip = IsEquip;
                this.IsHave = IsHave;
                this.Name = Name;
                this.CP = CP;
                this.DEF = DEF;
                this.Description = Description;
                this.Gold = Gold;
            }
        }

        static void DisplayMain(Player player, List<Item> items)
        {
            Console.Clear();
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.\n");

            ColorWrite("1. ", "DarkRed");
            Console.WriteLine("상태 보기");
            ColorWrite("2. ", "DarkRed");
            Console.WriteLine("인벤토리");
            ColorWrite("3. ", "DarkRed");
            Console.WriteLine("상점\n");

            List<string> validSelection = new List<string>{ "1", "2", "3"};
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            string selectEnter = Console.ReadLine();
            if (IsValidInput(selectEnter, validSelection) == false)
                DisplayMain(player, items);
            switch (selectEnter)
            {
                case "1":
                    DisplayMyInformation(player, items); break;
                case "2":
                    DisplayInventory(player, items); break;
                case "3":
                    Market(player, items); break;
            }   
        }

        static void DisplayMyInformation(Player player, List<Item>items)
        {
            Console.Clear();
            double plusCP = 0;
            double plusDEF = 0;

            foreach(Item item in items)
            {
                if (item.IsEquip == false)  continue;

                plusCP += item.CP;
                plusDEF += item.DEF;
            }

            ColorWrite("상태 보기", "DarkRed");
            Console.WriteLine();
            Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");
            Console.Write("Lv. ");
            ColorWrite(player.Level.ToString(), "DarkRed");
            Console.WriteLine();
            Console.WriteLine($"{player.Name} ( {player.CharClass} )");
            Console.Write("공격력 : ");
            ColorWrite(player.CP.ToString(),"DarkRed");
            if(plusCP != 0)  ColorWrite(" ( " + ((plusCP > 0) ? "+" + plusCP : plusCP) + " )", "DarkRed");
            Console.WriteLine();
            Console.Write("방어력 : ");
            ColorWrite(player.DEF.ToString(), "DarkRed");
            if (plusDEF != 0) ColorWrite(" ( " + ((plusDEF > 0) ? "+" + plusDEF : plusDEF) + " )", "DarkRed");
            Console.WriteLine();
            Console.Write("체  력 : ");
            ColorWrite(player.HP.ToString(), "DarkRed");
            Console.WriteLine();
            Console.Write("Gold: ");
            ColorWrite(player.Gold.ToString(), "DarkRed");
            Console.WriteLine("G");
            Console.WriteLine();
            ColorWrite("0. ", "DarkRed");
            Console.WriteLine("나가기");
            Console.WriteLine();

            List<string> validSelection = new List<string> { "0" };
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            string selectEnter = Console.ReadLine();
            if (IsValidInput(selectEnter, validSelection) == false)
            {
                DisplayMyInformation(player, items);
            }

        }

        static void DisplayInventory(Player player, List<Item> items)
        {
            Console.Clear();
            ColorWrite("인벤토리", "DarkRed");
            Console.WriteLine();
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");
            Console.WriteLine("[아이템 목록]");

            foreach (Item item in items)
            {
                if (item.IsHave == false)   continue;

                Console.Write(" - ");

                if (item.IsEquip == true)
                {
                    Console.Write($"{" [E]" + item.Name,-10}");
                    Console.Write("|");
                    if (item.CP != 0) {
                        Console.Write($"공격력{item.CP.ToString("+#;-#;0"),-10}");
                        Console.Write("|");
                    }
                    if (item.DEF != 0) {
                        Console.Write($"방어력{item.DEF.ToString("+#;-#;0"),-10}");
                        Console.Write("|");
                    }
                    Console.WriteLine($"{item.Description,-30}");
                }
                else
                {
                    Console.Write($"{" " + item.Name,-10}");
                    Console.Write("|");
                    if (item.CP != 0)
                    {
                        Console.Write($"공격력{item.CP.ToString("+#;-#;0"),-10}");
                        Console.Write("|");
                    }
                    if (item.DEF != 0)
                    {
                        Console.Write($"방어력{item.DEF.ToString("+#;-#;0"),-10}");
                        Console.Write("|");
                    }
                    Console.WriteLine($"{item.Description,-30}");
                }
                Console.WriteLine();
            }
            ColorWrite("1. ", "DarkRed");
            Console.WriteLine("장착 관리");
            ColorWrite("2. ", "DarkRed");
            Console.WriteLine("아이템 정렬");
            ColorWrite("0. ", "DarkRed");
            Console.WriteLine("나가기\n");

            List<string> validSelection = new List<string> { "0", "1", "2" };
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            string selectEnter = Console.ReadLine();
            if (IsValidInput(selectEnter, validSelection) == false)
                DisplayInventory(player, items);
            switch (selectEnter)
            {
                case "1":
                    EquipMode(player, items); break;

                case "2":
                    ItemSort(player, items); break;
                case "0":
                    DisplayMain(player, items); break;
            }

        }

        static void ItemSort(Player player, List<Item> items)
        {
            Console.Clear();
            ColorWrite("인벤토리", "DarkRed");
            Console.WriteLine();
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");
            Console.WriteLine("[아이템 목록]");

            foreach (Item item in items)
            {
                if (item.IsHave == false) continue;

                Console.Write(" - ");

                if (item.IsEquip == true)
                {
                    Console.Write($"{" [E]" + item.Name,-10}");
                    Console.Write("|");
                    if (item.CP != 0)
                    {
                        Console.Write($"공격력{item.CP.ToString("+#;-#;0"),-10}");
                        Console.Write("|");
                    }
                    if (item.DEF != 0)
                    {
                        Console.Write($"방어력{item.DEF.ToString("+#;-#;0"),-10}");
                        Console.Write("|");
                    }
                    Console.WriteLine($"{item.Description,-30}");
                }
                else
                {
                    Console.Write($"{" " + item.Name,-10}");
                    Console.Write("|");
                    if (item.CP != 0)
                    {
                        Console.Write($"공격력{item.CP.ToString("+#;-#;0"),-10}");
                        Console.Write("|");
                    }
                    if (item.DEF != 0)
                    {
                        Console.Write($"방어력{item.DEF.ToString("+#;-#;0"),-10}");
                        Console.Write("|");
                    }
                    Console.WriteLine($"{item.Description,-30}");
                }
                Console.WriteLine();
            }
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            ColorWrite("1. ", "DarkRed");
            Console.WriteLine("이름순 정렬");
            ColorWrite("2. ", "DarkRed");
            Console.WriteLine("장착순 정렬");
            ColorWrite("3. ", "DarkRed");
            Console.WriteLine("공격력순 정렬");
            ColorWrite("4. ", "DarkRed");
            Console.WriteLine("방어력순 정렬");
            ColorWrite("0. ", "DarkRed");
            Console.WriteLine("나가기\n");

            List<string> validSelection = new List<string> { "0", "1", "2", "3" , "4"};
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            string selectEnter = Console.ReadLine();
            if (IsValidInput(selectEnter, validSelection) == false)
                ItemSort(player, items);
            List < Item > sortedItems = items;
            switch (selectEnter)
            {
                case "1":
                    sortedItems = items.OrderBy(p => p.Name).ToList(); ItemSort(player, sortedItems); break;
                case "2":
                    sortedItems = items.OrderBy(p => p.IsEquip).ToList(); ItemSort(player, sortedItems); break;
                case "3":
                    sortedItems = items.OrderBy(p => p.CP).ToList(); ItemSort(player, sortedItems); break;
                case "4":
                    sortedItems = items.OrderBy(p => p.DEF).ToList(); ItemSort(player, sortedItems); break;
                case "0":
                    DisplayInventory(player, sortedItems); break;
            }
        }

        static void EquipMode(Player player, List<Item> items)
        {
            Console.Clear();
            ColorWrite("인벤토리 - 장착 관리", "DarkRed");
            Console.WriteLine();
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");
            Console.WriteLine("[아이템 목록]");
            List<string> validSelection = new List<string> { "0" };

            foreach (Item item in items)
            {
                if (item.IsHave == false)   continue;
                validSelection.Add((items.IndexOf(item) + 1).ToString());

                Console.Write(" - ");
                ColorWrite((validSelection.Count-1).ToString(), "DarkRed");
                if (item.IsEquip == true)
                {
                    Console.Write($"{" [E]" + item.Name,-10}");
                    Console.Write("|");
                    if (item.CP != 0)
                    {
                        Console.Write($"공격력{item.CP.ToString("+#;-#;0"),-10}");
                        Console.Write("|");
                    }
                    if (item.DEF != 0)
                    {
                        Console.Write($"방어력{item.DEF.ToString("+#;-#;0"),-10}");
                        Console.Write("|");
                    }
                    Console.WriteLine($"{ item.Description, -30}");
                }
                else
                {
                    Console.Write($"{" " + item.Name,-10}");
                    Console.Write("|");
                    if (item.CP != 0)
                    {
                        Console.Write($"공격력{item.CP.ToString("+#;-#;0"),-10}");
                        Console.Write("|");
                    }
                    if (item.DEF != 0)
                    {
                        Console.Write($"방어력{item.DEF.ToString("+#;-#;0"),-10}");
                        Console.Write("|");
                    }
                    Console.WriteLine($"{item.Description,-30}");
                }
                Console.WriteLine();
            }
            ColorWrite("0. ", "DarkRed");
            Console.WriteLine("나가기\n");

            Console.WriteLine("원하시는 행동을 입력해주세요.");
            string selectEnter = Console.ReadLine();
            if (IsValidInput(selectEnter, validSelection) == false)
            {
                EquipMode(player, items);
            }
            if (selectEnter == "0")  DisplayInventory(player, items);
            else
            {
                Item temp = items[int.Parse(selectEnter) - 1];
                temp.IsEquip = !temp.IsEquip;
                items[int.Parse(selectEnter) - 1] = temp;
                EquipMode(player, items);
            }

        }

        static void Market(Player player, List<Item> items)
        {
            Console.Clear();
            ColorWrite("상점\n", "DarkRed");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine();
            Console.WriteLine("[보유 골드]");
            ColorWrite(player.Gold.ToString(), "DarkRed");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");

            foreach (Item item in items)
            {
                Console.Write(" - ");

                Console.Write($"{" " + item.Name,-10}");
                Console.Write("|");
                if (item.CP != 0)
                {
                    Console.Write($"공격력{item.CP.ToString("+#;-#;0"),-10}");
                    Console.Write("|");
                }
                if (item.DEF != 0)
                {
                    Console.Write($"방어력{item.DEF.ToString("+#;-#;0"),-10}");
                    Console.Write("|");
                }
                Console.Write($"{item.Description,-30}");
                Console.Write("|");
                if (item.IsHave == true)
                    Console.WriteLine("구매완료");
                else 
                {
                    ColorWrite(item.Gold.ToString(), "DarkRed");
                    Console.WriteLine("G");
                }
            }
            Console.WriteLine();
            ColorWrite("1. ", "DarkRed");
            Console.WriteLine("아이템 구매");
            ColorWrite("2. ", "DarkRed");
            Console.WriteLine("아이템 판매");
            ColorWrite("0. ", "DarkRed");
            Console.WriteLine("나가기\n");

            List<string> validSelection = new List<string> { "0", "1", "2" };
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            string selectEnter = Console.ReadLine();
            if (IsValidInput(selectEnter, validSelection) == false)
            {
                Market(player, items);
            }
            switch (selectEnter)
            {
                case "1":
                    BuyItems(player, items); break;

                case "2":
                    SellItems(player, items); break;
                case "0":
                    DisplayMain(player, items); break;
            }

        }

        static void BuyItems(Player player, List<Item> items)
        {
            Console.Clear();
            ColorWrite("상점 - 아이템 구매\n", "DarkRed");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine();
            Console.WriteLine("[보유 골드]");
            ColorWrite(player.Gold.ToString(), "DarkRed");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");
            List<string> validSelection = new List<string> { "0" };

            foreach (Item item in items)
            {
                validSelection.Add((items.IndexOf(item)).ToString());

                Console.Write(" - ");
                ColorWrite((validSelection.Count - 1).ToString(), "DarkRed");

                Console.Write($"{" " + item.Name,-10}");
                Console.Write("|");
                if (item.CP != 0)
                {
                    Console.Write($"공격력{item.CP.ToString("+#;-#;0"),-10}");
                    Console.Write("|");
                }
                if (item.DEF != 0)
                {
                    Console.Write($"방어력{item.DEF.ToString("+#;-#;0"),-10}");
                    Console.Write("|");
                }
                Console.Write($"{item.Description,-30}");
                Console.Write("|");
                if (item.IsHave == true)
                    Console.WriteLine("구매완료");
                else
                {
                    ColorWrite(item.Gold.ToString(), "DarkRed");
                    Console.WriteLine("G");
                }
            }
            Console.WriteLine();
            ColorWrite("0. ", "DarkRed");
            Console.WriteLine("나가기\n");
            foreach (string val in validSelection)
                Console.WriteLine(val);
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            string selectEnter = Console.ReadLine();
            if (IsValidInput(selectEnter, validSelection) == false)
                BuyItems(player, items);
            if(selectEnter == "0")    Market(player, items);

            int selectedItem = int.Parse(validSelection[int.Parse(selectEnter)]);
            if (items[selectedItem].IsHave == true)
            {
                ColorWrite("이미 구매한 아이템입니다.", "DarkRed");
                Console.ReadKey();
                BuyItems(player, items);
            }

            if (player.Gold >= items[selectedItem].Gold)
            {
                ColorWrite("구매를 완료했습니다.", "DarkRed");
                player.Gold -= items[selectedItem].Gold;
                Item newitem = items[selectedItem];
                newitem.IsHave = true;
                items[selectedItem] = newitem;
                Console.ReadKey();
                BuyItems(player, items);
            }
            else
            {
                ColorWrite("Gold가 부족합니다.", "DarkRed");
                Console.ReadKey();
                BuyItems(player, items);
            }
        }

        static void SellItems(Player player, List<Item> items)
        {
            Console.Clear();
            ColorWrite("상점 - 아이템 판매\n", "DarkRed");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine();
            Console.WriteLine("[보유 골드]");
            ColorWrite(player.Gold.ToString(), "DarkRed");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");

            List<string> validSelection = new List<string> { "0" } ;
            foreach (Item item in items)
            {
                if (item.IsHave == false) continue;
                validSelection.Add((items.IndexOf(item)).ToString());
                Console.Write(" - ");
                ColorWrite((validSelection.Count-1).ToString(), "DarkRed");


                Console.Write($"{" " + item.Name,-10}");
                Console.Write("|");
                if (item.CP != 0)
                {
                    Console.Write($"공격력{item.CP.ToString("+#;-#;0"),-10}");
                    Console.Write("|");
                }
                if (item.DEF != 0)
                {
                    Console.Write($"방어력{item.DEF.ToString("+#;-#;0"),-10}");
                    Console.Write("|");
                }
                Console.Write($"{item.Description,-30}");
                Console.Write("|");

                ColorWrite(((int)item.Gold*0.85).ToString(), "DarkRed");
                Console.WriteLine("G");
            }
            Console.WriteLine();
            Console.WriteLine("아이템 판매");
            ColorWrite("0. ", "DarkRed");
            Console.WriteLine("나가기\n");

            foreach (string val in validSelection)
                Console.WriteLine(val);


            Console.WriteLine("원하시는 행동을 입력해주세요.");
            string selectEnter = Console.ReadLine();
            if (IsValidInput(selectEnter, validSelection) == false) SellItems(player, items);
            if(selectEnter == "0")  Market(player, items);
            else
            {
                int selectedItem = int.Parse(validSelection[int.Parse(selectEnter)]);
                Item newitem = items[selectedItem];
                player.Gold += (int)((newitem.Gold) * 0.85);
                newitem.IsEquip = false;
                newitem.IsHave = false;
                items[selectedItem] = newitem;
                SellItems(player, items);
            }
        }

        static bool IsValidInput(string input, List<string> validValues)
        {
            foreach (string value in validValues)
            {
                if (string.Equals(input, value)) return true;
            }
            ColorWrite("잘못된 입력입니다.", "DarkRed");
            Console.ReadKey();
            return false;
        }

        static void ColorWrite(string words, string color)
        {
            if (color.Equals("DarkRed"))
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write(words);
                Console.ResetColor();
            }
        }

        static void SettingPlayer(out Player player)
        {
            player = new Player();
            player.Level = 1;
            player.Name = "콘치즈삼겹살";
            player.CharClass = "전사";
            player.CP = 10;
            player.DEF = 5;
            player.HP = 100;
            player.Gold = 1500;
        }

        static void SettingItems(out List<Item> items)
        {
            Item item0 = new Item(true, true, "무쇠갑옷", 0, 9, "무쇠로 만들어져 튼튼한 갑옷입니다.", 2000);
            Item item1 = new Item(false, true, "낡은 검", 2, 0, "쉽게 볼 수 있는 낡은 검 입니다.", 500);
            Item item2 = new Item(false, false, "수련자 갑옷", 0, 5, "수련에 도움을 주는 갑옷입니다.", 1000);
            Item item3 = new Item(false, false, "스파르타의 갑옷", 0, 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500);
            Item item4 = new Item(false, false, "청동 도끼", 5, 0, "어디선가 사용됐던거 같은 도끼입니다.", 1500);
            Item item5 = new Item(false, false, "스파르타의 창", 7, 0, "스파르타의 전사들이 사용했다는 전설의 창입니다.", 3500);

            items = new List<Item>();
            items.Add(item0);
            items.Add(item1); 
            items.Add(item2);
            items.Add(item3);
            items.Add(item4);
            items.Add(item5);
        }

        static void Main(string[] args)
        {
            Player player;
            List<Item> items;
            SettingPlayer(out player);
            SettingItems(out items);

            while (true)
            {
                DisplayMain(player, items);
            }

        }

        
    }
}
