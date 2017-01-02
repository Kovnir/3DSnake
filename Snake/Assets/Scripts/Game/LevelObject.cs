using UnityEngine;

/// <summary>
/// Убирает объект, если он не подходит по сложности
/// </summary>
public class LevelObject : MonoBehaviour {

    /// <summary>
    /// Необходимый для этого объекта уровень
    /// </summary>
    [SerializeField]
    private int neededLevel = 0;
    /// <summary>
    /// Инициализация.
    /// </summary>
    void Awake ()
    {
        if (neededLevel > AppController.difficulty)     //если текущий уровень меньше необходимого
            gameObject.SetActive(false);                //убираем этот объект
	}
}
