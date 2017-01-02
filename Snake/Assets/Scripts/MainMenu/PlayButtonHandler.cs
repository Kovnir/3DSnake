using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;
/// <summary>
/// Прослушиватель кнопки начала игры в меню
/// </summary>
public class PlayButtonHandler : MonoBehaviour {
    /// <summary>
    /// Ссылка на чекбокс управления
    /// </summary>
    [SerializeField] private UICheckbox checkbox;
    /// <summary>
    /// Ссылка на прослушиватель кнопки смены камеры
    /// </summary>
    [SerializeField]
    private CameraButtonHandler cameraButton;
    /// <summary>
    /// Ссылка на прослушиватель кнопки смены сложности
    /// </summary>
    [SerializeField]
    private DifficultyButtonHandler difficultyButtonHandler;
    /// <summary>
    /// Ссылка на панель с кнопками
    /// </summary>
    [SerializeField]
    private GameObject menuPanel;
    /// <summary>
    /// Ссылка на текст с надписью "Загрузка"
    /// </summary>
    [SerializeField]
    private GameObject loadingText;
    /// <summary>
    /// Прослушивание события [click].
    /// </summary>
    void OnClick()
    {
        //запоминаем выбранный тип камеры
        AppController.cameraType = cameraButton.GetCameraType();
        //запоминаем выбранный тип управления
        AppController.controllType = checkbox.isChecked ? ControllType.joystick : ControllType.keyboard;
        //в зависимости от него инициализируем один из методов управления
        InitControl(AppController.controllType);
        //запоминае выбранную сложность игры
        AppController.difficulty = difficultyButtonHandler.GetDifficulty();
        //пряем панель с кнопками
        menuPanel.SetActive(false);
        //показываем текст загрузки
        loadingText.SetActive(true);
        //грузим уровень
        SceneManager.LoadSceneAsync("GameScene");
    }

    /// <summary>
    /// Инициализирует один из методов управления
    /// </summary>
    private void InitControl(ControllType controllType)
    {
        switch (controllType)
        {
            case ControllType.joystick:
                CrossPlatformInputManager.SetJoystick();
                break;
            case ControllType.keyboard:
                CrossPlatformInputManager.SetKeyboard();
                break;
        }
    }

}
