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





