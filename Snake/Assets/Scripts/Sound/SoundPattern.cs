using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
[InitializeOnLoad]
#endif

/// <summary>
/// Класс, отвечающий за содержание и управления звуками
/// </summary>
[ExecuteInEditMode]
[System.Serializable] //Сериализуемый класс
public class SoundPattern : ScriptableObject {
    /// <summary>
    /// Звук добавления сегмента змейки
    /// </summary>
    public AudioClip addingClip;
    /// <summary>
    /// Звук отъедения хвоста
    /// </summary>
    public AudioClip cuttingClip;
    /// <summary>
    /// Звук столкновения
    /// </summary>
    public AudioClip collisionClip;
    /// <summary>
    /// Звук конца игры
    /// </summary>
    public AudioClip gameOverClip;
    /// <summary>
    /// Звук стартового отсчёта
    /// </summary>
    public AudioClip countdownClip;

    /// <summary>
    /// Ссылка на себя же
    /// </summary>
    private static SoundPattern _instance;

    /// <summary>
    /// Ссылка на текущий (и единственный) экземпляр класса
    /// </summary>
    /// <value>The instance.</value>
    public static SoundPattern instance
    {
        get
        {
            if (_instance == null)                                          //если экземпляр ещё не был создан
            {
                _instance = Resources.Load("SoundEditor") as SoundPattern;  //найдём его в "ресурсах".
                if (_instance == null)                                      //если там его нет
                    _instance = CreateInstance(typeof(SoundPattern)) as SoundPattern; //создадим новый
            }
            return _instance;                                               //вернём ссылку.
        }
    }
    /// <summary>
    /// Массив источников звука
    /// </summary>
    private AudioSource[] audioSources;
    /// <summary>
    /// Количество источников звука
    /// </summary>
    private const int AUDIO_SOURCES_COUNT = 16;

    /// <summary>
    /// Инициализация.
    /// </summary>
    public void Init(GameObject go)
    {
        audioSources = new AudioSource[AUDIO_SOURCES_COUNT];                //инициализируем массив источников звука
        for (int i = 0; i < AUDIO_SOURCES_COUNT; i ++)                      //идём по их массиву
        {                                                                   //и добавляем к нашему GameObject
            audioSources[i] = go.AddComponent<AudioSource>();               //компоненты AudioSource,
            audioSources[i].volume = 0.8f;
        }                                                                   //попутно сохраняя ссылки на них в массив

    }

    /// <summary>
    /// Проигрывает нужный звук.
    /// </summary>
    /// <param name="clip">Звук, который нужно проиграть.</param>
    private void Play(AudioClip clip)
    {
        if (clip == null)                                                   //если клип пуст
            return;                                                         //уходим
        for (int i = 0; i < AUDIO_SOURCES_COUNT; i++)                       //идём по массиву источников звука
        {                                                                   
            if (!audioSources[i].isPlaying)
            {
                audioSources[i].clip = clip;
                audioSources[i].Play();
                break;
            }
        }                              
    }

    /// <summary>
    /// Програть звук добавления сегмента змейки
    /// </summary>
    public static void PlayAddingSound() { instance.Play(instance.addingClip); }
    /// <summary>
    /// Програть звук отъедения хвоста
    /// </summary>
    public static void PlayCuttingSound() { instance.Play(instance.cuttingClip); }
    /// <summary>
    /// Програть звук столкновения
    /// </summary>
    public static void PlayCollisionSound() { instance.Play(instance.collisionClip); }
    /// <summary>
    /// Програть звук конца игры
    /// </summary>
    public static void PlayGameOverSound() { instance.Play(instance.gameOverClip); }
    /// <summary>
    /// Програть звук стартового отсчёта
    /// </summary>
    public static void PlayCountDownSound() { instance.Play(instance.countdownClip); }
}