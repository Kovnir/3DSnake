using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Прослушиватель кнопки выхода в главное меню.
/// </summary>
public class MainMenuButtonHandler : MonoBehaviour {
    /// <summary>
    /// Обработчик события [click].
    /// </summary>
    void OnClick ()
    {
        SceneManager.LoadScene("MainMenu");         //грузим сцену
    }
}
