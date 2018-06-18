using System;
using System.Collections.Generic;

public class Interpreter
{
    public const int BufSize = 8;

    public string Run(char[] code, short[] book, int[] initBuf)
    {
        var output = "";
        int flag = 0;
        int len = code.Length;

        int[] buf = new int[BufSize];
        for (int i = 0; i < initBuf.Length; i++) buf[i] = initBuf[i];
        int p = 0;

        while (flag != len)
        {
            if (code[flag] == '+')
            {
                buf[p] += 1;
            }
            else if (code[flag] == '-')
            {
                buf[p] -= 1;
            }
            else if (code[flag] == '.')
            {
                output.Insert(output.Length, buf[p].ToString());
            }
            else if (code[flag] == '>')
            {
                p += 1;
                if (p == BufSize) throw new Exception("Memory Overflow.");
            }
            else if (code[flag] == '<')
            {
                p -= 1;
                if (p < 0) throw new Exception("Negative Pointer.");
            }
            else if (code[flag] == '[')
            {
                if (buf[p] != 0)
                {
                    var n = book[flag];
                    while (book[++flag] != n) ;
                }
            }
            else if (code[flag] == ']')
            {
                if (buf[p] != 0)
                {
                    var n = book[flag];
                    while (book[--flag] != n) ;
                }
            }

            flag++;
        }

        return output;
    }

    public string Run(char[] code, short[] book)
    {
        var output = "";
        var flag = 0;
        var len = code.Length;

        var buf = new int[BufSize];
        var p = 0;

        while (flag != len)
        {
            switch (code[flag])
            {
                case '+':
                    buf[p] += 1;
                    break;
                case '-':
                    buf[p] -= 1;
                    break;
                case '.':
                    output.Insert(output.Length, buf[p].ToString());
                    break;
                case '>':
                    p += 1;
                    if (p == BufSize) throw new Exception("Memory Overflow.");
                    break;
                case '<':
                    p -= 1;
                    if (p < 0) throw new Exception("Negative Pointer.");
                    break;

                case '[':
                    if (buf[p] != 0)
                    {
                        var n = book[flag];
                        while (book[++flag] != n) ;
                    }

                    break;

                case ']':
                    if (buf[p] != 0)
                    {
                        var n = book[flag];
                        while (book[--flag] != n) ;
                    }

                    break;

                default:
                    break;
            }

            flag++;
        }

        return output;
    }

    private bool check(char[] code, Int16[] book)
    {
        int len = code.Length;
        Int16 cnt = 0;
        Stack<Int16> s = new Stack<short>();
        for (int i = 0; i < len; i++)
            if (code[i] == '[')
            {
                s.Push(cnt);
                book[i] = cnt;
                cnt += 1;
            }
            else if (code[i] == ']')
            {
                book[i] = s.Pop();
            }

        return true;
    }
}