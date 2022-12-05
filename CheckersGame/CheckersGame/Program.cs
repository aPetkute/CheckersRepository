// See https://aka.ms/new-console-template for more information
Dictionary<char, int> positionLetterToInt = new Dictionary<char, int>();
for (int i = 0; i < 8; i++) { positionLetterToInt.Add((char)(i + 97), i); }
//changes
const char PLAYER1SYMBOL = 'X';
const char PLAYER1ROYAL = 'K';
const char PLAYER2SYMBOL = 'O';
const char PLAYER2ROYAL = 'Q';
const char EMPTYSQUARE = '·';

Console.SetWindowSize(75, 35);

List<List<char>> board = new List<List<char>>();
int Xs = 12, Os = 12;
SetupBoard(board);
Play(board);

void DisplayBoard(List<List<char>> chars)
{
    Console.Clear();
    Console.WriteLine(" ~ C H E C K E R S ~ ");
    Console.WriteLine(" Press Q to quit!");
    Console.WriteLine("Player1 has " + Xs + " pieces left, Player2 has " + Os + " pieces left.");
    Console.WriteLine();
    Console.WriteLine("    |  A  ||  B  ||  C  ||  D  ||  E  ||  F  ||  G  ||  H  |");
    for (int i = 0; i < 8; i++)
    {
        Console.WriteLine("─── ┌─────┐┌─────┐┌─────┐┌─────┐┌─────┐┌─────┐┌─────┐┌─────┐");
        Console.Write(" " + (i + 1) + "  ");
        for (int j = 0; j < 8; j++)
        {
            Console.Write("|  " + chars[i][j] + "  |");
        }
        Console.WriteLine();
        Console.WriteLine("─── └─────┘└─────┘└─────┘└─────┘└─────┘└─────┘└─────┘└─────┘");
    }
    Console.WriteLine();
}
void SetupBoard(List<List<char>> board)
{
    string[] data = { " x x x x", "x x x x ", " x x x x", "· · · · ", " · · · ·", "o o o o ", " o o o o", "o o o o " };
    string[] data1 = { " "+PLAYER1SYMBOL+" "+PLAYER1SYMBOL+" "+PLAYER1SYMBOL +" "+PLAYER1SYMBOL,
                    PLAYER1SYMBOL+" "+ PLAYER1SYMBOL+" "+ PLAYER1SYMBOL+" "+ PLAYER1SYMBOL+" ",
                    " "+PLAYER1SYMBOL+" "+PLAYER1SYMBOL+" "+PLAYER1SYMBOL +" "+PLAYER1SYMBOL,
                    EMPTYSQUARE+" "+EMPTYSQUARE+" "+EMPTYSQUARE+" "+EMPTYSQUARE+" ",
                    " "+EMPTYSQUARE+" "+EMPTYSQUARE+" "+EMPTYSQUARE+" "+EMPTYSQUARE,
                    PLAYER2SYMBOL+" "+ PLAYER2SYMBOL+" "+ PLAYER2SYMBOL+" "+ PLAYER2SYMBOL+" ",
                    " "+PLAYER2SYMBOL+" "+PLAYER2SYMBOL+" "+PLAYER2SYMBOL +" "+PLAYER2SYMBOL,
                    PLAYER2SYMBOL+" "+PLAYER2SYMBOL+" "+PLAYER2SYMBOL +" "+PLAYER2SYMBOL+" "};
    foreach (string d in data1)
    {
        List<char> datalist = new List<char>();
        foreach (char c in d)
        {
            datalist.Add(c);
        }
        board.Add(datalist);
    }
}
void Play(List<List<char>> board)
{
    DisplayBoard(board);
    string position = " ";
    string newPosition;
    bool turn = true;
    char turnSymbol = PLAYER1SYMBOL;
    int x = 0, y = 0, x1 = 0, y1 = 0;

    while (position.ToLower() != "q" && (Xs != 0 && Os != 0))
    {
        Console.Write(" ~ Player ");
        if (turn) Console.Write("1 (" + PLAYER1SYMBOL + ")");
        else Console.Write("2 (" + PLAYER2SYMBOL + ")");
        Console.Write(", your turn! ~\n");

        position = GetWhichPiece(board, turn);
        newPosition = GetNewPosition(board);
        if (position == "q" || newPosition == "q") { break; }

        //Console.WriteLine(position + " to " + newPosition + " is valid: " + CheckIfValidMove(position, newPosition, turn));
        x = ConvertPositionToFirstIndex(position);
        y = ConvertPositionToSecondIndex(position);
        if (board[x][y] == 'K' || board[x][y] == 'Q')
        {
            RoyalMovement(board, position, newPosition, turn);
        }
        else
        {
            bool CaptureValidity = CheckIfValidCapture(board, position, newPosition, turn);
            while (!CheckIfValidMove(position, newPosition, turn) && !CaptureValidity)
            {
                Console.WriteLine("Invalid move. Try again");
                position = GetWhichPiece(board, turn);
                newPosition = GetNewPosition(board);
                if (position == "q" || newPosition == "q") { break; }
                CaptureValidity = CheckIfValidCapture(board, position, newPosition, turn);
            }
            x = ConvertPositionToFirstIndex(position);
            y = ConvertPositionToSecondIndex(position);
            x1 = ConvertPositionToFirstIndex(newPosition);
            y1 = ConvertPositionToSecondIndex(newPosition);
            if (CaptureValidity)
            {
                board[(x + x1) / 2][(y + y1) / 2] = '·';
                if (turn) Os--;
                else Xs--;
            }
            board[x][y] = '·';
            board[x1][y1] = turnSymbol;

        }


        if (turn && x1 == 7) board[x1][y1] = 'K';
        if (!turn && x1 == 0) board[x1][y1] = 'Q';

        turn = !turn;
        if (turnSymbol == PLAYER1SYMBOL) turnSymbol = PLAYER2SYMBOL;
        else turnSymbol = PLAYER1SYMBOL;

        DisplayBoard(board);
    }
    if (Xs == 0) Console.WriteLine("Congradulations PLayer 2 (" + PLAYER2SYMBOL + ")!");
    else if (Os == 0) Console.WriteLine("Congradulations PLayer 1 (" + PLAYER1SYMBOL + ")!");
    Console.Write("END OF GAME . . .");
}
bool CheckIfInputValid(string input)
{
    if (input.Count() != 2) return false;
    input = input.ToLower();
    if (input[0] > '0' && input[0] < '9' && input[1] >= 'a' && input[1] <= 'h')
        return true;
    else return false;
}
string GetWhichPiece(List<List<char>> board, bool turn)
{
    Console.Write("\nChoose which piece to move: enter a number and a letter (ex. 3D) \n Piece: ");
    string position = Console.ReadLine();
    position = position.ToLower();
    if (position == "q") { return position; }
    while (!CheckIfInputValid(position))
    {
        Console.WriteLine("Invalid input, try again!");
        position = Console.ReadLine();
    }
    int x = ConvertPositionToFirstIndex(position);
    int y = ConvertPositionToSecondIndex(position);
    char symbol = ' ';
    char RoyalSymbol = ' ';
    if (turn)
    {
        symbol = PLAYER1SYMBOL;
        RoyalSymbol = PLAYER1ROYAL;
    }
    else
    {
        symbol = PLAYER2SYMBOL;
        RoyalSymbol = PLAYER2ROYAL;
    }
    while (board[x][y] != symbol && board[x][y] != RoyalSymbol)
    {
        Console.WriteLine("Choose a position with " + symbol + " or " + RoyalSymbol + " !");
        position = Console.ReadLine();
        x = ConvertPositionToFirstIndex(position);
        y = ConvertPositionToSecondIndex(position);
    }

    return position;
}
string GetNewPosition(List<List<char>> board)
{
    Console.Write(" Move to: ");
    string newPosition = Console.ReadLine();
    newPosition = newPosition.ToLower();
    if (newPosition == "q") { return newPosition; }
    while (!CheckIfInputValid(newPosition))
    {
        Console.WriteLine("Invalid input, try again!");
        newPosition = Console.ReadLine();
    }
    int x = ConvertPositionToFirstIndex(newPosition);
    int y = ConvertPositionToSecondIndex(newPosition);

    while (board[x][y] != EMPTYSQUARE)
    {
        Console.WriteLine("Choose an empty position with " + EMPTYSQUARE + "!");
        newPosition = Console.ReadLine();
        x = ConvertPositionToFirstIndex(newPosition);
        y = ConvertPositionToSecondIndex(newPosition);
    }
    return newPosition;
}
int ConvertPositionToFirstIndex(string pos)
{
    int index = pos[0] - 48 - 1;
    return index;
}
int ConvertPositionToSecondIndex(string pos)
{
    int index = positionLetterToInt[pos[1]];
    return index;
}
bool CheckIfValidMove(string pos, string newPos, bool turn)
{
    int x = ConvertPositionToFirstIndex(pos);
    int y = ConvertPositionToSecondIndex(pos);
    int x1 = ConvertPositionToFirstIndex(newPos);
    int y1 = ConvertPositionToSecondIndex(newPos);
    if (y1 == y + 1 || y1 == y - 1)
    {
        if (turn && x1 == x + 1)
            return true;
        else if (!turn && x1 == x - 1)
            return true;
        else return false;
    }
    else return false;

}
bool CheckIfValidCapture(List<List<char>> board, string pos, string newPos, bool turn)
{
    int x = ConvertPositionToFirstIndex(pos);
    int y = ConvertPositionToSecondIndex(pos);
    int x1 = ConvertPositionToFirstIndex(newPos);
    int y1 = ConvertPositionToSecondIndex(newPos);

    if (y1 == y + 2 || y1 == y - 2)
    {
        char temp = board[(x + x1) / 2][(y + y1) / 2];
        if (turn && x1 == x + 2 && board[(x + x1) / 2][(y + y1) / 2] == PLAYER2SYMBOL)
            return true;
        else if (!turn && x1 == x - 2 && board[(x + x1) / 2][(y + y1) / 2] == PLAYER1SYMBOL)
            return true;

        else return false;
    }
    else return false;
}
void RoyalMovement(List<List<char>> board, string pos, string newPos, bool turn)
{
    int x = ConvertPositionToFirstIndex(pos);
    int y = ConvertPositionToSecondIndex(pos);
    int x1 = ConvertPositionToFirstIndex(newPos);
    int y1 = ConvertPositionToSecondIndex(newPos);
    char NME = ' ', ME = ' ';
    int originalx = x;
    int originaly = y;
    if (turn)
    {
        ME = PLAYER1SYMBOL;
        NME = PLAYER2SYMBOL;
    }
    else
    {
        ME = PLAYER2SYMBOL;
        NME = PLAYER1SYMBOL;
    }
    bool jumpingOverFriend = false;

    if (Math.Abs(x - x1) == Math.Abs(y - y1))
    {
        int countNMEpieces = 0;
        if (x > x1 && y > y1)
        {
            x--;
            y--;
            while (x != x1)
            {
                if (board[x][y] == ME)
                {
                    jumpingOverFriend = true;
                    break;
                }
                if (board[x][y] == NME)
                {
                    board[x][y] = EMPTYSQUARE;
                    countNMEpieces++;
                }


                x--;
                y--;
            }
        }
        else if (x > x1 && y < y1)
        {
            x--;
            y++;
            while (x != x1)
            {
                if (board[x][y] == ME)
                {
                    jumpingOverFriend = true;
                    break;
                }
                if (board[x][y] == NME)
                {
                    board[x][y] = EMPTYSQUARE;
                    countNMEpieces++;
                }

                x--;
                y++;
            }
        }
        else if (x < x1 && y < y1)
        {
            x++;
            y++;
            while (x != x1)
            {
                if (board[x][y] == ME)
                {
                    jumpingOverFriend = true;
                    break;
                }
                if (board[x][y] == NME)
                {
                    board[x][y] = EMPTYSQUARE;
                    countNMEpieces++;
                }

                x++;
                y++;
            }
        }
        else if (x < x1 && y > y1)
        {
            x++;
            y--;
            while (x != x1)
            {
                if (board[x][y] == ME)
                {
                    jumpingOverFriend = true;
                    break;
                }
                if (board[x][y] == NME)
                {
                    board[x][y] = EMPTYSQUARE;
                    countNMEpieces++;
                }

                x++;
                y--;
            }
        }
        if (jumpingOverFriend)
        {
            Console.WriteLine("Invalid royal movement: cannot jump over friendly pieces:)");
            Thread.Sleep(3500);

        }
        else
        {
            if (turn) Os -= countNMEpieces;
            else Xs -= countNMEpieces;
            board[x1][y1] = board[originalx][originaly];
            board[originalx][originaly] = EMPTYSQUARE;
        }
    }
    else Console.WriteLine("Invalid royal movement:)");
    Thread.Sleep(3500);
}