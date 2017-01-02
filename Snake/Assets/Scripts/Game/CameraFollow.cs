using UnityEngine;
using System.Collections;
/// <summary>
/// Поведение камеры
/// </summary>
public class CameraFollow : MonoBehaviour
{
    /// <summary>
    /// Объект, за которым следит камера
    /// </summary>
    public Transform target;
    /// <summary>
    /// Затухание слеженич
    /// </summary>
    [SerializeField]
    private float moveDamping;
    /// <summary>
    /// Коэффициент смещения по оси Y в топ-даун режиме
    /// </summary>
    public float topDownYOffset = 1f;
    /// <summary>
    /// Коэффициент смещения по оси Y в режиме от третьего лица
    /// </summary>
    public float ThirdPersonYOffset = 1f;
    /// <summary>
    /// Коэффициент смещения от игрока назад в режиме от третьего лица
    /// </summary>
    public float ThirdPersonBackwordOffset = 0.5f;
    /// <summary>
    /// Затухание вращения
    /// </summary>
    public float rotationDamping = 7f;
    /// <summary>
    /// Кешированый трансформ
    /// </summary>
    private Transform tr;
    /// <summary>
    /// Инициализация
    /// </summary>
    void Awake()
    {
        tr = transform;
    }
    /// <summary>
    /// Обновление каждый кадр
    /// </summary>
    void Update()
    {
        if (GameController.instance.IsGameOver()) return;               //если игра окончена - уходим
        if (target == null) return;                                     //если не за кем следить - уходим
        switch (AppController.cameraType)                               //комутатор режима камеры
        {
            case CameraType.thirdPerson:                                //если от третьего лица
                //высчитываем новое идеальное положение камеры 
                Vector3 newPosition = target.position - (target.forward* ThirdPersonBackwordOffset) + new Vector3(0, ThirdPersonYOffset, 0);
                //интерполируем его с текущим и записываем
                tr.position = Vector3.Lerp(tr.position, newPosition, Time.deltaTime);
                //высчитываем идеальный угол поворота
                Quaternion wantedRotation = Quaternion.LookRotation(target.position - tr.position, target.up);
                //интерполируем его с текущим и записываем
                tr.rotation = Quaternion.Slerp(tr.rotation, wantedRotation, Time.deltaTime * rotationDamping);
                break;
            case CameraType.topDown:                                    //топ-даун режим
                //считаем коэфициент отдаления
                float factor = (GameController.instance.globalSpeedFactor - 1) / 2 + 1;
                //считаем и присваеваем новую позицию
                tr.position = Vector3.Slerp(tr.position, target.position + new Vector3(0, topDownYOffset * factor, 0), moveDamping);
                break;
        }
    }
}
