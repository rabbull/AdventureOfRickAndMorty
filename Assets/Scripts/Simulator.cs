using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Simulator : MonoBehaviour
{
    public static void Run (GameObject tape, List<char> code, List<GameObject> password)
    {
        var book = Check(code) as List<short>;
        if (book == null)
        {
            Debug.Log("error!");
            return;
        }
        var cursor = 0;
        var t = tape.GetComponent<Tape>();
        var passwordCursor = 0;
        for (cursor = 0; cursor < code.Count; cursor += 1)
        {
            int i;
            switch (code.ElementAt(cursor))
            {
                case '>':
                    t.Perform(Tape.PerformRight);
                    break;
                case '<':
                    t.Perform(Tape.PerformLeft);
                    break;
                case '+':
                    t.Perform(Tape.PerformAdd);
                    break;
                case '-':
                    t.Perform(Tape.PerformMinus);
                    break;
                case '[':
                    if (t.GetData() == 0)
                    {
                        for (i = cursor + 1; i < code.Count && code.ElementAt(i) != code.ElementAt(cursor); i++) {}
                        cursor = i;
                    }
                    break;
                case ']':
                    if (t.GetData() != 0)
                    {
                        for (i = cursor - 1; i >= 0 && code.ElementAt(i) != code.ElementAt(cursor); i --) {}
                        cursor = i;
                    }
                    break;
                case '.':
                    password[passwordCursor].GetComponent<Text>().text = t.GetData().ToString();
                    passwordCursor += 1;
                    break;
                default:
                    Debug.Log("error!");
                    break;
            }
        }
    }

    private static IEnumerable<short> Check(IReadOnlyList<char> code)
    {
        var book = new List<short>();
        return Check(code, ref book) ? book : null;
    }

    private static bool Check(IReadOnlyList<char> code, ref List<short> book)
    {
        var stack = new Stack<short>();
        book.Clear();
        var length = code.Count;
        short cnt = 0;

        for (var i = 0; i < length; i++)
        {
            if (code[i] == '[')
            {
                stack.Push(cnt);
                book.Add(cnt);
                cnt += 1;
            }
            else if (code[i] == ']')
            {
                if (stack.Count == 0)
                {
                    return false;
                }

                book.Add(cnt);
                stack.Pop();
            }
            book.Add(-1);
        }

        return true;
    }
}
