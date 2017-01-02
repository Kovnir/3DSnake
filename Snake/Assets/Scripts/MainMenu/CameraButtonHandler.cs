using UnityEngine;
/// <summary>
/// Прослушиватель кнопки смены камеры в меню
/// </summary>
public class CameraButtonHandler : MonoBehaviour {
    /// <summary>
    /// Переключена ли кнопка
    /// </summary>
    private bool pressed = false;
    /// <summary>
    /// Текст на кнопке
    /// </summary>
    [SerializeField]
    private UILabel text;

    /// <summary>
    /// Прослушивание события [click].
    /// </summary>
    void OnClick () {
        pressed = !pressed;                 //переключаем кнопку в противоположное значение 
        UpdateText();                       //обновляем текст кнопки
    }

    /// <summary>
    /// Установка типа камеры
    /// </summary>
    /// <param name="ct">Новый тип камеры</param>
    public void SetCameraType(CameraType ct)
    {
        if (ct == CameraType.topDown)       //преобразуем знчение типа в булеан
            pressed = true;
        else
            pressed = false;
        UpdateText();                       //обновляем текст кнопки
    }
    /// <summary>
    /// Обновление текста кнопки.
    /// </summary>
    private void UpdateText()
    {
        if (pressed)                        //если нажата - пишем топ даун
            text.text = "Top Down Camera";
        else                                //иначе - от третьего лица
            text.text = "Third Persone Camera";
    }
    /// <summary>
    /// Возвращает выбранный тип камеры
    /// </summary>
    /// <returns></returns>
    public CameraType GetCameraType()
    {
        return pressed ? CameraType.topDown : CameraType.thirdPerson;
    }
}
