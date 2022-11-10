using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusFast : BonusBase
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
            rb.velocity *= 1.25f;
        }
    }
}
