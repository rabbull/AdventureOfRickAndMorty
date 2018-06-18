using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int _position;
    private List<char> _code;
    private List<GameObject> _password;
    private GameObject _cursor;
    private static GameManager _instance;
    private const float CharactorWidth = 0.4785f;


    private GameObject _tape;
    
    private GameObject _codeText;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("GameManager").GetComponent<GameManager>();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        _code = new List<char>();
        _position = 0;

        _tape = GameObject.Find("Tape");
        
        _password = new List<GameObject>();
        _password.Clear();
        _password.Add(GameObject.Find("Password"));
        _password.Add(GameObject.Find("Password1"));
        _password.Add(GameObject.Find("Password2"));
        _password.Add(GameObject.Find("Password3"));
        
        _codeText = GameObject.Find("Code");
        
        _cursor = GameObject.Find("Cursor");
    }

    public void AddNewInstruction (char c)
    {
        _code.Insert (_position, c);
        _codeText.GetComponent<Text>().text = new string(_code.ToArray());
        MoveCursor(false);
    }

    public void MoveCursor(bool toLeft)
    {
        var cursorMoves = true;
        var towards = new Vector3(0, 0, 0);
        if (toLeft)
        {
            if (_position != 0)
            {
                _position -= 1;
                if (_cursor.transform.position.x > -4.4f || _position < 1)
                {
                    towards -= new Vector3(CharactorWidth, 0, 0);
                }
                else
                {
                    cursorMoves = false;
                    towards += new Vector3(CharactorWidth, 0, 0);
                }
            }
        }
        else
        {
            if (_position != _code.Count + 1)
            {
                _position += 1;
                if (_cursor.transform.position.x <= 4.3f)
                {
                    towards += new Vector3(CharactorWidth, 0, 0);
                }
                else
                {
                    cursorMoves = false;
                    towards -= new Vector3(CharactorWidth, 0, 0);
                }
            }
        }

        if (cursorMoves)
        {
            _cursor.transform.position += towards;
        }
        else
        {
            _codeText.transform.position += towards;
        }
    }

    public void DeleteHere()
    {
        if (_position != 0)
        {
            _code.RemoveAt (_position - 1);
            MoveCursor(true);
        }
        _codeText.GetComponent<Text>().text = new string(_code.ToArray());
    }

    public void Run()
    {
        HideCursor();
        _tape.GetComponent<Tape>().SetData(_code, _password);
    }

    public void Restart()
    {
        _tape.GetComponent<Tape>().Restart();
    }

    private void HideCursor()
    {
        _cursor.GetComponent<SpriteRenderer>().sortingOrder = -1;
    }

    public void ShowCursor()
    {
        _cursor.GetComponent<SpriteRenderer>().sortingOrder = 2;
    }
}
