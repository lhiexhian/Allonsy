namespace Allonsy.Console;

using System;

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
                } else
                {
                    grass[i, j] = ",";
                }
            }
        };
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
                //if (i == 4)
                //{
                //    tiles["wallH"][i, j] = "▒";
                //} else
                //{
                //    tiles["wallH"][i, j] = "▓";
                //}
                wallH[i, j] = (i >= 3) ? "▒" : "▓";
            }
        }
        tiles["wallH"] = wallH;

        string[,] player = new string[tileSize, tileSize];
        for (int i = 0; i < tileSize; i++)
        {
            for (int j = 0; j < tileSize; j++)
            {
                player[i, j] = grass[i,j];
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
        } else
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





//namespace Allonsy.Console
//{
//    //class Player
//    //{
//    //    private string _uName;

//    //    public string Name
//    //    {
//    //        get { return _uName; }
//    //        set { _uName = value; }
//    //    }

//    //    public void GetName(string text)
//    //    {
//    //        System.Console.WriteLine(text + _uName);
//    //    }
//    //    public string GetName()
//    //    {
//    //        return _uName;
//    //    }

//    //    public void SetName(string Name)
//    //    {
//    //        _uName = Name;
//    //    }
//    //}



//    class Program
//    {
//        static char[,] map2D;
//        static Dictionary<char, string[,]> tileItem;
//        static int tileWidth = 5;
//        static int tileHeight = 5;
//        static int playerX = 1;
//        static int playerY = 1;
//        static Random rand = new Random();

//        static void TileSet()
//        {
//            tileItem = new Dictionary<char, string[,]>();

//            string[,] grass = new string[5, 5];
//            for (int i = 0; i < 5; i++)
//            {
//                for (int j = 0; j < 5; j++)
//                {
//                    bool isGrass = rand.Next(0, 2) == 0;
//                    if (isGrass)
//                    {
//                        grass[i, j] = ".";
//                    }
//                    else
//                    {
//                        grass[i, j] = ",";
//                    }
//                }
//            }
//            tileItem['.'] = grass;

//            string[,] water = new string[5, 5];
//            for (int i = 0; i < 5; i++)
//            {
//                for (int j = 0; j < 5; j++)
//                {
//                    water[i, j] = "~";
//                }
//            }
//            water[0, 0] = " ";
//            tileItem['~'] = water;

//            string[,] wall = new string[5, 5];
//            for (int i = 0; i < 5; i++)
//            {
//                for (int j = 0; j < 5; j++)
//                {
//                    if (i == 4)
//                    {
//                        wall[i, j] = "▒";
//                    }
//                    else if (j == 0 || j == 4 || i == 0 || i == 3)
//                    {
//                        wall[i, j] = "▓";
//                    }
//                    else
//                    {
//                        wall[i, j] = " ";
//                    }
//                }
//            }
//            tileItem['w'] = wall;

//            string[,] wall2 = new string[5, 5];
//            for (int i = 0; i < 5; i++)
//            {
//                for (int j = 0; j < 5; j++)
//                {
//                    wall2[i, j] = (j == 0 || j == 4 || i == 0 || i == 4) ? "▓" : " ";
//                }
//            }
//            tileItem['W'] = wall2;

//            string[,] player = new string[5, 5];
//            for (int i = 0; i < 5; i++)
//            {
//                for (int j = 0; j < 5; j++)
//                {
//                    player[i, j] = grass[i, j];
//                }
//            }
//            player[2, 2] = "O";
//            player[3, 1] = "/";
//            player[3, 2] = "T";
//            player[3, 3] = "\\";
//            player[4, 2] = "A";
//            tileItem['p'] = player;
//        }


    //    static void detectCollision(int addX, int addY)
    //{
    //    playerX += addX;
    //    playerY += addY;


    //    if (map2D[playerY, playerX] == '.')
    //    {
    //        map2D[playerY, playerX] = 'p';
    //    }
    //}

//        static void LogicMap()
//        {
//            int width = 10;
//            int height = 8;
//            map2D = new char[height, width];

//            int x, y;
//            x = 0;
//            for (y = 0; y < height; y++)
//            {
//                for (x = 0; x < width; x++)
//                {
//                    if (y == 0 && x != 0 && x != width-1 || y == height - 1 )
//                    {
//                        map2D[y, x] = 'w';
//                    }
//                    else if (x == 0 || x == width - 1)
//                    {
//                        map2D[y, x] = 'W';
//                    }
//                    else
//                    {
//                        map2D[y, x] = '.';
//                    }
//                }
//            }
//        }

//        static void DrawMap()
//        {
//            //System.Console.Clear();
//            System.Console.SetCursorPosition(0, 0);
//            for (int y = 0; y < map2D.GetLength(0); y++)
//            {
//                for (int row = 0; row < tileHeight; row++)
//                {
//                    for (int x = 0; x < map2D.GetLength(1); x++)
//                    {
//                        char tileId = map2D[y, x];
//                        string[,] tile = tileItem[tileId];

//                        for (int col = 0; col < tileWidth; col++)
//                        {
//                            switch (tile[row, col])
//                            {
//                                case "▓":
//                                case "▒":
//                                    System.Console.ForegroundColor = ConsoleColor.DarkGray;
//                                    break;

//                                case "~":
//                                    System.Console.ForegroundColor = ConsoleColor.Blue;
//                                    break;

//                                case ".":
//                                    System.Console.ForegroundColor = ConsoleColor.DarkGreen;
//                                    break;
//                                case ",":
//                                    System.Console.ForegroundColor = ConsoleColor.Green;
//                                    break;

//                                default:
//                                    System.Console.ForegroundColor = ConsoleColor.White;
//                                    break;
//                            }
//                            System.Console.Write(tile[row, col]);
//                        }
//                    }
//                    System.Console.WriteLine();
//                }
//            }
//            map2D[playerY, playerX] = 'p'; // Set player position
//        }

//        static void Main(string[] args)
//        {
//            System.Console.Title = "Wisinshu";
//            System.Console.SetWindowSize(55, 42);
//            System.Console.ForegroundColor = ConsoleColor.White;

//            TileSet();
//            LogicMap();
//    DrawMap();
//            while (true)
//            {
//                DrawMap();
//var key = System.Console.ReadKey().Key;
//                switch (key)
//                {
//                    case ConsoleKey.W:
//                    case ConsoleKey.UpArrow:
//                        detectCollision(0, -1); // Move up
//                        break;
//                    case ConsoleKey.S:
//                    case ConsoleKey.DownArrow:
//                        detectCollision(0, 1); // Move down
//                        break;
//                    case ConsoleKey.A:
//                    case ConsoleKey.LeftArrow:
//                        detectCollision(-1, 0); // Move left
//                        break;
//                    case ConsoleKey.D:
//                    case ConsoleKey.RightArrow:
//                        detectCollision(1, 0); // Move right
//                        break;
//                }
//            }
//            Thread.Sleep(50);
//            //var player = new Player();
//            //player.SetName(System.Console.ReadLine());
//            //player.GetName("Player Name: ");
//        }
//    }
//}
