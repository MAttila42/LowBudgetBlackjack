using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Numerics;

namespace Blackjack
{
    class Blackjack
    {
        // Tulajdonságok:
        //  [✓] - Fájlba mentés(game.saves)
        //  [✓] - Ha nincs fájl létrehozni egyet
        //  [✓] - Navigálás billentyűk leütésével a menük között(számok)
        //  [✓] - Mentések létrehozása(9db)
        //  [✓] - Mentések kiírása listában
        //  [✓] - Mentés törlése
        //  [✓] - $200 kezdőösszeg
        //  [✓] - Mentés kiválasztása
        //  [✓] - Profilhoz tartozó menü(Név, Pénz; Új játék, Statisztikák; Vissza)
        //  [✓] - Statisztikák(Név, Pénz, Játékok, Nyerések, Nyerési %, Profit)
        //  [✓] - 2db piros, 2db fehér ugyan olyan kártya(francia kártyák vektor)
        //  [✓] - Egyéni összeg feltétele, megjegyzése
        //  [✓] - Ha nincs annyi pénz, akkor nem csinál semmit
        //  [✓] - Minimum feltett összeg: $20. Ha kevesebb van feltéve, akkor nem csinál semmit
        //  [✓] - Véletlenszerű kártyasorsolás a banknak úgy, hogy a második nem ismert, de a program tudja mi az
        //  [✓] - Véletlenszerű kártyasorsolás a játékosnak
        //  [✓] - Lap húzása
        //  [✓] - Megállás
        //  [✓] - A bank a játékos pontjai fölé megy kivéve, ha a játékosnak 21 fölött vannak a lapjai, vagy blackjackje van
        //  [✓] - Nyerés ha több a kártyalapok összege
        //  [✓] - 21-nél nyerés a banknál => ha döntetlen: bank nyer
        //  [✓] - 21-en felül vesztés
        //  [✓] - 21-en felül automatikusan a megállás fut le
        //  [✓] - 21-nél automatikus megállás
        //  [✓] - Blackjacknél a bank ne húzzon lapokat
        //  [✓] - Vesztésnél a feltett összeg levonása, nyerésnél a feltett összeg megkapása
        //  [✓] - Játszott menetek megjegyzése
        //  [✓] - Nyerések megjegyzése
        //  [✓] - Random szöveg játék végénél
        //  [✓] - Fájlok mentése dictionary-val
        //  [✓] - Ne lehessen olyan nevet létrehozni ami tartalmazza a ";" karaktert
        //  [✓] - Ne lehessen egyszerre két egyforma nevet létrehozni
        //  [✓] - Duplázás
        //  [✓] - Egyik lap félretétele (kiszedés a két lap közül és egy ideiglenes változóba helyezni)
        //  [✓] - A score csökkentése a kievett laptól függően
        //  [✓] - Az aktuális lap mellé hozzáadni egyet
        //  [✓] - Mindennek a kiírása, lapműveletekkel
        //  [✓] - A játék végénél a gameOver lefuttatása
        //  [✓] - Játékos lapjainak törlése
        //  [✓] - Bank lapjainak törlése
        //  [✓] - 2 új lap húzása a banknak úgy, mint a játék elején
        //  [✓] - Eltett lap előhozása
        //  [✓] - Random kártyalap húzása mellé
        //  [✓] - Mindennek a kiírása, lapműveletekkel
        //  [✓] - Utolsó játék lejátszása gameOverrel a végén és visszakerül a menübe
	    //  [X] - Ha nincs elég pénzed, ne engedjen új játékot indítani
        //  [X] - Üres nevet ne engedjen
        //  [X] - Ha nincs elég pénzed, ne lehessen duplázni
        //  [X] - Blackjacknél 150%-kot kapsz vissza
        //  [X] - Hibás bevitel ne crasheljen, kérjen újra értéket
        //  [X] - Információk

        // ESC: ''

