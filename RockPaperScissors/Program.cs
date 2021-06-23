/**
 * Rock Paper Scissors Game
 * Individual game statistics are stored in a remote database
 * 
 * 
 * 
 * John McKinney  22/06/2021
 * 
 * 
 */



using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using RockPaperScissors.Models;

namespace RockPaperScissors
{
    class Program
    {
        private static IConfiguration _iconfiguration;
        private static String[] option = new string[3] { "ROCK", "PAPER", "SCISSORS" };
        private static String[] action = new string[3] { "blunts", "covers", "cut" };
        private static string firstName;

        static void Main(string[] args)
        {
            GetAppSettingsFile();

            Console.WriteLine("\n\tRock Paper Scissors!\n\n");
            Console.WriteLine("Type in your first name\n");
            firstName = Console.ReadLine();

            Console.WriteLine($"Welcome {firstName}\n");

            Menu();

        }


    /**
     * 
     *   DISPLAY MAIN MENU
     * 
     */

        static void Menu()
        {
            char choice;
            do
            {
                Console.WriteLine("\n\t----MAIN MENU-----");
                Console.WriteLine("\t1 - Play Game");
                Console.WriteLine("\t2 - View Stats");               
                Console.WriteLine("\t3 - Exit Program");
                Console.WriteLine("\nPress a number");

                do
                    choice = Console.ReadKey().KeyChar;
                while (!Char.IsDigit(choice));

                switch (choice)
                {

                    case '1':
                        PlayComputer();
                        break;
                    case '2':
                        ViewAllStats();
                        break;

                    case '3':
                        Console.WriteLine("Thank you for using the application. Goodbye");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Please choose 1 - 3 only");
                        break;
                }


            } while (choice != '3');
        }


         /**
          * 
          *   method to play computer 
          * 
          */

        static void PlayComputer()
        {
            var rand = new Random();
            bool computerWins,success;
            int compChoiceNum, playerChoiceNum, count;
            string compChoice, playerChoice,input;




            //stats can be computer wins, player wins, draws and number of rock/ paper/ scissor occurences
            int[] stats = new int[6] { 0, 0, 0, 0, 0, 0 };



            //need to enter an integer
            int numberOfTurns;// = Convert.ToInt32(Console.ReadLine());
            do
            {
                Console.WriteLine("\nEnter number of turns for this round\n");
                input = Console.ReadLine();
                success = Int32.TryParse(input, out numberOfTurns);
            } while (!success);


            for (count = 0; count < numberOfTurns; count++)
            {

                //in a "best of" situation there comes a point when either player may not be able to win
                //here we check if there are enough turns left to attain victory
                //
                if((numberOfTurns - count) < (stats[0] - stats[1]))
                {
                    
                    Console.WriteLine($"\nNumber of turns remaining: {numberOfTurns - count}");
                    Console.WriteLine($"Computer lead: {stats[0] - stats[1]}");
                    break;

                }
                else if ((numberOfTurns - count) < (stats[1] - stats[0]))
                {
                    
                    Console.WriteLine($"\nNumber of turns remaining: {numberOfTurns - count}");
                    Console.WriteLine($"Your lead: {stats[1] - stats[0]}");
                    break;

                }

                Console.WriteLine($"\n\tTURN NUMBER {count + 1}\n");




                //========================================================================
                //============================= generate computer choice =================
                //========================================================================

                computerWins = false;
                compChoiceNum = rand.Next(3);
                compChoice = option[compChoiceNum];

                //record the number of rock/paper/scissors choices by the computer
                //e.g. if the computer chooses 0 for rock - we need to shift index by 3
                stats[compChoiceNum + 3]++;


                //========================================================================
                //============================= input player choice ======================
                //========================================================================

                do
                {

                    // a way of allowing the user to strike a key to play?
                    //need to control user input 

                    do
                    {
                        Console.WriteLine("Enter 1, 2 or 3 for Rock, Paper or Scissors\n");
                        input = Console.ReadLine();
                        success = Int32.TryParse(input, out playerChoiceNum);
                    } while (!success);

                    playerChoiceNum--;  //adjust for index

                } while (playerChoiceNum < 0 || playerChoiceNum > 2);  //loop until selection is within range

                //record the number of rock/paper/scissors choices by the player          
                stats[playerChoiceNum + 3]++;  
                playerChoice = option[playerChoiceNum];


                //========================================================================
                //============================= game logic ===============================
                //========================================================================

                if (!playerChoice.Equals(compChoice))   //if it's not a draw.......
                {
                    switch (playerChoice)
                    {
                        case "ROCK":
                            computerWins = compChoice.Equals("PAPER");                            
                            break;
                        case "PAPER":
                            computerWins = compChoice.Equals("SCISSORS");                           
                            break;
                        case "SCISSORS":
                            computerWins = compChoice.Equals("ROCK");                            
                            break;
                        //default:
                            // flag error condition because choice was not a valid option
                            //playerChoiceNum = -1;
                           // break;

                    }


                    //=======================================================================
                    //============================ display result ===========================
                    //=======================================================================


                    if (computerWins)
                    {

                        Console.WriteLine("\nBad luck, you lost");
                        Console.WriteLine("Computer chose " + compChoice);
                        Console.WriteLine("You chose " + playerChoice);
                        
                        Console.WriteLine($"{compChoice} {action[compChoiceNum]} {playerChoice}");
                        stats[0]++;

                    }
                    else 
                    {

                        Console.WriteLine("\nCongratulations, you won");
                        Console.WriteLine("You chose " + playerChoice);
                        Console.WriteLine("Computer chose " + compChoice);

                        Console.WriteLine($"{playerChoice} {action[playerChoiceNum]} {compChoice}");
                        stats[1]++;

                    }

                }
                else     // it is a draw......
                {

                    Console.WriteLine("\nDraw!");
                    Console.WriteLine("You chose " + playerChoice);
                    Console.WriteLine("Computer chose " + compChoice);
                    stats[2]++;
                }

            }


            //=======================================================================
            //====================== display round summary messages =================
            //=======================================================================
            GameStats gameStats = new GameStats()
            { 
                Turns = count,
                MostMoves = HighSelect(stats),
                Date = DateTime.Now
                
            };
            Console.WriteLine("\n--------- END OF ROUND ------------\n");


            //===============================================================================
            //============================= display game stats ===============================
            //===============================================================================
            Console.WriteLine("------------- GAME STATS -------------------");
            Console.WriteLine("comp\tplayer\tdraws\trock\tpaper\tscissors");
            for (int j = 0; j < 6; j++)
            {
                Console.Write($"{stats[j]}\t");
            }
            Console.WriteLine("\n");
            Console.WriteLine("---------------------------------------------");




            //Console.WriteLine(gameStats.Date);
            if (stats[0] > stats[1])
            {
                Console.WriteLine("\tComputer WINS.");
                gameStats.Winner = "Computer";
            }
            else if (stats[0] < stats[1])
            {
                Console.WriteLine("\tYou WIN.");
                gameStats.Winner = firstName;
            }
            else if (stats[0] == stats[1])
            {
                Console.WriteLine("\tIt's a DRAW!.");
                gameStats.Winner = "DRAW";
            }
            Console.WriteLine($"\n\tRound finished in {count} turns");
            Console.WriteLine($"\n\tMost used move:  {HighSelect(stats)} used " +
                $"{Math.Max(stats[3], Math.Max(stats[4], stats[5]))} times\n");

            if (SaveGameStats(gameStats)) Console.WriteLine("Stats Saved.");

            Console.WriteLine("\n-----------------------------------\n");

            

        }

