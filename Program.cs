using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;

namespace TicTacToe
{
    internal class Program
    {
        public static char[] boardPositions = { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        public static char playerCharacter = 'X';
        public static bool isGameEnded = false;
        public static int currentPlayer = 1;

        public static void DrawBoard() 
        {
            Console.Clear();
            Console.WriteLine("  -------------------------");
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine("  |       |       |       |");
                Console.Write("  |   ");
                for (int j = 0; j < 3; j++)
                {
                    char current = boardPositions[i * 3 + j];
                    if (current == 'X')
                    { 
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    if (current == '0')
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    
                    Console.Write(current);
                    Console.ResetColor();
                    Console.Write("   |   ");
                }
                Console.WriteLine();
                Console.WriteLine("  |       |       |       |");
                Console.WriteLine("  -------------------------");
            }
        }

        public static void Play(int player, int input)
        {
            if (player == 1) playerCharacter = 'X';
            else playerCharacter = '0';
            boardPositions[input - 1] = playerCharacter;
        }

        public static bool isPlayerWin(int input)
        {
            bool isWin = false;
            for (int i = 0; i < 3; i++) 
            {
                int xcount = 0;
                int ycount = 0;
                for (int j = 0; j < 3; j++)
                {
                    if (boardPositions[i * 3 + j] == input) xcount++;
                    if (boardPositions[j * 3 + i] == input) ycount++;
                }
                if (xcount == 3 || ycount == 3) isWin = true;
            }

            if ((boardPositions[0] == input && boardPositions[4] == input && boardPositions[8] == input) || 
                (boardPositions[2] == input && boardPositions[4] == input && boardPositions[6] == input)) isWin = true;
            
            return isWin;
        }

        public static bool isDraw()
        {
            int count = 0;
            for (int i = 0; i < 9; i++)
            { 
                if (boardPositions[i] == 'X' || boardPositions[i] == '0') count++;
            }
            
            return count == 9;
        }

        public static bool isValidMove(int userInput)
        {
            return ((userInput > 0 && userInput < 10) &&
                (boardPositions[userInput - 1] != 'X' && boardPositions[userInput - 1] != '0'));
        }

        public static void ShowWinnerScreen(int player)
        {
            if (player == 0)
            {
                Console.Clear();
                Console.WriteLine($"Draw!");
            }
            else 
            {
                Console.Clear();
                Console.WriteLine($"Player {player} win! Congratulations!");
                isGameEnded= true;
            }
            Console.WriteLine("Pess any key for reset...");
            Console.ReadKey();
            reset();
        }

        public static void checkWinner()
        {
            if (isDraw()) ShowWinnerScreen(0);
            if (isPlayerWin('X')) ShowWinnerScreen(1);
            if (isPlayerWin('0')) ShowWinnerScreen(2);
        }

        public static void reset()
        {
            boardPositions = new char[]{'1', '2', '3', '4', '5', '6', '7', '8', '9' };
        }

        public static int GetPlayerTurn(int player)
        {
            bool isValidInputGiven = false;
            int userInput = 0;
            while (!isValidInputGiven)
            {
                Console.WriteLine($"Player {player} - your move!");
                try
                {
                    userInput = Convert.ToInt16(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Pleae enter number between 1 and 9");
                    continue;
                }
                if (isValidMove(userInput)) isValidInputGiven = true; 
            }
            return userInput;
        }

        public static int getComputerTurn()
        {
            int[] options = boardPositions.Where(x => x != 'X' && x != 'O').Select(x => x - '0').ToArray();
            Random random= new Random();
            return random.Next(options.Length) + 1;
        }

        static void Main(string[] args)
        {
            reset();
            while (!isGameEnded)
            {
                DrawBoard();
                int userInput = 0;
                
                if (currentPlayer == 1) userInput = GetPlayerTurn(currentPlayer);
                else userInput = getComputerTurn();
                
                Play(currentPlayer, userInput);
                checkWinner();
                currentPlayer = currentPlayer == 1 ? 2 : 1;
            }            
        }
    }
}
