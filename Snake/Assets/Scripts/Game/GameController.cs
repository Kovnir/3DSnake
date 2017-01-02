using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;
using System.Collections;
/// <summary>
/// Логика игры
/// </summary>
public class GameController : MonoBehaviour {
    /// <summary>
    /// Количество очков, начисляемых за присоединение животного к вереницы
    /// </summary>
    private static int SCORE_BY_EATING = 25;
    /// <summary>
    /// На сколько увеличится глобальный коэффициент скорости при присоединении животного к веренице
    /// </summary>
    private static float TIME_ADDITION_FACTOR = 0.05f;
    /// <summary>
    /// Длительность появляения интерфейсов проигрыша в конце игры
    /// </summary>
    private const float FINAL_COVER_TIME = 1f;
    
    /// <summary>
    /// Коэффициент, влияющий на скорость игры
    /// </summary>
    public float globalSpeedFactor
    { get { return _globalSpeedFactor; } }
    /// <summary>
    /// Коэффициент, влияющий на скорость игры
    /// </summary>
    private float _globalSpeedFactor = 0;

    /// <summary>
    /// Текущая длина змейки
    /// </summary>
    [SerializeField]
    private int length;
    /// <summary>
    /// Количество заработанный очков
    /// </summary>
    [SerializeField]
    private int score;
    /// <summary>
    /// Текущее количество жизней
    /// </summary>
    [SerializeField]
    private int lifes;
    /// <summary>
    /// Текст со всей статистикой
    /// </summary>
    [SerializeField]
    private UILabel stats;
    /// <summary>
    /// Панель, появляющаяся при проигрыше
    /// </summary>
    [SerializeField]
    private UIPanel gameOverPanel;
    /// <summary>
    /// Текст, появляющийся при проигрыше и содержащий количество очей
    /// </summary>
    [SerializeField]
    private UILabel gameOverText;
    /// <summary>
    /// Текст начального отсчёта
    /// </summary>
    [SerializeField]
    private UILabel startCountdown;
    /// <summary>
    /// Запущен ли инвок ежесекундного добавления очей
    /// </summary>
    private bool invokeStarted = false;
    /// <summary>
    /// Окончина ли игра
    /// </summary>
    private bool gameOver;
    /// <summary>
    /// Надо ли ожидать нажатия любой клавиши для вызода в меню
    /// </summary>
    private bool waitingKey;
    /// <summary>
    /// Ссылка на экранный джойстик
    /// </summary>
    [SerializeField]
    private GameObject joystick;

    /// <summary>
    /// Ссылка на текущий (и единственный) экpемпляр класса
    /// </summary>
    public static GameController _instance;
    /// <summary>
    /// Ссылка на текущий (и единственный) экpемпляр класса
    /// </summary>
    public static GameController instance
    {
        get
        {
            return _instance;                                               //вернём ссылку.
        }
    }

