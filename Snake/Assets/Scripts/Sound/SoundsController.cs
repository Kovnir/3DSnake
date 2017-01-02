using UnityEngine;
/// <summary>
/// Класс, управляющий звуками
/// </summary>
public class SoundsController : MonoBehaviour
{
    /// <summary>
    /// Инициализация
    /// </summary>
	void Awake () {
        SoundPattern.instance.Init(gameObject);
    }
}
