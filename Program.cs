namespace Allonsy.Console;

using System;
using System.Media;
using System.Threading;

class Program
{
    static string[,] map2D;
    static int playerX = 1;
    static int playerY = 1;
    static Random rand = new Random();
    static Dictionary<string, string[,]> tiles;
    static string prevTile = "grass"; // To track the previous tile type

    const int viewWidth = 5;
    const int viewHeight = 5;
    const int tileSize = 5;

    // Ensure the System.Windows.Extensions NuGet package is installed in your project.
    // You can install it using the NuGet Package Manager or the following command:
    // dotnet add package System.Windows.Extensions
    public static void PlayMusic(string filepath)
    {
        SoundPlayer musicPlayer = new SoundPlayer
        {
            SoundLocation = filepath
        };
        musicPlayer.Load(); // Ensure the file is loaded before playing
        musicPlayer.Play();
    }

    static void InitializeTiles()
    {
        tiles = new Dictionary<string, string[,]>();

        string[,] grass = new string[tileSize, tileSize];
        for (int i = 0; i < tileSize; i++)
        {
            for (int j = 0; j < tileSize; j++)
            {
                bool isGrass = rand.Next(0, 2) == 0;

                if (isGrass)
                {
                    grass[i, j] = ".";
                }
                else
                {
                    grass[i, j] = ",";
                }
            }
        }
        ;
        tiles["grass"] = grass;

        string[,] water = new string[tileSize, tileSize];
        for (int i = 0; i < tileSize; i++)
        {
            for (int j = 0; j < tileSize; j++)
            {
                water[i, j] = "~";
            }
        }
        tiles["water"] = water;

        string[,] wallV = new string[tileSize, tileSize];
        for (int i = 0; i < tileSize; i++)
        {
            for (int j = 0; j < tileSize; j++)
            {
                wallV[i, j] = "▓";
            }
        }
        tiles["wallV"] = wallV;

        string[,] wallH = new string[tileSize, tileSize];
        for (int i = 0; i < tileSize; i++)
        {
            for (int j = 0; j < tileSize; j++)
            {
                wallH[i, j] = (i >= 3) ? "▒" : "▓";
            }
        }
        tiles["wallH"] = wallH;

        string[,] player = new string[tileSize, tileSize];
        for (int i = 0; i < tileSize; i++)
        {
            for (int j = 0; j < tileSize; j++)
            {
                player[i, j] = grass[i, j];
            }
        }
        player[2, 2] = "O";
        player[3, 1] = "/";
        player[3, 2] = "T";
        player[3, 3] = "\\";
        player[4, 2] = "A";
        tiles["player"] = player;
    }

    static void LogicMap()
    {
        int width = 10;
        int height = 8;
        map2D = new string[height, width];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (y == 0 && x != 0 && x != width - 1 || y == height - 1)
                {
                    map2D[y, x] = "wallH";
                }
                else if (x == 0 || x == width - 1)
                {
                    map2D[y, x] = "wallV";
                }
                else
                {
                    map2D[y, x] = "grass";
                }
            }
        }
    }

    static void detectCollision(int addX, int addY)
    {
        if (playerX + addX > 0 && playerX + addX < map2D.GetLength(1) &&
            playerY + addY > 0 && playerY + addY < map2D.GetLength(0) &&
            map2D[playerY + addY, playerX + addX] == "grass")
        {
            map2D[playerY, playerX] = prevTile; // Reset previous tile
            playerX += addX;
            playerY += addY;
            prevTile = map2D[playerY, playerX]; // Store current tile type
            map2D[playerY, playerX] = "player"; // Set player position
        }
        else
        {
            return; // Prevent moving into walls or out of bounds
        }

    }

    static void DrawMap()
    {
        System.Console.SetCursorPosition(0, 0);
        for (int y = 0; y < map2D.GetLength(0); y++)
        {
            for (int row = 0; row < tileSize; row++)
            {
                for (int x = 0; x < map2D.GetLength(1); x++)
                {
                    string tileId = map2D[y, x];
                    string[,] tile = tiles[tileId];

                    for (int col = 0; col < tileSize; col++)
                    {
                        switch (tile[row, col])
                        {
                            case "▓":
                            case "▒":
                                System.Console.ForegroundColor = ConsoleColor.Gray;
                                break;

                            case "~":
                                System.Console.ForegroundColor = ConsoleColor.Blue;
                                break;

                            case ".":
                                System.Console.ForegroundColor = ConsoleColor.DarkGreen;
                                break;
                            case ",":
                                System.Console.ForegroundColor = ConsoleColor.Green;
                                break;

                            default:
                                System.Console.ForegroundColor = ConsoleColor.White;
                                break;
                        }
                        System.Console.Write(tile[row, col]);
                    }
                }
                System.Console.WriteLine();
            }
        }
        map2D[playerY, playerX] = "player"; // Set player position
    }

    static void Main(string[] args)
    {
        PlayMusic("bgm.wav");
        Console.CursorVisible = false;
        System.Console.Title = "Wisinshu";
        System.Console.SetWindowSize(55, 42);
        System.Console.ForegroundColor = ConsoleColor.White;
        InitializeTiles();
        LogicMap();
        DrawMap();
        while (true)
        {
            DrawMap();
            var key = System.Console.ReadKey().Key;
            switch (key)
            {
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    detectCollision(0, -1); // Move up
                    break;
                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    detectCollision(0, 1); // Move down
                    break;
                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    detectCollision(-1, 0); // Move left
                    break;
                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    detectCollision(1, 0); // Move right
                    break;
            }
            System.Threading.Thread.Sleep(50); // Add a small delay to control the speed of movement
        }
    }
}
