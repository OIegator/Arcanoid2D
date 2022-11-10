using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusBase : MonoBehaviour
{
    public GameObject textObject;
    Text textComponent;
    public string text;
    public GameDataScript gameData;
    public Color bonColor;
    public Color textColor;
    SpriteRenderer rend;

    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        rend.color = bonColor;
        if (textObject != null)
        {
            textComponent = textObject.GetComponent<Text>();
            textComponent.color = textColor;
            textComponent.text = text;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            this.BonusActivate();
        }

        if ((other.gameObject.name == "Bottom Wall") || (other.gameObject.CompareTag("Player")))
        {
            Destroy(gameObject);
        }
    }

    public virtual void BonusActivate()
    {
        gameData.points += 100;
    }
}