        /**
         *
         * get the max of the stats array last three elements (ROCK, PAPER, SCISSORS)
         * <param name="stats">Used to find most used move</param>
         */
        static string HighSelect(int[] stats)
        {
     
            //use it to select the most used move
            int max = 0;
            int selector = 0;

            //loop through the last 3 elements of the stats array to find the highest
            for (int i = 3; i < 6; i++)
            {
                if (stats[i] > max)
                {
                    max = stats[i];
                    selector = i - 3; // record the selected move (offset 3)
                }

            }

            return option[selector];
        }


        /**
         *    Save the current game stats in the DB
         */
        static bool SaveGameStats(GameStats stats)
        {
            var statsData = new DBAccess(_iconfiguration);
            return statsData.AddGameStats(stats);

        }


        /**
         *    Display the previous game stats as saved in the DB
         */
        static void ViewAllStats()
        {
            Console.WriteLine("------------- VIEW ALL PREVIOUS GAME STATS ---------------\n");
            Console.WriteLine("Winner\tMost Moves\tTurns\tDate");
            Console.WriteLine("----------------------------------------------------------\n");

            //obtain a db connection and get data
            var statsData = new DBAccess(_iconfiguration);          
            var listGameStats = statsData.GetGameStats();
            string format = "{0,-10} {1,-14} {2,-5} {3,-10}";
            //display the data
            listGameStats.ForEach(item =>
            {
                string[] row = new string[] { item.Winner, item.MostMoves, item.Turns.ToString(), item.Date.ToString()};
      
                Console.WriteLine(string.Format(format, row));
            });
            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();


        }

        //get the connection string
        static void GetAppSettingsFile()
        {
            var builder = new ConfigurationBuilder()
                                 .SetBasePath(Directory.GetCurrentDirectory())
                                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            _iconfiguration = builder.Build();
        }


    }
}
