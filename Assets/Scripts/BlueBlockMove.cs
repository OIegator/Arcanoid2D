using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBlockMove : MonoBehaviour
{
    public float speedV;
    public float point;
    // Start is called before the first frame update
    void Start()
    {
        speedV = Random.Range(1, 6);

        System.Random rand = new System.Random();
        int[] a = new int[2] { -1, 1 };
        speedV = speedV * a[rand.Next(0, a.Length)];
    }

    // Update is called once per frame
    void Update()
    {

        transform.Translate(speedV * Time.deltaTime, 0, 0);
        if (transform.position.x > 5.5f || transform.position.x < -5.5f)
        {
            speedV = -speedV;

        }


    }
}