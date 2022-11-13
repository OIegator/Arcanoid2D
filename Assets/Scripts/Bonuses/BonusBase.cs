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
        rend.color = bonColor; // Присваиваем префабу нужный цвет
        if (textObject != null)
        {
            textComponent = textObject.GetComponent<Text>(); // Присваиваем префабу нужный текст и цвет текста
            textComponent.color = textColor;
            textComponent.text = text;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other) // Добавляем триггеры. Если коснулся "игрока", то добавить бонус, потом уничтожить. Если коснулся нижней стенки, то просто уничтожить
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

    public virtual void BonusActivate() // Добавляем 100 очков к общему кол-ву
    {
        gameData.points += 100;
    }
}
