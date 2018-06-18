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

    private int _position;
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

    private void Update()
    {
        if (RickMoving || RoundEnded)
        {
            return;
        }

        var cursor = 0;
        var passwordCursor = 0;
        Run(ref cursor, ref passwordCursor);

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
        _buff[_position] += 1;
        UpdateBuffer();
        RickMoving = false;
    }

    private void MinusOne()
    {
        RickMoving = true;
        _buff[_position] -= 1;
        UpdateBuffer();
        RickMoving = false;
    }

    private void MoveRight()
    {
        if (_position == TapeLength - 1)
        {
            return;
        }

        _rickAnimator.SetTrigger("ToRight");
        var end = _rick.transform.position + new Vector3(OneStepLength, 0, 0);
        var move = SmoothMovement(end);
        StartCoroutine(move);
        _position++;
        UpdateBuffer();
    }

    private void MoveLeft()
    {
        if (_position == 0)
        {
            return;
        }

        _rickAnimator.SetTrigger("ToLeft");
        var end = _rick.transform.position + new Vector3(-OneStepLength, 0, 0);
        var move = SmoothMovement(end);
        StartCoroutine(move);
        _position--;
        UpdateBuffer();
    }

    private int GetData()
    {
        return _buff[_position];
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
        _book = Check(_code);
        RickMoving = false;
        RoundEnded = true;
        _buffer.text = "";
        for (var i = 0; i < TapeLength; i++)
        {
            _buff[i] = 0;
            _buffer.text += "0 ";
        }

        _position = 0;
        _rick.transform.position = _rickInitPosition;
    }

    private static List<short> Check(IReadOnlyList<char> code)
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

    public void SetData(List<char> code, List<GameObject> password)
    {
        _code = code;
        _password = password;
        RoundEnded = false;
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
                }

                break;
            case ']':
                if (GetData() != 0)
                {
                    for (i = cursor - 1; i >= 0 && _book.ElementAt(i) != _book.ElementAt(cursor); i--)
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

        if (cursor == _code.Count - 1)
        {
            RoundEnded = true;
        }
    }
}