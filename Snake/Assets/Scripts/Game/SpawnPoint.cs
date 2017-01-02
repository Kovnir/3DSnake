using UnityEngine;
/// <summary>
/// Класс объекта, указывающего позицию точки спауна.
/// </summary>
public class SpawnPoint : MonoBehaviour {
    /// <summary>
    /// Находится ли в пределах спауна этой точки часть змейки или новый моб.
    /// </summary>
    /// <value>
    ///   <c>true</c> если находится, иначе - <c>false</c>.
    /// </value>
    public bool isBusy
    {
        get { if (count > 0) return true; return false;}
    }
    /// <summary>
    /// Количество животных, находящихся в пределах этой точки спауна
    /// </summary>
    private int count = 0;
    /// <summary>
    /// Обработчик события [trigger enter].
    /// </summary>
    /// <param name="collider">Коллайдер объекта, который вошёл в пределы точки спауна</param>
    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Mob" || collider.tag == "Catched") //если зашёл новый моб или уже занятый в змейки
            count++;                                            //наращиваем их число
    }
    /// <summary>
    /// Обработчик события [trigger exit].
    /// </summary>
    /// <param name="collider">Коллайдер объекта, который вошёл в пределы точки спауна</param>
    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Mob" || collider.tag == "Catched") //если зашёл новый моб или уже занятый в змейки
            count--;                                            //дикрементируем их число
    }
}
