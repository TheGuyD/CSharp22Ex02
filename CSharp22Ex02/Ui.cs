using System;
using System.Text;
using System.Threading;


namespace CSharp22Ex02
{

    internal partial class Program
    {
       
        
        internal class Ui
        {
            private string m_NameOfFirstPlayer = string.Empty;
            private string m_NameOfSecondPlayer = string.Empty;
            private int m_MatrixRows = 6;
            private int m_MatrixColumns = 6;
            private eOpponent m_EOpponent = eOpponent.None;
            private GameLogic m_GameLogic = null;
            private string[] m_PrintMatrix = null;
            private string m_Move = null;
            private int[] m_PcMoves = null;
            


            public void PlayMemoryGame()
            {
                Start();
                restartMenu();
            }

            // Beginning of the program
            public void Start()
            {
                
                firstPlayerMessage();
                m_NameOfFirstPlayer = readPlayerName();
                chooseOpponentMassage();
                m_EOpponent = readOpponent();
                switch (m_EOpponent)
                {
                    case eOpponent.Pc:
                        ReadMatrixDimension();
                        m_GameLogic = new GameLogic(m_NameOfFirstPlayer,m_MatrixRows,m_MatrixColumns);
                        break;
                    case eOpponent.Player:
                        nameOfSecondPlayerMessage();
                        m_NameOfSecondPlayer = readPlayerName();
                        ReadMatrixDimension();
                        m_GameLogic = new GameLogic(m_NameOfFirstPlayer, m_NameOfSecondPlayer, m_MatrixRows, m_MatrixColumns);
                        break;
                }

                m_PrintMatrix = initiatePrintMatrix();
                round();
            }

            private void restartMenu()
            {
                const bool v_IsRestart = true;
                while (v_IsRestart)
                {
                    Console.WriteLine("Do you want to Restart ? 1-yes , press Q to quit game.");
                    string userInput = Console.ReadLine();

                    switch (userInput)
                    {
                        case "Q":
                            userPressedQToExist();
                            break;
                        case "1":
                            Restart();
                            break;
                        default:
                            RestartValidation();
                            Console.Clear();
                            break;
                    }
                }
            }
            
            public void RestartValidation()
            {
                Console.WriteLine("Please press 1 to restart or press Q to quit");
            }
            
            // New second Game Creation
            public void Restart()
            {
               Console.Clear();
                ReadMatrixDimension();
                m_GameLogic = (m_EOpponent == eOpponent.Player)
                                  ? new GameLogic(
                                      m_NameOfFirstPlayer,
                                      m_NameOfSecondPlayer,
                                      m_MatrixRows,
                                      m_MatrixColumns)
                                  : new GameLogic(m_NameOfFirstPlayer, m_MatrixRows, m_MatrixColumns);
                m_PrintMatrix = initiatePrintMatrix();
                round();
                restartMenu();
            }

            // Logic of round separator
            private void round()
            {
                if(m_EOpponent == eOpponent.Player)
                {
                    roundPlayerVsPlayer();
                }

                else
                {
                    roundPlayerVsPc();
                }
            }

            // Game scenario between Player an PC
            private void roundPlayerVsPc()
            {
                while (!m_GameLogic.IsWhiteMatrixFull())
                {

                    if(m_GameLogic.WhoPlayNow == eWhoPlay.PlayerOne)
                    {
                        roundPlayer();
                    }

                    else
                    {
                        roundPc();
                    }
                }

                finishGame();
            }

            // Game scenario between Player an Player
            private void roundPlayerVsPlayer()
            {
                while (!m_GameLogic.IsWhiteMatrixFull())
                {
                    roundPlayer(); 
                }

                finishGame();
            }

            // Round of Player moves
            private void roundPlayer()
            {
                for (int i = 0; i < 2; i++)
                {
                    playerMakeMoveMessage();
                    showMatrix();
                    m_Move = playerMakeMoveValidation();
                    m_GameLogic.Choose = stringInputConvertToInt();
                    m_GameLogic.Round();
                    copyToPrintMatrix();

                    //if there is first move, no need to do check
                    //if there is second move, need to check if it was good choice (find a pair)
                    if (i == 1)
                    {
                        bool isCellInWhiteMatrixWasDeleted = m_GameLogic.GetWhiteMatrix.WhiteMatrixChars[m_GameLogic.Choose[0], m_GameLogic.Choose[1]] == '\0';
 
                        if (isCellInWhiteMatrixWasDeleted)
                        {
                           Console.Clear();
                            showMatrix();
                            Thread.Sleep(2000);
                           Console.Clear();
                            deleteFromPrintMatrix();
                            break;
                        }

                       Console.Clear();
                        showMatrix();
                        Thread.Sleep(2000);
                       Console.Clear();
                        break;
                    }

                   Console.Clear();
                }
            }