        static void Menu()
        {
            Title();
            Console.WriteLine(" Főmenü:");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("   [1]");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(" Új létrehozása");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("   [2]");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(" Betöltés");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("   [3]");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(" Törlés");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("   [4]");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(" Információk");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.Write(" [ESC]");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(" Kilépés");

            for (int i = 0; i < 1; i++)
            {
                char key = Console.ReadKey(true).KeyChar;
                switch (key)
                {
                    case '1': New(); break;
                    case '2': Load(); break;
                    case '3': Delete(); break;
                    case '4': Info(); break;
                    case '': Environment.Exit(0); break;
                    default: i--; break;
                }
            }
        }
        static void New()
        {
            Title();
            Console.WriteLine(" Új létrehozása:");
            Console.WriteLine();
            Console.Write(" Név: ");

            Console.ForegroundColor = ConsoleColor.White;
            string name = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Gray;

            string[] saves = File.ReadAllLines("game.saves");
            List<string> names = new List<string>();
            int counter = 0;

            foreach (var i in saves)
            {
                string m = i.Split(';')[0];
                names.Add(m);
                counter++;
            }

            if (name.Contains(";") || names.Contains(name) && name != "-") New();

            for (int i = 0; i < saves.Length; i++)
            {
                if (saves[i] == "-")
                {
                    saves[i] = name + ";200;0;0";
                    File.WriteAllLines("game.saves", saves);
                    break;
                }
            }
            Menu();
        }
        static void Load()
        {
            Title();
            Console.WriteLine(" Betöltés:");
            Console.WriteLine();

            ListSaves();

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" [ESC]");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(" Vissza");

            for (int i = 0; i < 1; i++)
            {
                char key = Console.ReadKey(true).KeyChar;
                switch (key)
                {
                    case '1': LoadSave(key); break;
                    case '2': LoadSave(key); break;
                    case '3': LoadSave(key); break;
                    case '4': LoadSave(key); break;
                    case '5': LoadSave(key); break;
                    case '6': LoadSave(key); break;
                    case '7': LoadSave(key); break;
                    case '8': LoadSave(key); break;
                    case '9': LoadSave(key); break;
                    case '': break;
                    default: i--; break;
                }
            }
            Menu();
        }
        static void Delete()
        {
            Title();
            Console.WriteLine(" Törlés:");
            Console.WriteLine();

            ListSaves();

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" [ESC]");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(" Vissza");

