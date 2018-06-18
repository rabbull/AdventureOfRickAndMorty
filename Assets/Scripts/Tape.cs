using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;

public class Tape : MonoBehaviour
{
    public const int PerformLeft = 1;
    public const int PerformRight = 1;
    public const int PerformAdd = 1;
    public const int PerformMinus = 1;


    private int perform;
    public bool RickMoving;

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
        if (RickMoving)
        {
            return;
        }

        switch (perform)
        {
            case 1:
                MoveLeft();
                break;
            case 2:
                MoveRight();
                break;
            case 3:
                AddOne();
                break;
            case 4:
                MinusOne();
                break;
            default:
                Debug.Log("error");
                break;
        }
    }

    public void Perform(int p)
    {
        perform = p;
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
        _buff[_position] += 1;
        UpdateBuffer();
    }

    private void MinusOne()
    {
        _buff[_position] -= 1;
        UpdateBuffer();
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

    public int GetData()
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
        _buffer.text = "";
        for (var i = 0; i < TapeLength; i++)
        {
            _buff[i] = 0;
            _buffer.text += "0 ";
        }

        _position = 0;
        _rick.transform.position = _rickInitPosition;
    }
}