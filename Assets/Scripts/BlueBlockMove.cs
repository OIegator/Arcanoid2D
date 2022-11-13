using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Данный скрипт описывает поведение синих блоков
//Блоки движутся между заданными точками (от одного края экрана до противоположного)
//Блоки не взаимодействуют с другими блоками
//Блоки взаимодействуют с мячиком

public class BlueBlockMove : MonoBehaviour
{
    public float speedV;//скорость передвижения
    // Start вызыватся перед первым кадром
    void Start()
    {
        speedV = Random.Range(1, 6);//задаем случайную скорость в диапазоне от 1 до 6
        //задаем случайное направление для начала движения
        System.Random rand = new System.Random();
        int[] a = new int[2] { -1, 1 };
        speedV = speedV * a[rand.Next(0, a.Length)];
    }

    // Update вызыывается перед каждым кадром 1 раз
    void Update()
    {
        //расчитываем позицию блока в данный момент времени
        transform.Translate(speedV * Time.deltaTime, 0, 0);
        //если блок очень близок к границе игрового экрана, меняем направление на противоположное
        if (transform.position.x > 5.5f || transform.position.x < -5.5f)
        {
            speedV = -speedV;
        }
    }
}
