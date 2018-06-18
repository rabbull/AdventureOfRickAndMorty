using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Tape : MonoBehaviour
{
    private const int PerformNothing = 0;
    private const int PerformLeft = 1;
    private const int PerformRight = 2;
    private const int PerformAdd = 3;
    private const int PerformMinus = 4;

    private List<char> _code;
    private List<short> _book;
    private List<GameObject> _password;

    private int _perform;
    public bool RickMoving;
    public bool RoundEnded;

    private Text _buffer;
    private GameObject _rick;
    private Animator _rickAnimator;
    private Rigidbody2D _rickRigidbody2D;
    private Vector3 _rickInitPosition;

    public float MoveTime = .00001f;
    public float OneStepLength = 1.37f;

    private float _inverseMoveTime;

    private int _curr;
    private const int TapeLength = 7;
    private readonly int[] _buff = new int[TapeLength];

    private void Awake()
    {
        _buffer = GameObject.Find("Buff").GetComponent<Text>();
        _rick = GameObject.Find("Rick");
        _rickInitPosition = _rick.transform.position;
        _rickAnimator = _rick.GetComponent<Animator>();
        _rickRigidbody2D = _rick.GetComponent<Rigidbody2D>();
        _inverseMoveTime = 1 / MoveTime;
        Restart();
    }

    private int _cursor;
    private int _passwordCursor;

    private void Update()
    {
        if (RickMoving || RoundEnded)
        {
            return;
        }

        Run(ref _cursor, ref _passwordCursor);

        switch (_perform)
        {
            case PerformNothing:
                break;
            case PerformLeft:
                MoveLeft();
                break;
            case PerformRight:
                MoveRight();
                break;
            case PerformAdd:
                AddOne();
                break;
            case PerformMinus:
                MinusOne();
                break;
            default:
                Debug.Log("error");
                break;
        }
    }

    private void UpdateBuffer()
    {
        _buffer.text = "";
        for (var i = 0; i < TapeLength; i++)
        {
            _buffer.text += (_buff[i] % 10).ToString();
            _buffer.text += " ";
        }
    }

    private void AddOne()
    {
        RickMoving = true;
        _buff[_curr] += 1;
        UpdateBuffer();
        RickMoving = false;
    }

    private void MinusOne()
    {
        RickMoving = true;
        _buff[_curr] -= 1;
        UpdateBuffer();
        RickMoving = false;
    }

    private void MoveRight()
    {
        if (_curr == TapeLength - 1)
        {
            return;
        }

        _rickAnimator.SetTrigger("ToRight");
        var move = SmoothMovement(_rick.transform.position + new Vector3(OneStepLength, 0, 0));
        StartCoroutine(move);
        _curr++;
        UpdateBuffer();
    }

    private void MoveLeft()
    {
        if (_curr == 0)
        {
            return;
        }

        _rickAnimator.SetTrigger("ToLeft");
        var move = SmoothMovement(_rick.transform.position + new Vector3(-OneStepLength, 0, 0));
        StartCoroutine(move);
        _curr--;
        UpdateBuffer();
    }

    private int GetData()
    {
        return _buff[_curr];
    }

    private IEnumerator SmoothMovement(Vector3 end)
    {
        RickMoving = true;
        var sqrRemainingDistance = (_rick.transform.position - end).sqrMagnitude;
        while (sqrRemainingDistance > float.Epsilon)
        {
            var newPosition = Vector3.MoveTowards(_rickRigidbody2D.position, end, _inverseMoveTime * Time.deltaTime);
            _rickRigidbody2D.MovePosition(newPosition);
            sqrRemainingDistance = (_rick.transform.position - end).sqrMagnitude;
            yield return null;
        }

        RickMoving = false;
    }

    public void Restart()
    {
        _cursor = 0;
        _passwordCursor = 0;
        if (_password != null)
        {
            foreach (var passwd in _password)
            {
                passwd.GetComponent<Text>().text = "0";
            }
        }

        if (_code != null)
        {
            _book = Check(_code);
        }

        RickMoving = false;
        RoundEnded = true;
        _buffer.text = "";
        for (var i = 0; i < TapeLength; i++)
        {
            _buff[i] = 0;
            _buffer.text += "0 ";
        }

        _curr = 0;
        _rick.transform.position = _rickInitPosition;
    }

    private static List<short> Check(IReadOnlyList<char> code)
    {
        var book = new List<short>();
        return Check(code, ref book) ? book : null;
    }

    private static bool Check(IReadOnlyList<char> code, ref List<short> book)
    {
        if (book == null)
        {
            book = new List<short>();
        }
        else
        {
            book.Clear();
        }

        var stack = new Stack<short>();
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

                book.Add(stack.Pop());
            }
            else
            {
                book.Add(-1);
            }
        }

        return true;
    }

    public void SetData(List<char> code, List<GameObject> password)
    {
        _code = code;
        _password = password;
        RoundEnded = false;
        _book = Check(code);
        if (_book == null)
        {
            Debug.Log(_book);
        }
        else
        {
            foreach (var s in _book)
            {
                Debug.Log(s);
            }
        }
    }

    private void Run(ref int cursor, ref int passwordCursor)
    {
        _perform = -1;
        int i;
        switch (_code.ElementAt(cursor))
        {
            case '>':
                _perform = PerformRight;
                break;
            case '<':
                _perform = PerformLeft;
                break;
            case '+':
                _perform = PerformAdd;
                break;
            case '-':
                _perform = PerformMinus;
                break;
            case '[':
                if (GetData() == 0)
                {
                    for (i = cursor + 1; i < _code.Count && _book.ElementAt(i) != _book.ElementAt(cursor); i++)
                    {
                    }

                    cursor = i;
                    Debug.Log(i.ToString());
                }

                break;
            case ']':
                if (GetData() != 0)
                {
                    for (i = cursor - 1; i < _code.Count && _book.ElementAt(i) != _book.ElementAt(cursor); i--)
                    {
                    }

                    cursor = i;
                }

                break;
            case '.':
                _password[passwordCursor].GetComponent<Text>().text = GetData().ToString();
                passwordCursor += 1;
                break;
            default:
                Debug.Log("error!");
                break;
        }

        cursor += 1;
        if (cursor == _code.Count)
        {
            RoundEnded = true;
        }
    }
}