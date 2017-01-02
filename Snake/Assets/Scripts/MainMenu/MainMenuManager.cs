using UnityEngine;
/// <summary>
/// Класс, отвечающий за правильную инициализацию начальных условий меню
/// </summary>
public class MainMenuManager : MonoBehaviour {
    /// <summary>
    /// Прослушиватель кнопки смены камеры
    /// </summary>
    [SerializeField]
    private CameraButtonHandler cameraButtonHandler;
    /// <summary>
    /// Прослушиватель кнопки смены слоности
    /// </summary>
    [SerializeField]
    private DifficultyButtonHandler difficultyButtonHandler;
    /// <summary>
    /// Переключатель управления
    /// </summary>
    [SerializeField]
    private UICheckbox checkbox;
    /// <summary>
    /// Инициализация
    /// </summary>
    void Start () {
        //устанавливаем начальные (предыдущие) значения типа камеры
        cameraButtonHandler.SetCameraType(AppController.cameraType);
        //устанавливаем начальные (предыдущие) значения типа управления
        if (AppController.controllType == ControllType.joystick)
            checkbox.isChecked = true;
        else
            checkbox.isChecked = false;
        //устанавливаем начальные (предыдущие) значения сложности игры
        difficultyButtonHandler.SetDifficulty(AppController.difficulty);
    }
}