            string[] saves = File.ReadAllLines("game.saves");
            for (int i = 0; i < 1; i++)
            {
                char key = Console.ReadKey(true).KeyChar;
                switch (key)
                {
                    case '1': DelSave(key); break;
                    case '2': DelSave(key); break;
                    case '3': DelSave(key); break;
                    case '4': DelSave(key); break;
                    case '5': DelSave(key); break;
                    case '6': DelSave(key); break;
                    case '7': DelSave(key); break;
                    case '8': DelSave(key); break;
                    case '9': DelSave(key); break;
                    case '': Menu(); break;
                    default: i--; break;
                }
            }
            Delete();
        }
        static void Info()
        {
            Title();
            Console.WriteLine(" Információk:\n");
            Console.Write(" Verzió: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("BETA v0.1\n");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(" Készítette: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("ExAtom\n");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(" Low budget blackjack C# nyelven egy tizenéves sráctól! Jó szórakozást!\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" [ESC]");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(" Vissza");

            for (int i = 0; i < 1; i++)
            {
                char key = Console.ReadKey(true).KeyChar;
                switch (key)
                {
                    case '': break;
                    default: i--; break;
                }
            }
            Menu();
        }
        static void Title() { Console.Clear(); Console.WriteLine("\n - Low budget blackjack -\n"); }
        static void ListSaves()
        {
            string[] saves = File.ReadAllLines("game.saves");
            byte saveCount = 1;
            for (int i = 0; i < saves.Length; i++)
            {
                string[] m = saves[i].Split(';');
                if (saves[i] != "-")
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"   [{saveCount++}] ");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine($"{m[0]} | ${m[1]}");
                }
            }
            if (saveCount == 1) Console.WriteLine(" Nincs mentés!");
        }
        static void LoadSave(char num)
        {
            string[] saves = File.ReadAllLines("game.saves");
            int loaded = int.Parse(num.ToString()) - 1;
            if (saves[loaded] == "-") Load();
            string[] m = saves[loaded].Split(';');

            Title();
            Console.Write("  Név: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(m[0]);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(" Pénz: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"${m[1]}");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("   [1]");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(" Új játék");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("   [2]");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(" Statisztikák");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" [ESC]");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(" Vissza");

            for (int i = 0; i < 1; i++)
            {
                char key = Console.ReadKey(true).KeyChar;
                switch (key)
                {
                    case '1': NewGame(loaded, num); break;
                    case '2': Statistics(loaded, num); break;
                    case '': break;
                    default: i--; break;
                }
            }
            Menu();
        }
        static void Statistics(int loaded, char num)
        {
            string[] saves = File.ReadAllLines("game.saves");
            string[] m = saves[loaded].Split(';');
            string name = m[0];
            BigInteger money = BigInteger.Parse(m[1].ToString());
            double games = double.Parse(m[2].ToString());
            double wins = double.Parse(m[3].ToString());

            Title();
            Console.Write("      Név: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(name);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("     Pénz: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"${money}");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("  Játékok: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(games);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(" Nyerések: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(wins);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(" Nyerés %: ");
            Console.ForegroundColor = ConsoleColor.White;

            if (games == 0) games = 1;
            double percentage = Math.Round(wins / games * 100, 2, MidpointRounding.ToEven);

            Console.WriteLine($"{percentage}%");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("   Profit: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"${money - 200}");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" [ESC]");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(" Vissza");

            for (int i = 0; i < 1; i++)
            {
                char key = Console.ReadKey(true).KeyChar;
                switch (key) { case '': break; default: i--; break; }
            }
            LoadSave(num);
        }
        static void DelSave(char num)
        {
            string[] saves = File.ReadAllLines("game.saves");
            if (num != '9') for (int i = int.Parse(num.ToString()) - 1; i < 8; i++) saves[i] = saves[i + 1];
            saves[8] = "-";
            File.WriteAllLines("game.saves", saves);
        }
        static void NewGame(int loaded, char num)
        {
            // [<KiírandóNév::2>][<Érték::2>][<Fekete/Fehér(0/1)::1>] // Név
            List<string> deck = new List<string>();
            deck.Add("-A110"); // SpadesA
            deck.Add("-2020"); // Spades2
            deck.Add("-3030"); // Spades3
            deck.Add("-4040"); // Spades4
            deck.Add("-5050"); // Spades5
            deck.Add("-6060"); // Spades6
            deck.Add("-7070"); // Spades7
            deck.Add("-8080"); // Spades8
            deck.Add("-9090"); // Spades9
            deck.Add("10100"); // Spades10
            deck.Add("-J100"); // SpadesJ
            deck.Add("-Q100"); // SpadesQ
            deck.Add("-K100"); // SpadesK
            deck.Add("-A110"); // CkubA
            deck.Add("-2020"); // Ckub2
            deck.Add("-3030"); // Ckub3
            deck.Add("-4040"); // Ckub4
            deck.Add("-5050"); // Ckub5
            deck.Add("-6060"); // Ckub6
            deck.Add("-7070"); // Ckub7
            deck.Add("-8080"); // Ckub8
            deck.Add("-9090"); // Ckub9
            deck.Add("10100"); // Ckub10
            deck.Add("-J100"); // CkubJ
            deck.Add("-Q100"); // CkubQ
            deck.Add("-K100"); // CkubK
            deck.Add("-A111"); // HeartsA
            deck.Add("-2021"); // Hearts2
            deck.Add("-3031"); // Hearts3
            deck.Add("-4041"); // Hearts4
            deck.Add("-5051"); // Hearts5
            deck.Add("-6061"); // Hearts6
            deck.Add("-7071"); // Hearts7
            deck.Add("-8081"); // Hearts8
            deck.Add("-9091"); // Hearts9
            deck.Add("10101"); // Hearts10
            deck.Add("-J101"); // HeartsJ
            deck.Add("-Q101"); // HeartsQ
            deck.Add("-K101"); // HeartsK
            deck.Add("-A111"); // DiamondsA
            deck.Add("-2021"); // Diamonds2
            deck.Add("-3031"); // Diamonds3
            deck.Add("-4041"); // Diamonds4
            deck.Add("-5051"); // Diamonds5
            deck.Add("-6061"); // Diamonds6
            deck.Add("-7071"); // Diamonds7
            deck.Add("-8081"); // Diamonds8
            deck.Add("-9091"); // Diamonds9
            deck.Add("10101"); // Diamonds10
            deck.Add("-J101"); // DiamondsJ
            deck.Add("-Q101"); // DiamondsQ
            deck.Add("-K101"); // DiamondsK

            string[] saves = File.ReadAllLines("game.saves");
            string[] m = saves[loaded].Split(';');
            string name = m[0];
            BigInteger money = BigInteger.Parse(m[1].ToString());
            int games = int.Parse(m[2]);
            int wins = int.Parse(m[3]);

            Title();
            Console.Write($" Pénzed: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"${money}\n");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(" Pénz feltétele (min. $20): ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("$");
            BigInteger bet = BigInteger.Parse(Console.ReadLine());
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\n");

            if (bet > money || bet < 20) NewGame(loaded, num);
            List<int> bankCardsValue = new List<int>();
            List<int> playerCardsValue = new List<int>();
            List<string> bankCards = new List<string>();
            List<string> playerCards = new List<string>();
            int bankScore = 0;
            int playerScore = 0;
            int absBankScore = 0;
            int absPlayerScore = 0;
            int tempBankScore = 0;
            int tempPlayerScore = 0;
            Random random = new Random();

            string card;
            bool red;
            string display;

            Title();
            Console.Write(" Bank:\n\n  ");
            for (int i = 0; i < 2; i++)
            {
                int randNum = random.Next(0, deck.Count);
                card = deck[randNum];
                deck.RemoveAt(randNum);
                int cardValue = int.Parse(card[2].ToString() + card[3].ToString());
                bankCards.Add(card);
                bankCardsValue.Add(cardValue);
                absBankScore += cardValue;
                red = card[4] == '1';
                if (card[0] == '-') display = card[1].ToString();
                else display = card[0].ToString() + card[1].ToString();

                if (red) Console.ForegroundColor = ConsoleColor.Red;
                else Console.ForegroundColor = ConsoleColor.White;
                if (i == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write("[X] ");
                }
                else Console.Write($"[{display}] ");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            tempBankScore = absBankScore;
            for (int i = 0; i < 4; i++) if (tempBankScore > 21 && bankCardsValue.Count(x => x == 11) > i) tempBankScore -= 10; else break;
            bankScore = tempBankScore;
            Console.Write($"\n\n Érték: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"-");
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.Write($"\n\n {name}:\n\n  ");
            for (int i = 0; i < 2; i++)
            {
                int randNum = random.Next(0, deck.Count);
                card = deck[randNum];
                deck.RemoveAt(randNum);
                int cardValue = int.Parse(card[2].ToString() + card[3].ToString());
                playerCards.Add(card);
                playerCardsValue.Add(cardValue);
                absPlayerScore += cardValue;
                red = card[4] == '1';
                if (card[0] == '-') display = card[1].ToString();
                else display = card[0].ToString() + card[1].ToString();

                if (red) Console.ForegroundColor = ConsoleColor.Red;
                else Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"[{display}] ");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            tempPlayerScore = absPlayerScore;
            for (int i = 0; i < 4; i++) if (tempPlayerScore > 21 && playerCardsValue.Count(x => x == 11) > i) tempPlayerScore -= 10; else break;
            playerScore = tempPlayerScore;
            Console.Write($"\n\n Érték: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(playerScore);
            Console.ForegroundColor = ConsoleColor.Gray;

            string[] temporary = { "0" };
            File.WriteAllLines("temporary.txt", temporary);
            string doubledCard;

            for (int i = 0; i < 1; i++)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(" [1]");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(" Lap húzása");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(" [2]");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(" Megállás");

                string doubl = File.ReadAllLines("temporary.txt")[0];

                string playerCard1 = playerCards[0];
                string playerCard2 = playerCards[1];
                if (playerCard1[1].ToString() == playerCard2[1].ToString() && playerCards.Count == 2 && doubl == "0")
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" [3]");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(" Duplázás");
                }

                bool getCard = false;
                bool stopHere = false;
                bool gameOver = false;
                bool win = false;

                if (playerScore != 21)
                {
                    for (int j = 0; j < 1; j++)
                    {
                        char key = Console.ReadKey(true).KeyChar;
                        switch (key)
                        {
                            case '1': getCard = true; break;
                            case '2': stopHere = true; break;
                            case '3':
                                if (playerCard1[1].ToString() == playerCard2[1].ToString() && playerCards.Count == 2 && doubl == "0")
                                {
                                    doubl = "1";
                                    temporary[0] = doubl;
                                    File.WriteAllLines("temporary.txt", temporary);
                                }
                                else j--;
                                break;
                            default: j--; break;
                        }
                    }
                }
                else stopHere = true;

                if (getCard)
                {
                    Title();
                    Console.Write(" Bank:\n\n  ");
                    for (int j = 0; j < bankCards.Count; j++)
                    {
                        card = bankCards[j];
                        red = card[4] == '1';
                        if (card[0] == '-') display = card[1].ToString();
                        else display = card[0].ToString() + card[1].ToString();

                        if (red) Console.ForegroundColor = ConsoleColor.Red;
                        else Console.ForegroundColor = ConsoleColor.White;
                        if (j == 1)
                        {
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.Write("[X] ");
                        }
                        else Console.Write($"[{display}] ");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    Console.Write($"\n\n Érték: ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"-");
                    Console.ForegroundColor = ConsoleColor.Gray;

                    Console.Write($"\n\n {name}:\n\n  ");
                    for (int j = 0; j < playerCards.Count; j++)
                    {
                        if (j == 1)
                        {
                            int randNum = random.Next(0, deck.Count);
                            string newCard = deck[randNum];
                            deck.RemoveAt(randNum);
                            int cardValue = int.Parse(newCard[2].ToString() + newCard[3].ToString());
                            playerCards.Add(newCard);
                            playerCardsValue.Add(cardValue);
                            absPlayerScore += cardValue;
                        }

                        card = playerCards[j];
                        red = card[4] == '1';
                        if (card[0] == '-') display = card[1].ToString();
                        else display = card[0].ToString() + card[1].ToString();

                        if (red) Console.ForegroundColor = ConsoleColor.Red;
                        else Console.ForegroundColor = ConsoleColor.White;
                        Console.Write($"[{display}] ");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    tempPlayerScore = absPlayerScore;
                    for (int j = 0; j < 4; j++) if (tempPlayerScore > 21 && playerCardsValue.Count(x => x == 11) > j) tempPlayerScore -= 10; else break;
                    playerScore = tempPlayerScore;
                    Console.Write($"\n\n Érték: ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(playerScore);
                    Console.ForegroundColor = ConsoleColor.Gray;

                    if (playerScore > 21) stopHere = true;
                    else i--;
                }

                if (stopHere)
                {
                    while (bankScore < playerScore && playerScore <= 21 && !(playerScore == 21 && playerCards.Count == 2))
                    {
                        if (playerScore == 21 && playerCards.Count == 2) break;
                        int randNum = random.Next(0, deck.Count);
                        string newCard = deck[randNum];
                        deck.RemoveAt(randNum);
                        int cardValue = int.Parse(newCard[2].ToString() + newCard[3].ToString());
                        bankCards.Add(newCard);
                        bankCardsValue.Add(cardValue);
                        absBankScore += cardValue;
                        tempBankScore = absBankScore;
                        for (int j = 0; j < 4; j++) if (tempBankScore > 21 && bankCardsValue.Count(x => x == 11) > j) tempBankScore -= 10; else break;
                        bankScore = tempBankScore;
                    }

                    Title();
                    Console.Write($" Bank:\n\n  ");
                    for (int j = 0; j < bankCards.Count; j++)
                    {
                        card = bankCards[j];
                        red = card[4] == '1';
                        if (card[0] == '-') display = card[1].ToString();
                        else display = card[0].ToString() + card[1].ToString();

                        if (red) Console.ForegroundColor = ConsoleColor.Red;
                        else Console.ForegroundColor = ConsoleColor.White;
                        Console.Write($"[{display}] ");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    tempBankScore = absBankScore;
                    for (int j = 0; j < 4; j++) if (tempBankScore > 21 && bankCardsValue.Count(x => x == 11) > j) tempBankScore -= 10; else break;
                    bankScore = tempBankScore;
                    Console.Write($"\n\n Érték: ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(bankScore);
                    Console.ForegroundColor = ConsoleColor.Gray;

                    Console.Write($"\n\n {name}:\n\n  ");
                    for (int j = 0; j < playerCards.Count; j++)
                    {
                        card = playerCards[j];
                        red = card[4] == '1';
                        if (card[0] == '-') display = card[1].ToString();
                        else display = card[0].ToString() + card[1].ToString();

                        if (red) Console.ForegroundColor = ConsoleColor.Red;
                        else Console.ForegroundColor = ConsoleColor.White;
                        Console.Write($"[{display}] ");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    tempPlayerScore = absPlayerScore;
                    for (int j = 0; j < 4; j++) if (tempPlayerScore > 21 && playerCardsValue.Count(x => x == 11) > j) tempPlayerScore -= 10; else break;
                    playerScore = tempPlayerScore;
                    Console.Write($"\n\n Érték: ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(playerScore);
                    Console.ForegroundColor = ConsoleColor.Gray;

                    gameOver = true;
                    if ((playerScore > bankScore || bankScore > 21) && playerScore <= 21 && !(bankCards.Count == 2 && bankScore == 21)) win = true;
                    if (doubl == "1")
                    {
                        doubl = "1";
                        temporary[0] = doubl;
                        File.WriteAllLines("temporary.txt", temporary);
                    }
                }

                doubledCard = playerCards[1];

                doubl = File.ReadAllLines("temporary.txt")[0];

                if (doubl == "1")
                {
                    if (doubl == "1")
                    {
                        playerCards.RemoveAt(1);
                        int doubledCardValue = int.Parse(doubledCard[2].ToString() + doubledCard[3].ToString());
                        playerCardsValue.Remove(doubledCardValue);
                        absPlayerScore -= doubledCardValue;

                        Title();
                        Console.Write(" Bank:\n\n  ");
                        for (int j = 0; j < bankCards.Count; j++)
                        {
                            card = bankCards[j];
                            red = card[4] == '1';
                            if (card[0] == '-') display = card[1].ToString();
                            else display = card[0].ToString() + card[1].ToString();

                            if (red) Console.ForegroundColor = ConsoleColor.Red;
                            else Console.ForegroundColor = ConsoleColor.White;
                            if (j == 1)
                            {
                                Console.ForegroundColor = ConsoleColor.Gray;
                                Console.Write("[X] ");
                            }
                            else Console.Write($"[{display}] ");
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                        Console.Write($"\n\n Érték: ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine($"-");
                        Console.ForegroundColor = ConsoleColor.Gray;

                        Console.Write($"\n\n {name}:\n\n  ");
                        for (int j = 0; j < playerCards.Count; j++)
                        {
                            if (j == 0)
                            {
                                int randNum = random.Next(0, deck.Count);
                                string newCard = deck[randNum];
                                deck.RemoveAt(randNum);
                                int cardValue = int.Parse(newCard[2].ToString() + newCard[3].ToString());
                                playerCards.Add(newCard);
                                playerCardsValue.Add(cardValue);
                                absPlayerScore += cardValue;
                            }

                            card = playerCards[j];
                            red = card[4] == '1';
                            if (card[0] == '-') display = card[1].ToString();
                            else display = card[0].ToString() + card[1].ToString();

                            if (red) Console.ForegroundColor = ConsoleColor.Red;
                            else Console.ForegroundColor = ConsoleColor.White;
                            Console.Write($"[{display}] ");
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                        tempPlayerScore = absPlayerScore;
                        for (int j = 0; j < 4; j++) if (tempPlayerScore > 21 && playerCardsValue.Count(x => x == 11) > j) tempPlayerScore -= 10; else break;
                        playerScore = tempPlayerScore;
                        Console.Write($"\n\n Érték: ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(playerScore);
                        Console.ForegroundColor = ConsoleColor.Gray;

                        if (playerScore > 21) stopHere = true;
                        else i--;
                    }
                    doubl = "2";
                    temporary[0] = doubl;
                    File.WriteAllLines("temporary.txt", temporary);
                }

                if (gameOver)
                {
                    if (win)
                    {
                        money += bet;
                        wins++;
                        RandomWin(bet);
                    }
                    else
                    {
                        money -= bet;
                        RandomLose(bet);
                    }

                    doubl = File.ReadAllLines("temporary.txt")[0];

                    if (doubl == "2")
                    {
                        playerCards.Clear();
                        playerCards.Add(doubledCard);
                        int doubledCardValue = int.Parse(doubledCard[2].ToString() + doubledCard[3].ToString());
                        playerCardsValue.Clear();
                        playerCardsValue.Add(doubledCardValue);
                        absPlayerScore = 0;
                        absPlayerScore += doubledCardValue;

                        bankCards.Clear();
                        bankCardsValue.Clear();
                        absBankScore = 0;

                        Title();
                        Console.Write(" Bank:\n\n  ");
                        for (int j = 0; j < 2; j++)
                        {
                            int randNum = random.Next(0, deck.Count);
                            card = deck[randNum];
                            deck.RemoveAt(randNum);
                            int cardValue = int.Parse(card[2].ToString() + card[3].ToString());
                            bankCards.Add(card);
                            bankCardsValue.Add(cardValue);
                            absBankScore += cardValue;
                            red = card[4] == '1';
                            if (card[0] == '-') display = card[1].ToString();
                            else display = card[0].ToString() + card[1].ToString();

                            if (red) Console.ForegroundColor = ConsoleColor.Red;
                            else Console.ForegroundColor = ConsoleColor.White;
                            if (j == 1)
                            {
                                Console.ForegroundColor = ConsoleColor.Gray;
                                Console.Write("[X] ");
                            }
                            else Console.Write($"[{display}] ");
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                        tempBankScore = absBankScore;
                        for (int j = 0; j < 4; j++) if (tempBankScore > 21 && bankCardsValue.Count(x => x == 11) > j) tempBankScore -= 10; else break;
                        bankScore = tempBankScore;
                        Console.Write($"\n\n Érték: ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine($"-");
                        Console.ForegroundColor = ConsoleColor.Gray;

                        Console.Write($"\n\n {name}:\n\n  ");
                        for (int j = 0; j < playerCards.Count; j++)
                        {
                            if (j == 0)
                            {
                                int randNum = random.Next(0, deck.Count);
                                string newCard = deck[randNum];
                                deck.RemoveAt(randNum);
                                int cardValue = int.Parse(newCard[2].ToString() + newCard[3].ToString());
                                playerCards.Add(newCard);
                                playerCardsValue.Add(cardValue);
                                absPlayerScore += cardValue;
                            }

                            card = playerCards[j];
                            red = card[4] == '1';
                            if (card[0] == '-') display = card[1].ToString();
                            else display = card[0].ToString() + card[1].ToString();

                            if (red) Console.ForegroundColor = ConsoleColor.Red;
                            else Console.ForegroundColor = ConsoleColor.White;
                            Console.Write($"[{display}] ");
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                        tempPlayerScore = absPlayerScore;
                        for (int j = 0; j < 4; j++) if (tempPlayerScore > 21 && playerCardsValue.Count(x => x == 11) > j) tempPlayerScore -= 10; else break;
                        playerScore = tempPlayerScore;
                        Console.Write($"\n\n Érték: ");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(playerScore);
                        Console.ForegroundColor = ConsoleColor.Gray;

                        if (playerScore > 21) stopHere = true;
                        i--;
                        doubl = "3";
                        temporary[0] = doubl;
                        File.WriteAllLines("temporary.txt", temporary);
                    }
                }
            }

            string[] nothing = new string[0];
            File.WriteAllLines("temporary.txt", nothing);

            games++;
            m[1] = money.ToString();
            m[2] = games.ToString();
            m[3] = wins.ToString();
            saves[loaded] = m[0] + ";" + m[1] + ";" + m[2] + ";" + m[3];
            File.WriteAllLines("game.saves", saves);
            LoadSave(num);
        }
        static void RandomWin(BigInteger bet)
        {
            string[] randomWin =
            {
                "Legközelebb úgyis vesztesz.",
                "Ez jó menet volt. Jöhet még egy?",
                "Végre egy nyereséges játszma!",
                "A jól megérdemelt zseton.",
                "Fasza volt! Mehet mégegyszer!",
                "Az anyaország büszke rád!",
                "EZ.",
                "Előre a győzelembe és tovább!",
                "Wow! Megy ez neked.",
                "Még egy nyerés mehet fel a listára.",
                "Ilyen egy igazi gamer."
            };

            Console.Write("\n\n ");
            Random random = new Random();
            Console.WriteLine(randomWin[random.Next(0, randomWin.Length)]);

            Console.Write(" Nyertél");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($" ${bet}");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("-t!");
            Console.WriteLine("\n A folytatáshoz nyomj meg egy gombot...");

            Console.ReadKey(true);
        }
        static void RandomLose(BigInteger bet)
        {
            string[] randomLose =
            {
                "Gratulálok! Egy újabb veszteséggel lettél gazdagabb.",
                "Ha így folytatod, fenntarthatod az adósságaidból az egész várost.",
                "Eh! Ezt se tudod játszani? Pedig csak lapokat kellett húznod.",
                "Na jó, menj aludni...",
                "Ez nem a te napod.",
                "Pfff. Szerintem ne is próbáld újra.",
                "Majdnem!",
                "Hogy lehetsz olyan rossz, mint ExAtom?",
                "Haver! Figyelj, csak játssz tovább és talán nyersz.",
                "Hú! Hallod, húzzá mán' haza te csóró! Ez a hely nem neked való.",
                "Kíváncsi lennék, hogy van-e olyan dolog amiben jó vagy."
            };

            Console.Write("\n\n ");
            Random random = new Random();
            Console.WriteLine(randomLose[random.Next(0, randomLose.Length)]);

            Console.Write(" Vesztettél");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($" ${bet}");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("-t!");
            Console.WriteLine("\n A folytatáshoz nyomj meg egy gombot...");

            Console.ReadKey(true);
        }
        static void Main()
        {
            Console.Title = "Low budget backjack";
            if (!File.Exists("game.saves"))
            {
                string[] saves = new string[9];
                for (int i = 0; i < saves.Length; i++) saves[i] = "-";
                File.WriteAllLines("game.saves", saves);
            }
            if (!File.Exists("temporary.txt")) File.WriteAllLines("temporary.txt", new string[1]);
            Menu();
        }
    }
}
