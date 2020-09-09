using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public PlayerController Player;
    public TimeTravelMechanic TimeTravelMechanic;
    public ThirdPersonCamera Camera;

    private bool _isMovementEnabled;
    public bool IsMovementEnabled
    {
        set
        {
            _isMovementEnabled = value;
            Camera.SetActive(value);
            Player.SetActive(value);
        }
        get { return _isMovementEnabled; }
    }

    private int _gameScore;
    public int GameScore
    {
        get { return _gameScore; }
        set { _gameScore = value; }
    }
}