            //Round of PC moves
            private void roundPc()
            {
                Console.WriteLine("PC makes move");
                Thread.Sleep(500);
                
                //receiving coordinate of PCs prediction
                m_PcMoves = m_GameLogic.PlayerPc.PcMakeMove();

                for (int i = 0; i < 2; i++)
                {
                    intInputToStringInput(i);
                    splitPcMove(i);
                    m_GameLogic.Round();
                    copyToPrintMatrix();
                    showMatrix();
                    
                    //if there is first move, no need to do check
                    //if there is second move, need to check if it was good choice (find a pair)
                    if(i == 1)
                    {
                        bool isCellEmptyInWhiteMatrix = m_GameLogic.GetWhiteMatrix.WhiteMatrixChars
                                                            [m_GameLogic.Choose[0], m_GameLogic.Choose[1]] == '\0';
 
                        if (isCellEmptyInWhiteMatrix)
                        {
                            Thread.Sleep(2000);
                           Console.Clear();
                            deleteFromPrintMatrix();
                            break;
                        }

                       Console.Clear();
                        showMatrix();
                        Thread.Sleep(2000);
                        break;
                    }

                   Console.Clear();
                }
            }

            // Convert PCs` steps into string for printing
            private void intInputToStringInput(int i_FirstPcMoveOrSecondPcMove)
            {
                m_Move = i_FirstPcMoveOrSecondPcMove == 0 ? $"{m_PcMoves[0] + 1}{(char)(m_PcMoves[1] + 'A')}" 
                                  : $"{m_PcMoves[2] + 1}{(char)(m_PcMoves[3] + 'A')}";
            }

            // Splitting Pc moves into divided steps
            private void splitPcMove(int i_FirstPcMoveOrSecondPcMove)
            {
                int index = i_FirstPcMoveOrSecondPcMove == 0 ? 0 : 2;
                m_GameLogic.Choose = new int[2] { m_PcMoves[index], m_PcMoves[index+1] };
            }

            //Terminating rhe program
            private  void userPressedQToExist()
            {
               Console.Clear();
                Console.WriteLine("Program was terminated");
                System.Environment.Exit(1);
            }

            private void firstPlayerMessage()
            {
                Console.WriteLine("Player 1 please enter your name:");
            }

            //Input Matrix dimension acording to rules of exercise
            public void ReadMatrixDimension()
            {
                // can be only int
                bool isNumberRows = false;
                bool isNumberCols = false;
                // must be even number od cells
                bool isEven = false;
                // not negative
                bool isPositive = false;
                // quantity of cells must be more then 15 and less then 37
                bool isInRange = false;
 
                while (!(isNumberRows && isNumberCols && isEven && isPositive && isInRange))
                {
                    messageInvalidDimension();
                    isNumberRows = int.TryParse(Console.ReadLine(), out m_MatrixRows);
                    isNumberCols = int.TryParse(Console.ReadLine(), out m_MatrixColumns);
                    isEven = (m_MatrixRows * m_MatrixColumns) % 2 == 0;
                    isPositive = m_MatrixColumns > 0 && m_MatrixRows > 0;
                    isInRange = (m_MatrixRows * m_MatrixColumns) >= 16 && (m_MatrixRows * m_MatrixColumns) <= 36;
                }
            }
 
            private void messageInvalidDimension()
            {
                Console.WriteLine("Please enter valid matrix dimensions");
            }

            private  void chooseOpponentMassage()
            {
                Console.WriteLine(
                    "Who do you want to be your opponent? Press 1 for play vs Player, press 2 for play vs PC");
            }
            
            //Deciding VS who first Player will play: Pc or second player
            private  eOpponent readOpponent()
            {
                while (true)
                {
                    string userInput = Console.ReadLine();
                    bool isNumber = int.TryParse(userInput, out int number);
 
                    if (isNumber)
                    {
                        eOpponent pcOrPlayer = (eOpponent)number;
                        bool isValidEopponent = pcOrPlayer == eOpponent.Pc || pcOrPlayer == eOpponent.Player;
 
                        if (isValidEopponent)
                        {
                            return pcOrPlayer;
                        }
                    }
 
                    Console.WriteLine("Please enter valid input , enter 1 for Player , 2 for Pc");
                }
            }
 
            private  void nameOfSecondPlayerMessage()
            {
                Console.WriteLine("Hello Player 2, please enter you name: ");
            }

            // Input of Players name: Only Letters
            private  string readPlayerName()
            {
                string strNameInput= Console.ReadLine();
                while (!Player.IsNameValid(strNameInput))
                {
                    Console.WriteLine("Please enter a valid name ");
                    strNameInput = Console.ReadLine();
                }
                return strNameInput;
            }

            //Inside Message of a Move stage
            private void playerMakeMoveMessage()
            {
                Console.WriteLine($"{m_GameLogic.PlayerWhoPlayNow.Name} choose card or press Q to exit");
            }
 
