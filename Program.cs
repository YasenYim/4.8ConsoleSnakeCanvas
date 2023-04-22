using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _4_8贪吃蛇_Canvas
{
    enum Dir
    {
        None,
        Up,
        Down,
        Left,
        Right,
    }

    struct Pos
    {
        public int x;
        public int y;

        public Pos(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    class Snake
    {
        public List<Pos> bodies;

        public Dir dir = Dir.None;

        public void ChangeDir(Dir input)
        {
            if (input == Dir.None) return;
            if (input == dir) return;
            if (dir == Dir.Right && input == Dir.Left) return;
            if (dir == Dir.Left && input == Dir.Right) return;
            if (dir == Dir.Up && input == Dir.Down) return;
            if (dir == Dir.Down && input == Dir.Up) return;

            dir = input;
        }

        public Snake()
        {
            bodies = new List<Pos>();

            for (int i = 0; i < 10; i++)
            {
                bodies.Add(new Pos(1, 1));
            }
        }


        public void Move()
        {
            Pos head = bodies[0];
            switch (dir)
            {
                case Dir.Up:
                    {
                        head.y--;
                    }
                    break;
                case Dir.Down:
                    {
                        head.y++;
                    }
                    break;
                case Dir.Left:
                    {
                        head.x--;
                    }
                    break;
                case Dir.Right:
                    {
                        head.x++;
                    }
                    break;
                default:
                    {
                    }
                    break;
            }

            // 蛇的身体，一节一节跟上前一节的身体
            for (int i = bodies.Count - 1; i > 0; i--)
            {
                bodies[i] = bodies[i - 1];
            }

            bodies[0] = head;
        }

        public void OnEatApple()
        { bodies.Add(bodies[bodies.Count - 1]); }


    }

    class Program
    {
        static Random random;

        static Snake snake;

        static string[,] buffer;     // 屏幕缓冲区

        static ConsoleColor[,] colorBuffer;  // 屏幕颜色缓冲区

        static int width = 20;     // 游戏世界宽度

        static int height = 20;    // 游戏世界高度

        static int ox = 6;         // 边框偏差量

        static int oy = 5;         // 边框偏差量

        static Pos apple;          // 苹果的位置（只有一个）

        static int score = 0;

        static ConsoleCanvas canvas;

        static void RandApple()
        {


            apple.x = random.Next(0, width);

            apple.y = random.Next(0, height);
        }

        static void Main(string[] args)
        {
            // 1、初始化
            random = new Random();

            canvas = new ConsoleCanvas(width + ox + 1, height + oy + 1);  // 指定宽度高度

            snake = new Snake();

            buffer = canvas.GetBuffer();

            colorBuffer = canvas.GetColorBuffer();

            Dir inputDir = Dir.None;

            RandApple();

            Refresh();

            while (true)
            {
                // 2、输入处理
                // ReadKey会造成程序卡住（阻塞）

                // 输入队列：[111111111],使用while每一帧尽可能清空队列
                ConsoleKeyInfo keyInfo = new ConsoleKeyInfo();

                while (Console.KeyAvailable)
                {
                    keyInfo = Console.ReadKey(true);

                }

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        {
                            inputDir = Dir.Up;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        {
                            inputDir = Dir.Down;
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        {
                            inputDir = Dir.Left;
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        {
                            inputDir = Dir.Right;
                        }
                        break;
                    default:
                        {
                            inputDir = Dir.None;
                        }
                        break;
                }

                // 3、蛇逻辑更新

                // 蛇调整方向
                snake.ChangeDir(inputDir);       // ???

                Pos head = snake.bodies[0];

                // 蛇前进
                snake.Move();

                // 如果蛇撞到自己的身体，游戏结束

                // 如果蛇撞墙，游戏结束
                if (head.y < 0 || head.y >= height || head.x < 0 || head.x >= width)
                {
                    break;
                }



                // 吃苹果的判断
                //if (snake.bodies[0].x == apple.x && snake.bodies[0].y == apple.y)
                if (snake.bodies[0].Equals(apple))
                {
                    snake.OnEatApple();
                    score++;
                    RandApple();   // 苹果换个位置
                }
                // 4、渲染
                Refresh();

                // 控制刷新的频率
                Thread.Sleep(100);
            }
            Console.WriteLine("游戏结束！");

            Console.ReadKey(true);
        }


        static void Refresh()
        {
            // 清空buffer(画空）
            canvas.ClearBuffer();

            // 画边界
            for (int i = oy - 1; i < height + oy + 1; i++)
            {
                for (int j = ox - 1; j < width + ox + 1; j++)
                {
                    if (i == oy - 1 && j != width + ox)
                    {
                        buffer[i, j] = "##";
                    }
                    else if (i == height + oy && j != width + ox)
                    {
                        buffer[i, j] = "##";
                    }
                    else if (j == ox - 1)
                    {
                        buffer[i, j] = "#";
                    }
                    else if (j == width + ox)
                    {
                        buffer[i, j] = "#";
                    }
                }
            }

            // 画蛇
            for (int i = 0; i < snake.bodies.Count; i++)
            {
                int tx = snake.bodies[i].x + ox;
                int ty = snake.bodies[i].y + oy;
                buffer[ty, tx] = "o";
                colorBuffer[ty, tx] = ConsoleColor.Green;
            }

            // 画苹果
            buffer[apple.y + oy, apple.x + ox] = "果";
            colorBuffer[apple.y + oy, apple.x + ox] = ConsoleColor.Red;

            //// 写得分,在buffer中写不确定长度的字符串，不方便
            //buffer[oy - 2, ox - 1] = '得';
            //buffer[oy - 2, ox    ] = '分';
            //buffer[oy - 2, ox + 1] = '：';
            //buffer[oy - 2, ox + 1] = score.ToString()[0];

            buffer[oy - 2, ox - 1] = $"得分：{score}";

            // Console打印buffer
            canvas.Refresh();

            //Console.SetCursorPosition((ox - 1) * 2, oy - 2);
            //Console.Write($"得分：{score}");
        }
    }
}
