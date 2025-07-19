//Project Name	: Eleven
//File Name		: Eleven
//Author		: Jason Wong
//Modified Date	: Feb 15 2017
//Description	: Plays the Card Game Eleven

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eleven
{
    class Program
    {
        //Fresh Deck
        static List<string> Cards = new List<string> {  "A", "K", "Q", "J", "10", "9", "8", "7", "6", "5", "4", "3", "2",
                                                        "A", "K", "Q", "J", "10", "9", "8", "7", "6", "5", "4", "3", "2",
                                                        "A", "K", "Q", "J", "10", "9", "8", "7", "6", "5", "4", "3", "2",
                                                        "A", "K", "Q", "J", "10", "9", "8", "7", "6", "5", "4", "3", "2"};
        //Track the End of the Deck
        static int EndCard;

        //Track the Number of Cards in the Pile and the Value
        static int[] StackNum = new int[12];
        static string[] StackValue = new string[12];

        //Tracks the Piles with Royals on Top
        static int[] RoyalPile = new int[12];

        //Tracks the Game State
        //"1" = On Going, "2" = Win, "3" = Lose
        static int GameState = 1;

        //For Loop Variable
        static int i;
        static int j;

        static void Main(string[] args)
        {
            //Inital Shuffle 
            ShuffleBoot();

            //Loops as long as the Program is Running
            while (true)
            {
                //Executes While Game is On-Going
                if (GameState == 1)
                {
                    UpdateList();
                    UserInput();
                    WinState();
                }
                //Excutes While Game is Finished
                else
                {
                    Console.Clear();

                    //Checks Whether User Won or Lose
                    if (GameState == 2)
                    {
                        Console.WriteLine("\nCongraulations, you won!\nWould you like to play again? Y/N");
                    }
                    else
                    {
                         Console.WriteLine("\nSorry, you lost!\nWould you like to play again? Y/N");
                    }

                    //Awaits for User Input
                    switch (Console.ReadLine().ToUpper())
                    {
                        case ("Y"):
                            //Put Cards in play Back into Deck
                            for (i = 0; i < StackValue.Length; i++)
                            {
                                Cards.Add(StackValue[i]);
                                StackNum[i] = 0;
                            }

                            //Resumes Game
                            ShuffleBoot();
                            GameState = 1;
                            break;
                        case ("N"):
                            //Exits Console
                            System.Environment.Exit(1);
                            break;
                        default:
                            //Invaild Inputs Filter Here
                            Console.WriteLine("Sorry, that is an invaild input\nPress ENTRE to continue");
                            Console.ReadLine();
                            break;
                    }
                }
            }
        }

        //Pre: None
        //Post: Replace Cards in Play upon Vaild Inputs 
        //Description: Executes Option One and Two
        static private void UserInput()
        {
            Console.WriteLine("\n\n" + "Choose an option by their designated number:\n1) Move a Face Card\n2) State an Eleven Pair");

            //Checks for which Option was Choosen
            switch (Console.ReadLine())
            {
                //Move a Face Card
                case ("1"):
                    Console.WriteLine("\nChoose a pile by its designated letter");

                    //Find which Pile the User is Choosing
                    int TempFace = AlphabetToNum(Console.ReadLine().ToUpper());

                    //Filters Out Inputs with No Designated Pile
                    if (TempFace != 12)
                    {
                        //Filters Out Inputs with Designated Piles which aren't Royals
                        if (StackValue[TempFace] == "J" || StackValue[TempFace] == "Q" || StackValue[TempFace] == "K" && StackNum[TempFace] == 0)
                        {
                            //Replace Royal with a New Card
                            Cards.Insert(EndCard, StackValue[TempFace]);
                            StackValue[TempFace] = Cards[0];
                            Cards.RemoveAt(0);
                        }
                        else
                        {
                            //Vaild Piles which aren't Royals Filter Here
                            Console.WriteLine("\n\nSorry, that is an invalid pile letter to modify\nPress ENTER to continue");
                            Console.ReadLine();
                        }
                    }
                    else
                    {
                        //Invaild Inputs Filter Here
                        Console.WriteLine("\n\nSorry, no pile is designated by that input\nPress ENTER to continue");
                        Console.ReadLine();
                    }
                    break;
                //State an Eleven Pair
                case ("2"):
                    //Potential Eleven Pair Values
                    int[] TempPair = new int[2];
                    int[] TempPile = new int[2];

                    for (i = 0; i < 2; i++)
                    {
                        if (i == 0)
                        {
                            Console.WriteLine("\nChoose the first pile by its designated letter");
                        }
                        else
                        {
                            Console.WriteLine("\nChoose the second pile by its designated letter");
                        }

                        //Find which Pile the User is Choosing
                        TempPile[i] = AlphabetToNum(Console.ReadLine().ToUpper());

                        //Filters Out Inputs with No Designated Pile
                        if (TempPile[i] != 12)
                        {
                            //Filters Out Inputs with Designated Piles which are Royals
                            if (StackValue[TempPile[i]] != "J" && StackValue[TempPile[i]] != "Q" && StackValue[TempPile[i]] != "K")
                            {
                                //Convert Ace into a Value of One, Otherelse Store Input for Further Calculations
                                if (StackValue[TempPile[i]] == "A")
                                {
                                    TempPair[i] = 1;
                                }
                                else
                                {
                                    TempPair[i] = Convert.ToInt32(StackValue[TempPile[i]]);
                                }
                            }
                            else
                            {
                                //Vaild Piles which are Royals Filter Here
                                Console.WriteLine("\n\nSorry, that is an invalid pile letter to modify\nPress ENTER to continue");
                                Console.ReadLine();
                                break;
                            }
                        }
                        else
                        {
                            //Invaild Inputs Filter Here
                            Console.WriteLine("\n\nSorry, no pile is designated by that input\nPress ENTER to continue");
                            Console.ReadLine();
                            break;
                        }

                        //Only Checks Eleven Pair if Both Pairs are Vaild
                        if (i == 1)
                        {
                            //Checks for Vaild Pairs
                            if (TempPair[0] + TempPair[1] == 11 )
                            {
                               for (i = 0; i < 2; i++)
                                {
                                    //Replace Pair with New Cards
                                    Cards.Insert(EndCard, StackValue[TempPile[i]]);
                                    StackValue[TempPile[i]] = Cards[0];
                                    Cards.RemoveAt(0);
                                    StackNum[TempPile[i]]++;
                                    EndCard--;
                                }
                            }
                            else
                            {
                                //Invaild Pairs Filter Here
                                Console.WriteLine("\nSorry, these two piles do not add up to eleven\nPress ENTER to continue");
                                Console.ReadLine();
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        //Pre: None
        //Post: Randomize the Order of the Playing Cards in the List
        //Description: Shuffles Deck
        static private void ShuffleBoot()
        {
            //Random Number Generator Variables
            int Temp;
            Random rng = new Random();

            //Places First Element of the List back into List at Random Locations 200 Times
            for (i = 0; i < 200; i++)
            {   
                Temp = rng.Next(0,Cards.Count-1);
                Cards.Add(Cards[Temp]);
                Cards.RemoveAt(Temp);
            }

            //Deal Cards into Play
            for (i = 0; i < StackValue.Length; i++)
            {
                StackValue[i] = Cards[0];
                Cards.RemoveAt(0);
            }

            EndCard = Cards.Count;
        }

        //Pre: None
        //Post: Display characteristics of Piles in play after an Adjustment
        //Decription: Updates Cards in Play
        static private void UpdateList()
        {
            Console.Clear();
            //Update Cards in Play
            for (i = 0; i < StackValue.Length; i++)
            {
                if (i == 6)
                {
                    Console.WriteLine("\n");
                }

                //Change Color if Royals
                if (StackValue[i] == "J" || StackValue[i] == "Q" || StackValue[i] == "K")
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }

                Console.Write(StackValue[i] + "[" + (char)(i + 'A') + "-" + (StackNum[i] + 1) + "]\t");
                Console.ResetColor();
            }    
        }

        //Pre: InputRequest is a Vaild String Input
        //Post: Returns the Number Corresponding to the Alphabet in Integer
        //Decription: Converts Alphabets into Numbers (Excluding Ace, Jack, Queen and King)
        static private int AlphabetToNum(string InputRequest)
        {
            //Matches Alphabets with their Corresponding Number
            switch (InputRequest)
            {
                case ("A"):
                    return 0;
                case ("B"):
                    return 1;
                case ("C"):
                    return 2;
                case ("D"):
                    return 3;
                case ("E"):
                    return 4;
                case ("F"):
                    return 5;
                case ("G"):
                    return 6;
                case ("H"):
                    return 7;
                case ("I"):
                    return 8;
                case ("J"):
                    return 9;
                case ("K"):
                    return 10;
                case ("L"):
                    return 11;
                default:
                    //Invaild Inputs Filter Here
                    return 12;
            }
        }

        //Pre: None
        //Post: Changes Game State whenever Win/Lose Conditions are met
        //Description: Tracks Game State
        static private void WinState()
        {
            //Tracks how many Potential Pairs are in play
            int PairCounter = 0;
            //Tracks how many Royals are in play
            int sum = 0;

            //Confirms a Royal Pile
            for (i = 0; i < StackValue.Length; i++)
            {
                //Only Piles with "J", "Q" or "K" Contributes to the Value of "sum"
                if (StackValue[i] == "J" || StackValue[i] == "Q" || StackValue[i] == "K")
                {
                    RoyalPile[i] = 1;
                }
                else
                {
                    RoyalPile[i] = 0;
                }

                sum += RoyalPile[i];
            }

            //Checks if all piles Contributed to the Value of "sum"
            if (sum == 12)
            {
                GameState = 2;
            }
            else
            {
                //Checks for Potential Pairs in play
                for (i = 0; i < StackValue.Length; i++)
                {
                    //Only Checks with Leading Piles (Previous Piles have already been Checked)
                    for (j = i + 1; j < StackValue.Length; j++)
                    {
                        //Filters Out Piles with Designated Piles which are Royals
                        if (StackValue[i] != "J" && StackValue[i] != "Q" && StackValue[i] != "K" && StackValue[j] != "J" && StackValue[j] != "Q" && StackValue[j] != "K")
                        {
                            //Manually Convert Ace into 1
                            if (StackValue[i] == "A")
                            {
                                if (StackValue[j] != "A")
                                {
                                    if (1 + Convert.ToInt32(StackValue[j]) == 11)
                                    {
                                        PairCounter++;
                                    }
                                }
                            }
                            //Manually Convert Ace into 1
                            else if (StackValue[j] == "A")
                            {
                                if (StackValue[i] != "A")
                                {
                                    if (1 + Convert.ToInt32(StackValue[i]) == 11)
                                    {
                                        PairCounter++;
                                    }
                                }
                            }
                            //Adds Piles together to Find Eleven Pairs
                            else
                            {
                                if (Convert.ToInt32(StackValue[i]) + Convert.ToInt32(StackValue[j]) == 11)
                                {
                                    PairCounter++;
                                }
                            }
                        }
                    }
                }

                //Changes State if there are no longer any Potential Pairs
                if (PairCounter > 0)
                {
                    GameState = 1;
                }
                else
                {
                    GameState = 3;
                }
            }
        }
    }
}