namespace MultiplayerSnake;

public class Game
{
    private static Snake? p1;
    private static Snake? p2;
    public static HashSet<Tuple<int, int>> p1Body = new HashSet<Tuple<int, int>>();
    public static HashSet<Tuple<int, int>> p2Body = new HashSet<Tuple<int, int>>();
    private static int FrameDelay = 45;
    public static void Begin()
    {
        int difficulty = Start.SelectDifficulty();
        if (difficulty == 4)
        {
            FrameDelay = 5;
        }
        else if (difficulty == 3)
        {
            FrameDelay = 20;
        }
        else if (difficulty == 2)
        {
            FrameDelay = 30;
        }
        else if (difficulty == 1)
        {
            FrameDelay = 50;
        }
        Console.Clear();
        Console.CursorVisible = false;
        p1 = new Snake("1");
        p2 = new Snake("2");

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Be the last snake standing. Consume the food spread around the map to grow your snake!");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("");
        Console.WriteLine("Player 1: Use WASD to move your snake.\nPlayer 2: Use the arrow keys to move your snake.");
        Console.WriteLine("");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Don't touch the border or another snake!\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
        Console.ForegroundColor = ConsoleColor.Gray;
        Thread.Sleep(5000);
        Console.Write("Game will begin in 3");
        Thread.Sleep(1000);
        Console.Write("\rGame will begin in 2");
        Thread.Sleep(1000);
        Console.Write("\rGame will begin in 1");
        Thread.Sleep(1000);
        Renderer.Init(); // Init renderer
        for (int i = 0; i < 30; i ++)
        {
            Food.Generate();
        }
        //Main game loop
        while (true)
        {
            //Gather input + clear current render
            Task.Factory.StartNew(() => GetInput());
            Renderer.Clear();
            //Food logic
            int index = 0;
            foreach (var place in Food.locations)
            {
                //Render food in the list
                Renderer.Point(place[0], place[1], ConsoleColor.DarkRed);
                //P1 Collision logic
                if (p1.x == place[0] && p1.y == place[1])
                {
                    if (OperatingSystem.IsWindows())
                    {
                        Console.Beep(750, 10);
                    }
                    p1.grow = index;
                }
                //P2 Collision logic
                if (p2.x == place[0] && p2.y == place[1])
                {
                    if (OperatingSystem.IsWindows())
                    {
                        Console.Beep(750, 10);
                    }
                    p2.grow = index;
                }
                index++;
            }
            //P1 Growing/removing tail logic
            if (p1.grow == -1)
            {
                p1Body.Remove(Tuple.Create(p1.body[p1.body.Count - 1][0], p1.body[p1.body.Count - 1][1]));
                p1.body.RemoveAt(p1.body.Count - 1);
            }
            else
            {
                Food.Generate();
                Food.locations.RemoveAt(p1.grow);
                p1.grow = -1;
            }
            //P2 Growing/removing tail logic
            if (p2.grow == -1)
            {
                p2Body.Remove(Tuple.Create(p2.body[p2.body.Count - 1][0], p2.body[p2.body.Count - 1][1]));
                p2.body.RemoveAt(p2.body.Count - 1);
            }
            else
            {
                Food.Generate();
                Food.locations.RemoveAt(p2.grow);
                p2.grow = -1;
            }
            //Reset acceptinginput variable + handle snake head movement
            //P1
            p1.acceptinginput = true;
            if (p1.direction == "up")
            {
                p1.y++;
            }
            if (p1.direction == "left")
            {
                p1.x--;
            }
            if (p1.direction == "down")
            {
                p1.y--;
            }
            if (p1.direction == "right")
            {
                p1.x++;
            }
            //P2
            p2.acceptinginput = true;
            if (p2.direction == "up")
            {
                p2.y++;
            }
            if (p2.direction == "left")
            {
                p2.x--;
            }
            if (p2.direction == "down")
            {
                p2.y--;
            }
            if (p2.direction == "right")
            {
                p2.x++;
            }
            //P1 Body collision check
            if (p1Body.Contains(Tuple.Create(p1.x, p1.y)) || p2Body.Contains(Tuple.Create(p1.x, p1.y))) {
                // p1's head collided with p1 or p2's body, game over
                Winner(p2);
                return;
            }
            //P2 Body collision check
            if (p1Body.Contains(Tuple.Create(p2.x, p2.y)) || p2Body.Contains(Tuple.Create(p2.x, p2.y))) {
                // p1's head collided with p1 or p2's body, game over
                Winner(p1);
                return;
            }
            //P1 Add new body after movement and collision check was made
            p1.body.Insert(0, new[]{p1.x, p1.y});
            p1Body.Add(Tuple.Create(p1.x, p1.y));
            //P1 Check new head location is within the screen + renders whole body
            if (p1.x > 0 && p1.x <= Console.WindowWidth && p1.y > 0 && p1.y <= Console.WindowHeight)
            {
                foreach (var segment in p1.body)
                {
                    Renderer.Inject(Utilities.Location(segment[0], segment[1]), p1.Color);
                }
            }
            else //Set P2 as winner
            {
                Winner(p2);
                break;
            }
            //P2 Add new body after movement and collision check was made
            p2.body.Insert(0, new[]{p2.x, p2.y});
            p2Body.Add(Tuple.Create(p2.x, p2.y));
            if (p2.x > 0 && p2.x <= Console.WindowWidth && p2.y > 0 && p2.y <= Console.WindowHeight)
            {
                foreach (var segment in p2.body)
                {
                    Renderer.Inject(Utilities.Location(segment[0], segment[1]), p2.Color);
                }
            }
            else //Set P1 as winner
            {
                Winner(p1);
                break;
            }
            //Tell renderer we're done adding things
            Renderer.Process();
            Thread.Sleep(FrameDelay);
        }
    }
    
