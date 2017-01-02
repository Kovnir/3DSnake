using UnityEngine;
using System.Collections;
/// <summary>
/// Скрипт отвечающий за столкновение змейки с объектом. 
/// Вешается на любой объект, при столкновении с которым нужно отнимать жизни и отрезать кусок змейки.
/// </summary>
public class BarrierCollision : MonoBehaviour
{
    /// <summary>
    /// Вызывается при событии [trigger enter].
    /// </summary>
    /// <param name="collider">The collider.</param>
    void OnTriggerEnter(Collider collider)
    {
        ISegment segment = AnimalsRegistry.animals[collider.gameObject];    //получаем экземпляр сегмента, который врезался
        if (segment.IsHead())                                               //если это голова
            segment.Collision();                                            //вызываем у неё столкновение
    }
}
