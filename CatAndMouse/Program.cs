using System;

namespace CatAndMouseGame
{
    public enum State
    {
        Winner,
        Looser,
        Playing,
        NotInGame
    }

    public class Player
    {
        public string name;
        public int location;
        public State state = State.NotInGame;
        public int distanceTraveled = 0;

        public Player(string name)
        {
            this.name = name;
            this.location = -1;
        }
    }

    public enum GameState
    {
        Start,
        End
    }

    public class Game
    {
        public int size;
        public Player cat;
        public Player mouse;
        public GameState state;

        public Game()
        {
            cat = new Player("Cat");
            mouse = new Player("Mouse");
            state = GameState.Start;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}