using UnityEngine;
using System.Collections;
/// <summary>
/// Класс, отвечающий за спаун новых животных.
/// </summary>
public class SpawnManager : MonoBehaviour
{
    /// <summary>
    /// Задержка между спаунами новых животных
    /// </summary>
    private const float SPAWNING_DELAY = 7f;
    /// <summary>
    /// Максимальное количество мобов на сцене
    /// </summary>
    private const int MOB_MAXIMUM = 5;
    /// <summary>
    /// Начальное количество мобов
    /// </summary>
    private const int START_MOB_COUNT = 3;

    /// <summary>
    /// Массив точек спауна
    /// </summary>
    public SpawnPoint[] spawnPoints;
    /// <summary>
    /// Массив объектов животных, которые можно спаунить
    /// </summary>
    public GameObject[] animals;

    /// <summary>
    /// Есть ли пустые спаун-поинты
    /// </summary>
    /// <value>
    /// <c>true</c>, если есть.
    /// </value>
    private bool isEmpty
    {
        get
        {
            foreach (SpawnPoint sp in spawnPoints)
                if (!sp.isBusy) return true;
            return false;
        }
    }
    /// <summary>
    /// Спаунит нового случайного моба в случайном месте.
    /// </summary>
    public void Spawn()
    {
        //если есть пустые спаун поинты и мобоа меньше максимального количетсва
        if (isEmpty && GetMobCount() < MOB_MAXIMUM)
            Instantiate(GetRandomUnit()).transform.position = GetRandomPoint().transform.position;
    }
    /// <summary>
    /// Возвращает количество мобов на сцене.
    /// </summary>
    /// <returns>Количество мобов на сцене</returns>
    private int GetMobCount()
    {
        return GameObject.FindGameObjectsWithTag("Mob").Length;
    }
    /// <summary>
    /// Возвращает ссылку на случайный спаун-поинт
    /// </summary>
    /// <returns>Случайный спаун-поинт</returns>
    SpawnPoint GetRandomPoint()
    {
        while (true)
        {
            int index = Random.Range(0, spawnPoints.Length);
            if (!spawnPoints[index].isBusy)
                return spawnPoints[index];
        }
    }
    /// <summary>
    /// Возвращает случайное животное
    /// </summary>
    /// <returns></returns>
    GameObject GetRandomUnit()
    {
        int index = Random.Range(0, animals.Length);
        return animals[index];
    }
    /// <summary>
    /// Инициализация.
    /// </summary>
    IEnumerator Start()
    {
        //запускаем регулярный спаун с нужным интервалом
        InvokeRepeating("Spawn", SPAWNING_DELAY, SPAWNING_DELAY);
        //создаём 5 стартовых животных с интервалом в пол секунды
        for (int i = 0; i < START_MOB_COUNT; i++)
        {
            Spawn();
            yield return new WaitForSeconds(0.5f);
        }
    }
}
