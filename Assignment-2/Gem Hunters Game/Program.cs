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



