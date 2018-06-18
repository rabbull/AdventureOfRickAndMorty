using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int _position;
    private List<char> _code;
    private List<GameObject> _password;
    private static GameManager _instance;

    private GameObject _tape;
    
    public Text CodeText;

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
    }

    public void AddNewInstruction (char c)
    {
        _code.Insert (_position, c);
        CodeText.text = new string(_code.ToArray());
        _position += 1;
    }

    public void MoveCursor (bool toLeft)
    {
        if (toLeft)
        {
            _position -= 1;
            if (_position == -1)
            {
                _position = 0;
            }
        }
        else
        {
            _position += 1;
            if (_position == _code.Count + 1)
            {
                _position = _code.Count;
            }
        }
    }

    public void DeleteHere()
    {
        if (_position != 0)
        {
            _code.RemoveAt (_position - 1);
            _position -= 1;
        }
        CodeText.text = new string(_code.ToArray());
    }

    public void Run()
    {
        _tape.GetComponent<Tape>().SetData(_code, _password);
    }

    public void Restart()
    {
        _tape.GetComponent<Tape>().Restart();
    }
}
