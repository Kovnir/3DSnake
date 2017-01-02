using UnityEngine;
/// <summary>
/// Класс, управляющий музыкой
/// </summary>
public class MusicController : MonoBehaviour {
    /// <summary>
    /// Инициализация
    /// </summary>
    void Start () {
        MusicPattern.instance.SetAuidioSource(GetComponent<AudioSource>());     //устанавливаем источник звука с этого объекта
        MusicPattern.instance.PlayMainMusic();                                  //включаем главную тему игры
    }
}