    private static void GetInput()
    {
        if (p1 != null && p2 != null && p1.direction != null && p2.direction != null)
        {
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.W:
                    if (p1.direction != "down" && p1.acceptinginput)
                    {
                        p1.direction = "up";
                        p1.acceptinginput = false;
                    }
                    break;
                case ConsoleKey.A:
                    if (p1.direction != "right" && p1.acceptinginput)
                    {
                        p1.direction = "left";
                        p1.acceptinginput = false;
                    }
                    break;
                case ConsoleKey.S:
                    if (p1.direction != "up" && p1.acceptinginput)
                    {
                        p1.direction = "down";
                        p1.acceptinginput = false;
                    }
                    break;
                case ConsoleKey.D:
                    if (p1.direction != "left" && p1.acceptinginput)
                    {
                        p1.direction = "right";
                        p1.acceptinginput = false;
                    }
                    break;
                case ConsoleKey.UpArrow:
                    if (p2.direction != "down" && p2.acceptinginput)
                    {
                        p2.direction = "up";
                        p2.acceptinginput = false;
                    }
                    break;
                case ConsoleKey.LeftArrow:
                    if (p2.direction != "right" && p2.acceptinginput)
                    {
                        p2.direction = "left";
                        p2.acceptinginput = false;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (p2.direction != "up" && p2.acceptinginput)
                    {
                        p2.direction = "down";
                        p2.acceptinginput = false;
                    }
                    break;
                case ConsoleKey.RightArrow:
                    if (p2.direction != "left" && p2.acceptinginput)
                    {
                        p2.direction = "right";
                        p2.acceptinginput = false;
                    }
                    break;
            }
        }
    }

    static void Winner(Snake player)
    {
        Console.BackgroundColor = ConsoleColor.Black;
        if (OperatingSystem.IsWindows())
        {
            Console.Beep(325, 240);
            Thread.Sleep(65);
            Console.Beep(325, 90);
            Thread.Sleep(10);
            Console.Beep(450, 190);
            Thread.Sleep(20);
            Console.Beep(450, 175);
            Thread.Sleep(20);
            Console.Beep(600, 1000);
        }
        Thread.Sleep(500);
        for (int i = 0; i < Console.WindowHeight; i++)
        {
            Renderer.DrawLine(1, Console.WindowHeight - i, Console.WindowWidth, Console.WindowHeight - i, ConsoleColor.Black);
            Renderer.Process();
            Thread.Sleep(25);
        }
        Renderer.Clear();
        Renderer.Process();
        Console.SetCursorPosition(Console.WindowWidth / 2 - 6, Console.WindowHeight / 2);
        Console.ForegroundColor = player.Color;
        Console.Write($"Player {player.num} wins!");
        Thread.Sleep(5000);
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.SetCursorPosition(0, Console.WindowHeight - 1);
        Console.Clear();
        Console.WriteLine("------------------------------------------------------------");
        Console.WriteLine($"Selected difficulty >> {FrameDelay} (expressed in frametime)");
        Console.WriteLine($"");
        Console.WriteLine($"");
        Console.WriteLine($"");
        Console.WriteLine("------------------------------------------------------------");
        Renderer.SendStats();
        Console.WriteLine("------------------------------------------------------------");
        Console.Read();
    }
}

