using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ConsoleCanvas
{
    int height;
    int width;
    string[,] backBuffer;
    string[,] buffer;
    ConsoleColor[,] color_buffer;

    public int Height
    {
        get { return height; }
    }

    public int Width
    {
        get { return width; }
    }

    public ConsoleCanvas(int w, int h, char _empty = ' ')
    {
        width = w;
        height = h;
        buffer = new string[height, width];
        backBuffer = new string[height, width];
        color_buffer = new ConsoleColor[height, width];
        Console.CursorVisible = false;
        //ClearBuffer();
        for (int i = 0; i < height; ++i)
        {
            for (int j = 0; j < width; ++j)
            {
                buffer[i, j] = " ";
                color_buffer[i, j] = ConsoleColor.Gray;
            }
        }
    }

    void CopyArray2D<T>(T[,] source, T[,] dest)
    {
        for (int i = 0; i < source.GetLength(0); ++i)
        {
            for (int j = 0; j < source.GetLength(1); ++j)
            {
                dest[i, j] = source[i, j];
            }
        }
    }

    public void ClearBuffer()
    {
        CopyArray2D(buffer, backBuffer);
        for (int i = 0; i < height; ++i)
        {
            for (int j = 0; j < width; ++j)
            {
                buffer[i, j] = " ";
                color_buffer[i, j] = ConsoleColor.Gray;
            }
        }
    }

    public string[,] GetBuffer()
    {
        return buffer;
    }

    public ConsoleColor[,] GetColorBuffer()
    {
        return color_buffer;
    }

    public void Refresh()
    {
        for (int i = 0; i < height; i++)
        {
            // 这里的算法是为了去除每行最后面的空白，空白多了影响排版
            int end = 0;
            for (int j = width - 1; j >= 0; --j)
            {
                if (buffer[i, j] != " " || backBuffer[i, j] != " ")
                {
                    end = j + 1;
                    break;
                }
            }

            for (int j = 0; j < end; j++)
            {
                if (buffer[i, j] != backBuffer[i, j])
                {
                    Console.SetCursorPosition(j * 2, i);
                    ConsoleColor c = color_buffer[i, j];
                    Console.ForegroundColor = c;
                    Console.Write(buffer[i, j]);
                }
            }
        }
    }

}
