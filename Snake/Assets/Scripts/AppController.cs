/// <summary>
/// Класс, отвечающий за хранение глобальных переменных игры
/// </summary>
public static class AppController {
    /// <summary>
    /// Тип управления
    /// </summary>
    public static ControllType controllType = ControllType.keyboard;
    /// <summary>
    /// Тип камеры
    /// </summary>
    public static CameraType cameraType = CameraType.thirdPerson;
    /// <summary>
    /// Сложность игры (от 0 до 2)
    /// </summary>
    public static int difficulty = 1;
}
/// <summary>
/// Тип управления: джойстик или клавиатура
/// </summary>
public enum ControllType
{ joystick, keyboard }
/// <summary>
/// Тип камеры - топ-даун или от третьего лица
/// </summary>
public enum CameraType
{ topDown, thirdPerson }
