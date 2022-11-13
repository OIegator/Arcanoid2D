using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{

    const int maxLevel = 30;
    [Range(1, maxLevel)]

    public int level = 1;
    public float ballVelocityMult = 0.02f;
    public GameObject bluePrefab;
    public GameObject redPrefab;
    public GameObject greenPrefab;
    public GameObject yellowPrefab;
    public GameObject ballPrefab;
    public GameObject bonusPrefab;


    static Collider2D[] colliders = new Collider2D[50];
    static ContactFilter2D contactFilter = new ContactFilter2D();

    public GameDataScript gameData;

    static bool gameStarted = false;

    // Флаг для перехода на экран конца игры
    public static bool gameFinished = false;

    public static AudioSource audioSrc;
    public AudioClip pointSound;

    public static bool GameIsPaused = false;

    // Поля привязки скрипта к слайдеру и чекбоксу для звука
    [SerializeField]
    Toggle soundToggle;
    [SerializeField]
    Slider soundSlider;

    // Поля привязки скрипта к слайдеру и чекбоксу для музыки
    [SerializeField]
    Toggle musicToggle;
    [SerializeField]
    Slider musicSlider;

    // Флаг нажатой паузы
    public GameObject pauseMenuUI;

    void CreateBlocks(GameObject prefab, float xMax, float yMax, int count, int maxCount)
    {
        if (count > maxCount)
            count = maxCount;
        for (int i = 0; i < count; i++) 
            for (int k = 0; k < 20; k++) {
                var obj = Instantiate(prefab,
                        new Vector3((Random.value * 2 - 1) * xMax,
                        Random.value * yMax, 0),
                        Quaternion.identity);
                if (obj.GetComponent<Collider2D>()
                    .OverlapCollider(contactFilter.NoFilter(), colliders) == 0)
                    break;
                Destroy(obj);
            }
    }

    public void CreateBalls(int plus)
    {
        int count = 2;
        if (gameData.balls == 1)
            count = 1;
        switch (plus) //Проверка, что вызов был совершён из бонусов +2, +10
        {
            case 2:
                count = 2;
                break;
            case 10:
                count = 10;
                break;
        }
        for (int i = 0; i < count; i++)
        {
            var obj = Instantiate(ballPrefab);
            var ball = obj.GetComponent<BallScript>();
            ball.ballInitialForce += new Vector2(10 * i, 0);
            ball.ballInitialForce *= 1 + level * ballVelocityMult;
        }
    }

    void SetBackground()
    {
        var bg = GameObject.Find("Background").GetComponent<SpriteRenderer>();
        bg.sprite = Resources.Load(level.ToString("d2"),
        typeof(Sprite)) as Sprite;
    }

    IEnumerator BlockDestroyedCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        if (GameObject.FindGameObjectsWithTag("Block").Length == 0)
        {
            if (level < maxLevel)
            {
                gameData.level++;
                gameData.Save();
                SceneManager.LoadScene("MainScene");
            }
            if(level == maxLevel) // Проверка завершения игры и переход на экран конца игры
            {
                gameFinished = true;
                gameData.Save();
                SceneManager.LoadScene("Menu");
            }
            
        }

    }

    IEnumerator BallDestroyedCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        if (GameObject.FindGameObjectsWithTag("Ball").Length == 0)
            if (gameData.balls > 0)
                CreateBalls(0);
            else // В случае проигрыша игрока выбрасывает на стартовый экран
            {
                gameData.Reset();
                gameData.Save();
                SceneManager.LoadScene("Menu");
            }
    }
    public void BallDestroyed()
    {
        gameData.balls--;
        StartCoroutine(BallDestroyedCoroutine());
    }

    IEnumerator BlockDestroyedCoroutine2()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.2f);
            // Громкость воспроизведения завист от из значения ползунка громкости звука и его нормализации относительно ползунка громкости музыки. Данные об их значениях берутся из gameData
            audioSrc.PlayOneShot(pointSound, gameData.soundVolume * (1 / audioSrc.volume)); 
        }
    }
    public void BlockDestroyed(int points, string name, Vector3 pos)
    {
        gameData.points += points;
        if (gameData.sound)
            // Громкость воспроизведения завист от из значения ползунка громкости звука и его нормализации относительно ползунка громкости музыки. Данные об их значениях берутся из gameData
            audioSrc.PlayOneShot(pointSound, gameData.soundVolume * (1 / audioSrc.volume));
        gameData.pointsToBall += points;
        if (gameData.pointsToBall >= requiredPointsToBall)
        {
            gameData.balls++;
            gameData.pointsToBall -= requiredPointsToBall;
            if (gameData.sound)
                StartCoroutine(BlockDestroyedCoroutine2());
        }
        StartCoroutine(BlockDestroyedCoroutine());
        if (name == "Green Block(Clone)") // Если разрушен зелёный блок, то вызывается функция создания бонуса
        {
            int[] probab = gameData.getProbab(); // Получаем вероятности из GameData
            CreateBonus(probab, pos);
        }
    }

    public void CreateBonus(int[] probab, Vector3 pos)
    {
        int rand = Random.Range(1, 100);
        var obj = Instantiate(bonusPrefab, pos, Quaternion.identity); // Создаём префаб шаблон бонуса 
        if (rand < probab[0]) // В зависимости от вероятности выбриаем бонус. Кладём в шаблон префаба нужный скрипт, цвет, текст и цвет текста
        {
            obj.AddComponent<Plus10>().gameData = gameData;
            obj.GetComponent<Plus10>().textObject = obj.transform.Find("Canvas").gameObject.transform.Find("Bonus Text").gameObject;
            obj.GetComponent<Plus10>().text = "+10";
            obj.GetComponent<Plus10>().bonColor = Color.blue;
            obj.GetComponent<Plus10>().textColor = Color.white;
            return;
        }
        else if (rand < probab[1])
        {
            obj.AddComponent<Plus2>().gameData = gameData;
            obj.GetComponent<Plus2>().textObject = obj.transform.Find("Canvas").gameObject.transform.Find("Bonus Text").gameObject;
            obj.GetComponent<Plus2>().text = "+2";
            obj.GetComponent<Plus2>().bonColor = Color.blue;
            obj.GetComponent<Plus2>().textColor = Color.white;
            return;
        }
        else if (rand < probab[2])
        {
            obj.AddComponent<BallBonus>().gameData = gameData;
            obj.GetComponent<BallBonus>().textObject = obj.transform.Find("Canvas").gameObject.transform.Find("Bonus Text").gameObject;
            obj.GetComponent<BallBonus>().text = "Ball";
            obj.GetComponent<BallBonus>().bonColor = Color.green;
            obj.GetComponent<BallBonus>().textColor = Color.white;
            return;
        }
        else if (rand < probab[3])
        {
            obj.AddComponent<BonusFast>().gameData = gameData;
            obj.GetComponent<BonusFast>().textObject = obj.transform.Find("Canvas").gameObject.transform.Find("Bonus Text").gameObject;
            obj.GetComponent<BonusFast>().text = "Fast";
            obj.GetComponent<BonusFast>().bonColor = Color.green;
            obj.GetComponent<BonusFast>().textColor = Color.white;
            return;
        }
        else if (rand < probab[4])
        {
            obj.AddComponent<BonusSlow>().gameData = gameData;
            obj.GetComponent<BonusSlow>().textObject = obj.transform.Find("Canvas").gameObject.transform.Find("Bonus Text").gameObject;
            obj.GetComponent<BonusSlow>().text = "Slow";
            obj.GetComponent<BonusSlow>().bonColor = Color.green;
            obj.GetComponent<BonusSlow>().textColor = Color.white;
            return;
        }
        else if (rand < probab[5])
        {
            obj.AddComponent<BonusBase>().gameData = gameData;
            obj.GetComponent<BonusBase>().textObject = obj.transform.Find("Canvas").gameObject.transform.Find("Bonus Text").gameObject;
            obj.GetComponent<BonusBase>().text = "+100";
            obj.GetComponent<BonusBase>().bonColor = Color.yellow;
            obj.GetComponent<BonusBase>().textColor = Color.black;
            return;
        }

    }

    // Включение и выключение воспроизведения музыки и перевод чекбокса в соответстующие положение при инициализации из памяти
    void SetMusic()
    {
        audioSrc.volume = gameData.musicVolume;
        if (gameData.music)
        {
            musicToggle.isOn = true;
            audioSrc.Play();
        }
        else
        {
            musicToggle.isOn = false;
            audioSrc.Stop();
        }
    }

    void StartLevel()
    {
        SetBackground();
        var yMax = Camera.main.orthographicSize * 0.8f;
        var xMax = Camera.main.orthographicSize * Camera.main.aspect * 0.85f;
        CreateBlocks(bluePrefab, xMax, yMax, level, 8);
        CreateBlocks(redPrefab, xMax, yMax, 1 + level, 10);
        CreateBlocks(greenPrefab, xMax, yMax, 1 + level, 12);
        CreateBlocks(yellowPrefab, xMax, yMax, 2 + level, 15);
        CreateBalls(0);
    }

    int requiredPointsToBall
    { get { return 400 + (level - 1) * 20; } }

    // Start is called before the first frame update
    void Start()
    {
        audioSrc = Camera.main.GetComponent<AudioSource>();
        Cursor.visible = false;
        if (!gameStarted)
        {
            gameStarted = true;
            if (gameData.resetOnStart)
                gameData.Load();
        }
        // Вместе со стартом игры ползунки и чекбоксы принимают сохраненные значения из gameData
        soundToggle.isOn = gameData.sound;
        soundSlider.value = gameData.soundVolume;
        musicToggle.isOn = gameData.music;
        musicSlider.value = gameData.musicVolume;
        level = gameData.level;
        SetMusic();
        StartLevel();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0)
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var pos = transform.position;
            pos.x = mousePos.x;
            transform.position = pos;
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            gameData.music = !gameData.music;
            SetMusic();
        }

        if (Input.GetKeyDown(KeyCode.S))
            gameData.sound = !gameData.sound;

        if (Input.GetKeyDown(KeyCode.N))
            Restart();

        // Открытие меню паузы
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
                Resume();
            else
                Pause();
        }
    }

    // Обработчик слайдера со звуком для записи значения в gameData
    public void SetSoundVolume()
    {
        gameData.soundVolume = (int)soundSlider.value;
    }

    // Обработчик слайдера с музыкой для записи значения в gameData
    public void SetMusicVolume()
    {
        audioSrc.volume = musicSlider.value;
        gameData.musicVolume = audioSrc.volume;
    }

    // Обработчик чекбокса со звуком и записью в gameData
    public void SetMusicToggle()
    {
        if (musicToggle.isOn)
        {
            gameData.music = true;
            audioSrc.Play();
        }
        else
        {
            gameData.music = false;
            audioSrc.Stop();
        }
    }


    // Обработчик чебкокса с музыкой и записью в gameData
    public void SetSoundToggle()
    {
        if (soundToggle.isOn)
            gameData.sound = true;
        else
            gameData.sound = false;
    }


    // Обработчик кнорпки рестарта. 
    public void Restart()
    {
        // Обнуление gameData и ее перезапись
        gameData.Reset();
        gameData.Save(); 
        Time.timeScale = 1f; 
        SceneManager.LoadScene("Menu"); // Возврат к стартовому меню
    }

    // Обработчик снятия игры с паузы и установки соответстующиего флага
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    // Обработчик постановки игры на паузу и установки соответстующиего флага
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    // Обработчик кнопки выхода из игры
    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    void OnApplicationQuit()
    {
        gameData.Save();
    }

    string OnOff(bool boolVal)
    {
        return boolVal ? "on" : "off";
    }

    void OnGUI()
    {
        GUI.Label(new Rect(5, 4, Screen.width - 10, 100),
        string.Format(
        "<color=yellow><size=30>Level <b>{0}</b> Balls <b>{1}</b>" +
        " Score <b>{2}</b></size></color>",
        gameData.level, gameData.balls, gameData.points));
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.UpperRight;
        GUI.Label(new Rect(5, 14, Screen.width - 10, 100),
        string.Format(
            "<color=yellow><size=20><color=white>N</color>-new" +
             " <color=white>J</color>-jump" +
             " <color=white>M</color>-music {0}" +
             " <color=white>S</color>-sound {1}" +
             " <color=white>Esc</color>-pause</size></color>",
             OnOff(Time.timeScale > 0), OnOff(!gameData.music),
             OnOff(!gameData.sound)), style);
    }

}
