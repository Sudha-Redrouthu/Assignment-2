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

public class Player
{
    public string Name { get; private set; }
    public Position Position { get; private set; }
    public int GemCount { get; private set; }

    public Player(string name, Position startPosition)
    {
        Name = name;
        Position = startPosition;
        GemCount = 0;
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
}

public class Cell
{
    public string Occupant { get; set; }

    public Cell(string occupant)
    {
        Occupant = occupant;
    }
}

public class Board
{
    private Cell[,] grid;
    public Board()
    {
        grid = new Cell[6, 6];
        for (int x = 0; x < 6; x++)
        {
            for (int y = 0; y < 6; y++)
            {
                grid[x, y] = new Cell("-");
            }
        }

        grid[0, 0] = new Cell("P1");
        grid[5, 5] = new Cell("P2");

        Random random = new Random();
        PlaceRandomElements("G", random, 10);
        PlaceRandomElements("O", random, 5);
    }
    private void PlaceRandomElements(string element, Random random, int count)
    {
        for (int i = 0; i < count; i++)
        {
            int x, y;
            do
            {
                x = random.Next(6);
                y = random.Next(6);
            } while (grid[x, y].Occupant != "-");

            grid[x, y] = new Cell(element);
        }
    }

    public void Display()
    {
        for (int y = 0; y < grid.GetLength(1); y++)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                Console.Write(grid[x, y].Occupant + " ");
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
        return newX >= 0 && newX < 6 && newY >= 0 && newY < 6 && grid[newX, newY].Occupant != "O";
    }

    public void CollectGem(Player player)
    {
        if (grid[player.Position.X, player.Position.Y].Occupant == "G")
        {
            player.CollectGem();
            grid[player.Position.X, player.Position.Y].Occupant = "-";
        }
    }

    public void UpdatePlayerPosition(Player player, Position oldPosition)
    {
        grid[oldPosition.X, oldPosition.Y].Occupant = "-";
        grid[player.Position.X, player.Position.Y].Occupant = player.Name;
    }
}






