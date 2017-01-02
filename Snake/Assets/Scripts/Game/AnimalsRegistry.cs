using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// Кеширует данные о игровых объектах, сопоставление важные компоненты с ссылками на них
/// </summary>
public static class AnimalsRegistry
{
    /// <summary>
    /// Список существующих на сцене животных
    /// </summary>
    public static Dictionary<GameObject, ISegment> animals;
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="AnimalsRegistry"/>
    /// </summary>
    public static void Init()
    {
        if (animals != null) return;
        animals = new Dictionary<GameObject, ISegment>();    //создаём новый массив
    }
    /// <summary>
    /// Забывает список животных
    /// </summary>
    public static void Destroy()
    {
        animals = null;
    }
}

