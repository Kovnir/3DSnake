using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
[InitializeOnLoad]
#endif

[System.Serializable] //Сериализуемый класс
                      /// <summary>
                      /// Класс, отвечающий за содержание и управления музыкой
                      /// </summary>
public class MusicPattern : ScriptableObject {
    /// <summary>
    /// Аудиоклип главной темы игры
    /// </summary>
    public AudioClip mainMusic;
    /// <summary>
    /// продлеваем ли переходящие в следующую сцену треки
    /// </summary>
    private bool extendTheRepetitiveTracks = true;
    /// <summary>
    /// источник звука
    /// </summary>
    private AudioSource musicAudioSource;
    /// <summary>
    /// включена ли музыка
    /// </summary>
    private bool _isMusic = true;
    /// <summary>
    /// включена ли музыка
    /// </summary>
    public bool isMusic
    {
        set
        {
            _isMusic = value;                               //устанавливаем новое значение
            if (!value)                                     //если это false
                musicAudioSource.Stop();                    //останавлииваем воспроизведение
            else                                            //иначе
            {                                               //установим музыку настроект, так как
                musicAudioSource.Play();                    //играем её
            }
        }
        get
        {
            return _isMusic;                                //возвращаем значение
        }
    }
    /// <summary>
    /// ссылка на себя же
    /// </summary>
    private static MusicPattern _instance;
    /// <summary>
    /// Ссылка на текущий (и единственный) экpемпляр класса
    /// </summary>
    /// <value>The instance.</value>
    public static MusicPattern instance
    {
        get
        {
            if (_instance == null)                                          //если экземпляр ещё не был создан
            {
                _instance = Resources.Load("MusicEditor") as MusicPattern;  //найдём его в "ресурсах".
                if (_instance == null)                                      //если там его нет
                    _instance = CreateInstance(typeof(MusicPattern))        //создадим новый
                        as MusicPattern; 
            }
            return _instance;                                               //вернём ссылку.
        }
    }

    /// <summary>
    /// Уставовка источника звука.
    /// </summary>
    /// <param name="audioSource"></param>
    public void SetAuidioSource(AudioSource audioSource)
    {
        musicAudioSource = audioSource;                                     //присвоим новый источник
        musicAudioSource.priority = 0;                                      //установим высший приоритет
        musicAudioSource.loop = true;                                       //играем циклично
    }

    /// <summary>
    /// Останавливаем музыку.
    /// </summary>
    public void Stop()
    {
        musicAudioSource.Stop();
    }
	
    /// <summary>
    /// Проигрываем заданный аудиоклип.
    /// </summary>
    /// <param name="clip">аудиоклип, который надо проиграть</param>
    private void Play(AudioClip clip)
    {
        if (!isMusic)                                   //если музыка отключена
            return;                                     //уходим
        if (extendTheRepetitiveTracks)                  //если продлеваем повторяющиеся треки
            if (musicAudioSource.clip == clip)          //и сейчас проигрывается нужный клип
                return;                                 //уходим
        musicAudioSource.Stop();                        //останавливаем предыдущую музыку
        musicAudioSource.clip = clip;                   //ставим новую
        musicAudioSource.Play();                        //играем
    }
    /// <summary>
    /// Начать проигрывать главную тему игры
    /// </summary>
    public void PlayMainMusic()     { Play(mainMusic); } 
}
