using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    public GameObject checkPoint;
    private GameManager game;
    
    void Awake()
    {
        game = GameManager.Instance;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CheckPoint")
        {
            //oh no i checkpointed
            checkPoint = other.gameObject;
        }

        if (other.tag=="Death")
        {
            //oh no i dieded
            game.Player.transform.parent = null;
            this.transform.position = checkPoint.transform.position;
        }
    }

    public void CheckPoint()
    {
        game.Player.transform.parent = null;
        this.transform.position = checkPoint.transform.position;
    }
}