            // Validation for Players move: if Input 'Q' - termination of program. If else, method check it
            private string playerMakeMoveValidation()
            {
                string userInput = string.Empty;
                bool isValidUserInput = false;

                while (!isValidUserInput)
                {
                    userInput = Console.ReadLine();

                    if(userInput == "Q")
                    {
                        userPressedQToExist();
                    }

                    try
                    {
                        //syntax check
                        lengthValidation(userInput);
                        digitLatterValidation(userInput);
                        
                        //logical check
                        m_GameLogic.GetWhiteMatrix.MatrixBorderValidation(userInput);
                        m_GameLogic.GetWhiteMatrix.MatrixCellAvailable(userInput);
                        
                        isValidUserInput = true;
                    }

                    catch (Exception e)
                    {
                        Console.WriteLine("{0}", e.Message);
                        Console.WriteLine("Please enter move");
                    }
                }
 
                return userInput;
            }

            // Validation for Input: Length of string must be 2 
            private void lengthValidation(string i_UserInput)
            {
                bool isValidLength = i_UserInput.Length == 2;
                if (!isValidLength)
                {
                    throw new Exception("User input length is invalid");
                }
            }

            // Validation for Input: First char must be number, second - Big Letter
            private void digitLatterValidation(string i_IoUserInput)
            {
                bool isValidDigitLetter = char.IsDigit(i_IoUserInput[0]) && char.IsUpper(i_IoUserInput[1]);
                if (!isValidDigitLetter)
                {
                    throw new Exception("Users` input should be row as a number and column as a Upper English Letter");
                }
            }

            // Template for creating a matrix that will be displayed on the screen
            private string[] initiatePrintMatrix() 
            {
                char c = 'A';
                StringBuilder matrixToShow = new StringBuilder();
                matrixToShow.Insert(0, " ", 4);

                for (int i = 0; i < m_MatrixColumns; i++)
                {
                    matrixToShow.Append($"{c}   ");
                    c++;
                }
 
                for (int j = 0; j < m_MatrixRows; j++)
                {
                    matrixToShow.Append(Environment.NewLine);
                    matrixToShow.Insert(matrixToShow.Length, "  ", 1).Insert(matrixToShow.Length, "=", 4 * m_MatrixColumns + 1);
                    matrixToShow.Append(Environment.NewLine);
                    matrixToShow.Insert(matrixToShow.Length, (j + 1).ToString(), 1).Insert(matrixToShow.Length, " ", 1).Insert(matrixToShow.Length, "|   ", m_MatrixColumns).Append("|");
                }

                matrixToShow.Append('\n');
                matrixToShow.Insert(matrixToShow.Length, "  ", 1).Insert(matrixToShow.Length, "=", 4 * m_MatrixColumns + 1);
 
                return matrixToShow.ToString().Split('\n');
            }

            // Convert received string to coordinate of Matrix
            private int[] stringInputConvertToInt()
            {
                int rowCoordinate = m_Move[0] - '1';
                int colCoordinate = m_Move[1] - 'A';

                return new int[2] { rowCoordinate, colCoordinate };
            }
 
            //Method for inserting Letters to Console Matrix
            private void copyToPrintMatrix()
            {
                //rowNumber needs to be normalize to 1 based index so the user won't notice when guessing a card
                int rowNumber = (m_Move[0] - '0') * 2;
                StringBuilder toPrintRow = new StringBuilder(m_PrintMatrix[rowNumber]);
 
                //colNumber needs to be normalized to 1 based index so the user won't notice when guessing a card
                int collNumber = (m_Move[1] - 'A' + 1) * 4;
                toPrintRow[collNumber] = m_GameLogic.LetterFromResultMatrix;
                m_PrintMatrix[rowNumber] = toPrintRow.ToString();
            }
 
            //Method for deleting failed try from Console View
            private void deleteFromPrintMatrix()
            {
                int row = (m_GameLogic.Choose[0] + 1) * 2;
                StringBuilder toDeleteFromPrintRow = new StringBuilder(m_PrintMatrix[row]);
 
                int col = (m_GameLogic.Choose[1] + 1) * 4;
                toDeleteFromPrintRow[col] = ' ';
                m_PrintMatrix[row] = toDeleteFromPrintRow.ToString();
 
                row = (m_GameLogic.Choose[2] + 1) * 2;
                toDeleteFromPrintRow = new StringBuilder(m_PrintMatrix[row]);
                col = (m_GameLogic.Choose[3] + 1) * 4;
                toDeleteFromPrintRow[col] = ' ';
                m_PrintMatrix[row] = toDeleteFromPrintRow.ToString();
            }
 
            //Method for printing Matrix to Console
            private void showMatrix()
            {
                foreach (string row in m_PrintMatrix)
                {
                    Console.WriteLine(row);
                }
            }
 
            // Output the message about the winner
            private void finishGame()
            {
                Console.WriteLine($"{m_GameLogic.Players[0].Name}s` score is {m_GameLogic.Players[0].Score}");
                Console.WriteLine(
                    m_EOpponent == eOpponent.Player
                        ? $"{m_GameLogic.Players[1].Name}s` score is {m_GameLogic.Players[1].Score}"
                        : $"PCs` score is {m_GameLogic.PlayerPc.Score}");

                Console.WriteLine(
                    m_GameLogic.WhoWin == null ? "It's a Draw!" : $"{m_GameLogic.WhoWin} won this game");
            }
        }
    }
}