using UnityEngine;
/// <summary>
/// Прослушиватель кнопки выхода из игры
/// </summary>
public class ExitButtonHandler : MonoBehaviour {

    /// <summary>
    /// Инициализация
    /// </summary>
    void Start () {
        if (Application.isWebPlayer)
            gameObject.SetActive(false);
	}
    /// <summary>
    /// Обработчик события клика.
    /// </summary>
    void OnClick()
    {
        Application.Quit();         //выходим из игры
    }
}