    /// <summary>
    /// Ранняя инициализация.
    /// </summary>
    void Awake()
    {
        if (_instance != null)      //если GameController уже был создан - напишим предупреждение
        {
            Debug.LogWarning("GameController allready exist!");
        }
        _instance = this;           //запомним ссылку на себя
        AnimalsRegistry.Init();     //инициализируем реестр находящихся на сцене животный
    }
    /// <summary>
    /// Инициализация.
    /// </summary>
    void Start () {
        //инициализируем стартовые параметры
        _globalSpeedFactor = 0;
        length = 1;
        score = 0;
        lifes = 3;
        UpdateStats();
        gameOver = false;
        waitingKey = false;
        //показываем или прячем экранный джойстик
        switch (AppController.controllType)
        {
            case ControllType.joystick:
                joystick.SetActive(true);
                break;
            case ControllType.keyboard:
                joystick.SetActive(false);
                break;
        }
        //начинаем стартовый отсчёт
        StartCoroutine("StartGame");
    }
    /// <summary>
    /// Подпрограмма стартового отсчёта.
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartGame()
    {
        startCountdown.gameObject.SetActive(true);
        for (int i = 3; i > 0; i--)
        {
            startCountdown.text = i.ToString();
            SoundPattern.PlayCountDownSound();
            yield return new WaitForSeconds(1);
        }
        startCountdown.gameObject.SetActive(false);
        _globalSpeedFactor = 1;
    }
    /// <summary>
    /// обновляем статистику игрока
    /// </summary>
    private void UpdateStats()
    {
        stats.text =
            "Lifes: " + lifes + "\n" +
            "Score: " + score + "\n" +
            "Length: " + length + "\n";
    }
    /// <summary>
    /// Возвращает текущую длину змейки.
    /// </summary>
    /// <returns>Текущая длина змейки</returns>
    public int GetLength()
    {
        return length;
    }
    /// <summary>
    /// Вызывается когда к змейке присоединился ещё один элемент.
    /// </summary>
    public void Eating()
    {
        //увеличиваем скорость
        _globalSpeedFactor += TIME_ADDITION_FACTOR;
        //добавляем очки в зависимости от сложности
        score += SCORE_BY_EATING * (1 + AppController.difficulty);
        //увеличиваем длину
        length++;
        //если добавление очков каждую секунду ещё не было начато - самое время
        if (!invokeStarted)
        {
            StartCoroutine("AddScoreEverySecond");
            invokeStarted = true;
        }
        //обновляем статы
        UpdateStats();
    }
    /// <summary>
    /// Инкремент очков каждую секунду
    /// </summary>
    IEnumerator AddScoreEverySecond()
    {
        //ждём секунду
        yield return new WaitForSeconds(1);
        while (!gameOver)   //пока не кончилась игра
        {
            //добаляем очки и обновляем интеерфейс
            score++;
            UpdateStats();
            yield return new WaitForSeconds(1);
        }
    }
    /// <summary>
    /// вызывается при "умирании" одного из сегментов змейки.
    /// </summary>
    public void Die()
    {
        length --;
    }
    /// <summary>
    /// Функция проигрыша
    /// </summary>
    public void GameOver()
    {
        _globalSpeedFactor = 0;                 //останавливаем змейку
        //обновляем назпись в конце игры
        gameOverText.text = "GAME OVER!\nScore: " + score + "\n\npress any key";
        StartCoroutine("GameOverCourutine");    //запускаем анимацию появления финальной статистики
        gameOver = true;                        //устанавливаем флаг проигрыша
        SoundPattern.PlayGameOverSound();       //проигрываем музыку проигрыша
    }
    /// <summary>
    /// Анимация проигрыша.
    /// </summary>
    IEnumerator GameOverCourutine()
    {
        //делаем медленное появление этого экрана
        float time = 0;
        while (time < FINAL_COVER_TIME)
        {
            gameOverPanel.alpha = time/ FINAL_COVER_TIME;
            yield return null;
            time += Time.deltaTime;
        }
        gameOverPanel.alpha = FINAL_COVER_TIME;
        //включаем ожидание нажатия кнопки
        waitingKey = true;
    }
    /// <summary>
    /// Обновление.
    /// </summary>
    void Update()
    {
        if (waitingKey)                                 //если ждём эникей
            if (Input.anyKey)                           //и он нажат
            {
                _instance = null;                       //забываем ссылку на себя
                AnimalsRegistry.Destroy();              //уничтожаем реестр животных
                SceneManager.LoadScene("MainMenu");     //грузим меню
            }
    }

    /// <summary>
    /// Уменьшение количетсва жизней.
    /// </summary>
    public void DecrementLife()
    {
        lifes--;                        //уменьшаем количество жизней
        UpdateStats();                  //обновляем статы
        if (lifes == 0)                 //если их 0
            GameOver();                 //заканчиваем игру
    }
    /// <summary>
    /// Закончина ли игра
    /// </summary>
    /// <returns>true, если игра закончена</returns>
    public bool IsGameOver()
    {
        return gameOver;
    }

}
