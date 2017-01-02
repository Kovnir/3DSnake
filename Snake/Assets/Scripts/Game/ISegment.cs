/// <summary>
/// Интерфейс сегмента змейки
/// </summary>
public interface ISegment
{
    /// <summary>
    /// Является ли данный сегмент головой.
    /// </summary>
    /// <returns>true, если да</returns>
    bool IsHead();
    /// <summary>
    /// Вызывается при столкновении с препятсвием
    /// </summary>
    /// <param name="collisionLevel">уровень столкновения (важен для уничтожения нескольких сегоментов змейки)</param>
    void Collision(int collisionLevel = 0);
    /// <summary>
    /// Вызывается при съедении этой части змейки самой змейкой.
    /// </summary>
    void EatingByHimself();
    /// <summary>
    /// Возвращает размер животного.
    /// </summary>
    /// <returns>Размер животного</returns>
    float GetSize();
    /// <summary>
    /// Живой ли объект
    /// </summary>
    /// <returns>true, если объект жив</returns>
    bool IsAlive();
}

