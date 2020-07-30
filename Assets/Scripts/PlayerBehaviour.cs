using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour
{
    Rigidbody rb;
    public float speed;
    private Text scoreText;

    private int score = 0;

    void Start()
    {
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        rb = GetComponent<Rigidbody>();
    }

    public void AddScore(int points)
    {
        score = score + points;
        scoreText.text = score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        rb.AddForce( (((transform.forward * inputY) + (transform.right * inputX)) ) * speed * Time.deltaTime);


    }
}
