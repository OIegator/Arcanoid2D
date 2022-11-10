using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Game Data", order = 51)]
public class GameDataScript : ScriptableObject
{
    public bool resetOnStart;
    public int level = 1;
    public int balls = 6;
    public int points = 0;
    public int pointsToBall = 0;
    public bool music = true;
    public bool sound = true;
    public int soundVolume = 1;
    public float musicVolume = 0.2f;

    public void Reset()
    {
        level = 1;
        balls = 6;
        points = 0;
        pointsToBall = 0;
    }

    public void Save()
    {
        PlayerPrefs.SetInt("level", level);
        PlayerPrefs.SetInt("balls", balls);
        PlayerPrefs.SetInt("points", points);
        PlayerPrefs.SetInt("pointsToBall", pointsToBall);
        PlayerPrefs.SetInt("music", music ? 1 : 0);
        PlayerPrefs.SetInt("sound", sound ? 1 : 0);
        PlayerPrefs.SetInt("soundVolume", soundVolume);
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
    }

    public void Load()
    {
        level = PlayerPrefs.GetInt("level", 1);
        balls = PlayerPrefs.GetInt("balls", 6);
        points = PlayerPrefs.GetInt("points", 0);
        pointsToBall = PlayerPrefs.GetInt("pointsToBall", 0);
        music = PlayerPrefs.GetInt("music", 1) == 1;
        sound = PlayerPrefs.GetInt("sound", 1) == 1;
        soundVolume = PlayerPrefs.GetInt("soundVolume", 1);
        musicVolume = PlayerPrefs.GetFloat("musicVolume", 0.2f);
    }

    public int[] getProbab()
    {
        int[] probab = new int[6];
        probab[0] = 10; // Probability of +10
        probab[1] = 15; // Probability of +2
        probab[2] = 20; // Probability of Ball
        probab[3] = 10; // Probability of Fast
        probab[4] = 20; // Probability of Slow
        probab[5] = 25; // Probability of Bonus+100

        int[] prosum = new int[6];
        prosum[0] = probab[0];
        for (int i = 1; i < 6; i++)
            prosum[i] = prosum[i - 1] + probab[i];

        return prosum;
    }

}
