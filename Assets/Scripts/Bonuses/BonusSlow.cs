using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusSlow : BonusBase
{
    PlayerScript playerObj;
    GameObject[] balls;
    Rigidbody2D rb;

    public override void BonusActivate()
    {
        balls = GameObject.FindGameObjectsWithTag("Ball");
        foreach (GameObject ball in balls)
        {
            rb = ball.GetComponent<Rigidbody2D>();
            rb.velocity *= 0.85f;
        }
    }
}
