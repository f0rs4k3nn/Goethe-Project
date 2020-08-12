using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMisc : MonoBehaviour
{
    private int pointsCollected;
    private ThirdPersonCamera camera;
    private PlayerController player;
    private Text scoreText;
    private bool isActive = true;

    private void Start()
    {
        player = GetComponent<PlayerController>();
        camera = Camera.main.GetComponent<ThirdPersonCamera>();
        scoreText = GameObject.Find("TrashScore").GetComponent<Text>();
        scoreText.text = pointsCollected.ToString();
    }

    public void IncrementPoints(int points)
    {
        pointsCollected += points;
        scoreText.text = pointsCollected.ToString();
    }

    public void ActivateMovement(bool isActive)
    {
        this.isActive = isActive;
        camera.SetActive(isActive);
        player.SetActive(isActive);
    }

    public bool IsActive()
    {
        return isActive;
    }
}
