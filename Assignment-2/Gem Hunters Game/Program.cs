using System;

public class Position
{
    public int X { get; private set; }
    public int Y { get; private set; }

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }
}

public abstract class GameElement
{
    public Position Position { get; protected set; }

    public GameElement(int x, int y)
    {
        Position = new Position(x, y);
    }

    public abstract void Display();
}

public class Player : GameElement
{
    public string Name { get; private set; }
    public int GemCount { get; private set; }

    public Player(string name, int x, int y) : base(x, y)
    {
        Name = name;
    }

    public void Move(char direction)
    {
        switch (direction)
        {
            case 'U': Position = new Position(Position.X, Position.Y - 1); break;
            case 'D': Position = new Position(Position.X, Position.Y + 1); break;
            case 'L': Position = new Position(Position.X - 1, Position.Y); break;
            case 'R': Position = new Position(Position.X + 1, Position.Y); break;
        }
    }

    public void CollectGem()
    {
        GemCount++;
    }

    public override void Display()
    {
        Console.Write(Name);
    }
}

public class Gem : GameElement
{
    public Gem(int x, int y) : base(x, y) { }

    public override void Display()
    {
        Console.Write(" G ");
    }
}

public class Obstacle : GameElement
{
    public Obstacle(int x, int y) : base(x, y) { }

    public override void Display()
    {
        Console.Write(" O ");
    }
}

public class Board
{
    private GameElement[,] grid;

    public Board()
    {
        grid = new GameElement[6, 6];
        InitializeBoard();
    }


    public class Cell : GameElement
    {
        public Cell(int x, int y) : base(x, y) { }

        public override void Display()
        {
            Console.Write(" - ");
        }
    }

    private void InitializeBoard()
    {
        for (int x = 0; x < 6; x++)
        {
            for (int y = 0; y < 6; y++)
            {
                grid[x, y] = new Cell(x, y); // Corrected: Passing x and y coordinates
            }
        }

        grid[0, 0] = new Player("p1 ", 0, 0);
        grid[5, 5] = new Player(" p2", 5, 5);

        Random random = new Random();
        PlaceRandomElements(typeof(Gem), random, 10);
        PlaceRandomElements(typeof(Obstacle), random, 5);
    }



    private void PlaceRandomElements(Type elementType, Random random, int count)
    {
        for (int i = 0; i < count; i++)
        {
            int x, y;
            do
            {
                x = random.Next(6);
                y = random.Next(6);
            } while (grid[x, y] is not Cell || grid[x, y] is Player);

            grid[x, y] = (GameElement)Activator.CreateInstance(elementType, x, y);
        }
    }

    public void Display()
    {
        for (int y = 0; y < grid.GetLength(1); y++)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                if (grid[x, y] != null)
                    grid[x, y].Display();
                else
                    Console.Write("-");
                Console.Write(" ");
            }
            Console.WriteLine();
        }
    }

    public bool IsValidMove(Player player, char direction)
    {
        int newX = player.Position.X, newY = player.Position.Y;
        switch (direction)
        {
            case 'U': newY--; break;
            case 'D': newY++; break;
            case 'L': newX--; break;
            case 'R': newX++; break;
            default: return false;
        }
        return newX >= 0 && newX < 6 && newY >= 0 && newY < 6 && !(grid[newX, newY] is Obstacle);
    }

    public void CollectGem(Player player)
    {
        if (grid[player.Position.X, player.Position.Y] is Gem)
        {
            player.CollectGem();
            grid[player.Position.X, player.Position.Y] = new Cell(player.Position.X, player.Position.Y);
        }
    }

    public void UpdatePlayerPosition(Player player, Position oldPosition)
    {
        grid[oldPosition.X, oldPosition.Y] = new Cell(player.Position.X, player.Position.Y);
        grid[player.Position.X, player.Position.Y] = player;
    }
}

public class Game
{
    private Board board;
    private Player player1;
    private Player player2;
    private Player currentTurn;
    private int totalTurns;

    public Game()
    {
        board = new Board();
        player1 = new Player("p1 ", 0, 0);
        player2 = new Player(" P2", 5, 5);
        currentTurn = player1;
        totalTurns = 0;
    }

    public void Start()
    {
        while (!IsGameOver())
        {
            board.Display();
            Console.WriteLine($"{currentTurn.Name}'s turn. Enter move (U, D, L, R): ");
            char move = Console.ReadKey().KeyChar;
            Console.WriteLine();

            Position oldPosition = new Position(currentTurn.Position.X, currentTurn.Position.Y);
            if (board.IsValidMove(currentTurn, move))
            {
                board.UpdatePlayerPosition(currentTurn, oldPosition);
                currentTurn.Move(move);
                board.CollectGem(currentTurn);
                board.UpdatePlayerPosition(currentTurn, new Position(currentTurn.Position.X, currentTurn.Position.Y));
            }
            else
            {
                Console.WriteLine("Invalid move. Try again.");
            }
            SwitchTurn();
        }
        AnnounceWinner();
    }

    private void SwitchTurn()
    {
        currentTurn = currentTurn == player1 ? player2 : player1;
        totalTurns++;
    }

    private bool IsGameOver()
    {
        return totalTurns >= 30;
    }

    private void AnnounceWinner()
    {
        if (player1.GemCount > player2.GemCount)
            Console.WriteLine($"{player1.Name} wins with {player1.GemCount} gems!");
        else if (player2.GemCount > player1.GemCount)
            Console.WriteLine($"{player2.Name} wins with {player2.GemCount} gems!");
        else
            Console.WriteLine("It's a tie!");
    }
}

class Program
{
    static void Main(string[] args)
    {
        Game game = new Game();
        game.Start();
    }
}