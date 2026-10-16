using System;
using System.IO;

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

        public void Move(int steps, int boardSize)
        {
            if (state == State.NotInGame)
            {
                location = ((steps - 1) % boardSize + boardSize) % boardSize + 1;
                state = State.Playing;
            }
            else if (state == State.Playing && steps != 0)
            {
                distanceTraveled += Math.Abs(steps);
                location = ((location - 1 + steps) % boardSize + boardSize) % boardSize + 1;
            }
        }
    }

    public enum GameState
    {
        Start,
        End
    }

    public class Game
    {
        public static string InputFile = "1.ChaseData.txt";
        public static string OutFile = "1.PursuitLog.txt";

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

        public void Run()
        {
            File.WriteAllText(OutFile, "Cat and Mouse\n\nCat   Mouse Distance\n-------------------\n");
            string[] lines = File.ReadAllLines(InputFile);
            size = int.Parse(lines[0].Trim());

            foreach (string line in lines)
            {
                if (state == GameState.End) break;

                string[] parts = line.Trim().Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 0) continue;

                char command = parts[0].ToUpper()[0];

                if ((command == 'M' || command == 'C') && parts.Length > 1 && int.TryParse(parts[1], out int steps))
                {
                    if (command == 'M') mouse.Move(steps, size);
                    else if (command == 'C') cat.Move(steps, size);

                    if (cat.state == State.Playing && mouse.state == State.Playing && cat.location == mouse.location)
                    {
                        cat.state = State.Winner;
                        mouse.state = State.Looser;
                        state = GameState.End;
                    }
                }
                else if (command == 'P')
                {
                    DoPrintCommand();
                }
            }

            state = GameState.End;

            string footer = "-------------------\n\n\nDistance traveled: Mouse Cat\n" +
                           $" {mouse.distanceTraveled,-14} {cat.distanceTraveled}\n\n" +
                           (mouse.state == State.Looser ? $"Mouse caught at: {cat.location}\n" : "Mouse evaded Cat\n");

            File.AppendAllText(OutFile, footer);
        }

        private void DoPrintCommand()
        {
            string catStr = (cat.state == State.NotInGame) ? "??" : cat.location.ToString();
            string mouseStr = (mouse.state == State.NotInGame) ? "??" : mouse.location.ToString();

            int dist = GetDistance();
            string distStr = (dist == -1) ? "??" : dist.ToString();

            File.AppendAllText(OutFile, $" {catStr,-5} {mouseStr,-5} {distStr}\n");
        }

        private int GetDistance()
        {
            if (cat.state == State.NotInGame || mouse.state == State.NotInGame)
                return -1;

            return Math.Abs(cat.location - mouse.location);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
