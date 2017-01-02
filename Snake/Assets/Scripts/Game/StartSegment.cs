using UnityEngine;
/// <summary>
/// Устанавливает животное, на котором висит этот скрипт стартовым.
/// </summary>
public class StartSegment : MonoBehaviour {

    /// <summary>
    /// Инициализация
    /// </summary>
    void Start () {
        GetComponent<Segment>().SetHead();
    }
}
