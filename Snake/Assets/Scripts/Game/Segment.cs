using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using System;
/// <summary>
/// Поведение животных - сегментов змейки
/// </summary>
public class Segment : MonoBehaviour, ISegment {
    /// <summary>
    /// Дополнительное расстояние между элементами цепочки
    /// </summary>
    private const float OFFSET = 0.005f;
    /// <summary>
    /// Коэффициент линейной интерполяции при догонянии головы хвостом
    /// </summary>
    private const float LERP_FACTOR = 2f;
    /// <summary>
    /// Длительность анимации умирания
    /// </summary>
    public const float DIEING_TIME = 1;
    /// <summary>
    /// Длительность анимации Рождения
    /// </summary>
    public const float BORNING_TIME = 1;
    /// <summary>
    /// Максимальное количество элементов змейки, умирающих при столкновении
    /// </summary>
    public const int DIEING_BY_COLLISION = 3;

    /// <summary>
    /// Закешированный трансформ
    /// </summary>
    private Transform tr;
    /// <summary>
    /// Закешированный CharacterController
    /// </summary>
    private CharacterController characterController;
    /// <summary>
    /// Задержка поворота в топ-даун режиме
    /// </summary>
    public float topDownRotationDamping = 1;
    /// <summary>
    /// Скорость поворота от третьего лица
    /// </summary>
    public float thirdPersonRotationSpeed = 1;
    /// <summary>
    /// Скорость перемещения животного
    /// </summary>
    public float speed = 1;
    /// <summary>
    /// Длина животного
    /// </summary>
    public float size;
    /// <summary>
    /// Масса животного
    /// </summary>
    public float mass = 0.1f;

    /// <summary>
    /// Следующий элемент цепочки
    /// </summary>
    public Transform next;
    /// <summary>
    /// Предыдущий элемент цепочки
    /// </summary>
    public Transform previous;
    /// <summary>
    /// Является ли данный сегмент готовой
    /// </summary>
    private bool head = false;
    /// <summary>
    /// Скорость падения животного (гравитация)
    /// </summary>
    private float vSpeed;
    /// <summary>
    /// Живо ли это животное
    /// </summary>
    public bool alive = true;
    /// <summary>
    /// Масштабирование объекта по умолчанию
    /// </summary>
    [SerializeField]
    private Vector3 standardScale = new Vector3(0.1f, 0.1f, 0.1f);
    /// <summary>
    /// Ранняя инициализация
    /// </summary>
    void Awake()
    {
        tr = transform;                                             //кэшируем трансформ
        characterController = GetComponent<CharacterController>();  //и CharacterController
    }
    /// <summary>
    /// Инициализация.
    /// </summary>
    void Start()
    {
        AnimalsRegistry.animals.Add(gameObject, this);              //записываем себя в список животных
        vSpeed = 9.8f * mass;                                       //высчитываем скорость падения животного
        StartCoroutine("Born");                                     //запускаем анимацию появления
    }
    /// <summary>
    /// Обновление сегмента
    /// </summary>
    void Update()
    {
        if (GameController.instance == null) return;                //если GameController уже был уничтожен - уходим
        if (GameController.instance.globalSpeedFactor == 0) return; //если игра не запущена
        if (GameController.instance.IsGameOver()) return;           //если игра же закончена - уходим
        if (!alive) return;                                         //если животное не живо - уходим
        if (!head)                                                  //если это не голова змейки
        {
            if (next == null) return;                               //если не за кем следить (не успели установить) - уходим
            //вычисляем допустимое расстояние до следуюзего объекта
            float offset = (AnimalsRegistry.animals[next.gameObject].GetSize() + size) / 2 + OFFSET;
            float curOffset = (next.position - tr.position).magnitude;  //получаем расстояние между текущим и сл объектами
            //расчитываем текущий угол поворота
            tr.rotation = Quaternion.LookRotation(next.position - tr.position, Vector3.up);
            if (curOffset > offset)                                 //если текущее смещение больше допустимого расстояния
                //интерполируем, приближаясь к следующему сегменту
                tr.position = Vector3.Lerp(tr.position, next.position, LERP_FACTOR * Time.deltaTime);
        }
        else                                                        //если это голова змейки
        {
            //берём значения стрелоче/джойстика
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
            if (h != 0 || v != 0)                                   //если они не равны нулю
                switch (AppController.cameraType)                   //делаем разные вычисления в зависимости от режима камеры
                {
                    case CameraType.thirdPerson:
                        tr.Rotate(Vector3.up*h*Time.deltaTime * thirdPersonRotationSpeed * 60);
                        break;
                    case CameraType.topDown:
                        //получаем направления движения исходя из этих значений
                        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(h, 0, v));
                        //интерполируем относительно текущего
                        Quaternion newRotation = Quaternion.Slerp(tr.rotation, targetRotation, Time.deltaTime * topDownRotationDamping);
                        //задаём
                        tr.rotation = newRotation;
                        break;
                }
            //двигаем персонажа вперёд с учётом силы тяжести
            characterController.Move(((tr.forward * speed * GameController.instance.globalSpeedFactor - new Vector3(0, vSpeed, 0))) * Time.deltaTime);
        }
    }
    /// <summary>
    /// Анимация умирания
    /// </summary>
    private IEnumerator DieCourutime()
    {
        float time = 0;
        //пока анимация по времени не кончилась
        while (time < DIEING_TIME)
        {
            //уменьшаем масштаб
            tr.localScale -= standardScale * Time.deltaTime;
            yield return null;
            time += Time.deltaTime;
        }
        //удаляем животного из реестра
        AnimalsRegistry.animals.Remove(gameObject);
        //удаляем сам объект со сцены
        Destroy(gameObject);
    }
    /// <summary>
    /// Вызывается при столкновении с препятсвием
    /// </summary>
    /// <param name="collisionLevel">уровень столкновения (важен для уничтожения нескольких сегоментов змейки)</param>
    public void Collision(int collisionLevel = 0)
    {
        if (!alive) return;                             //если мы уже мертвы - уходим
        if (GameController.instance.GetLength() == 1)   //если это единственный сегмент змейки
            if (collisionLevel != 0)                    //но мы уничтожили предыдущих только сейчас
            {
                //говорим камере следить за этим животным
                Camera.main.GetComponent<CameraFollow>().target = tr;
                head = true;                            //назначаем себя головой змейки
                return;                                 //просто продолжим
            }
        Die();                                          //выполняем стандартные операции

        if (collisionLevel == 0)                        //если метод запущен от самого коллайдера
        {
            GameController.instance.DecrementLife();    //уменьшаем жизни
            SoundPattern.PlayCollisionSound();          //играем звук столкновения
        }

        if (GameController.instance.GetLength() == 0)   //если длина змейки равна нулю
        {
            GameController.instance.GameOver();         //принудительным гейм овер
            return;                                     //уходим
        }
        if (collisionLevel < DIEING_BY_COLLISION - 1)   //если уничтожен не последний из секвенции
            AnimalsRegistry.animals[previous.gameObject].Collision(collisionLevel + 1); //сообщаем о коллизии дальше
        if (collisionLevel == DIEING_BY_COLLISION - 1)
        {
            Camera.main.GetComponent<CameraFollow>().target = previous;
            ((Segment) AnimalsRegistry.animals[previous.gameObject]).SetHead();
        }
    }
    /// <summary>
    /// Вызывается при съедении этой части змейки самой змейкой.
    /// </summary>
    public void EatingByHimself()
    {
        if (!alive) return;                             //если мы уже не живы - уходим
        Die();                                          //выполняем стандартные операции
        if (previous != null)
            previous.GetComponent<Segment>().EatingByHimself();
    }
    /// <summary>
    /// стандартные манипуляции, связанные со смертью животного
    /// </summary>
    private void Die()
    {
        alive = false;                                      //говорим, что мы мертвы
        GameController.instance.Die();                      //уменьшаем длину
        characterController.detectCollisions = false;       //чтобы не было столкновения
        tag = "Dieing";                                     //устанавливаем тег "Dieing"
        StartCoroutine("DieCourutime");                     //вызываем корутину смерти
    }
    /// <summary>
    /// Корутина рождения животного.
    /// </summary>
    public IEnumerator Born()
    {
        tr.localScale = Vector3.zero;                       //максимально уменьшаем его в размерах
        float time = 0;                                     //обнуляем таймер
        while (time < BORNING_TIME)                         //если по времени анимация ещё не закончена
        {
            tr.localScale += standardScale * Time.deltaTime;//наращиваем размеры
            time += Time.deltaTime;                         //сдвигаем таймер
            yield return null;                              //ждём следующего кадра
        }
        tr.localScale = standardScale;                      //устанавливаем стандартные размеры
    }
    /// <summary>
    /// Обработчик события [trigger enter]. Запускается при приближении к объекту.
    /// </summary>
    /// <param name="collider">Коллайдер с которым было столкновение.</param>
    void OnTriggerEnter(Collider collider)
    {
        if (!head) return;                                  //если это не голова
        if (collider.gameObject == gameObject) return;      //если сработа на себе же - уходим
        //в зависимости от тега объекта, который инициировал вызов делаем разные действия
        switch (collider.tag)
        {
            case "Mob":                                     //если это незадействованный в змейке моб
                next = collider.transform;                  //запоминаем, что это следующий после нас объект
                collider.GetComponent<Segment>().previous = tr; //говорим ему, что мы перед ним
                SoundPattern.PlayAddingSound();             //проигрываем звук добавления
                collider.tag = "Catched";                   //ставим тег "пойман"
                collider.transform.rotation = tr.rotation;  //задаём ему такой же угол поворота, как у нас
                head = false;                               //мы больше не голова, голова - он
                ((Segment)AnimalsRegistry.animals[collider.gameObject]).SetHead();
                GameController.instance.Eating();           //сообщаем GameController"у о увеличении змейки
                break;
            case "Catched":                                 //если это часть змейки
                if (collider.transform != previous)         //если не предыдущая
                {
                    //если этот объект ещё жив
                    if (AnimalsRegistry.animals[collider.gameObject].IsAlive())
                    {
                        //говорим ему, что он съеден
                        AnimalsRegistry.animals[collider.gameObject].EatingByHimself();
                        //играем музыку отъедения
                        SoundPattern.PlayCuttingSound();
                        //уменьшаем жизни
                        GameController.instance.DecrementLife();
                    }
                }
                break;
        }
    }
    /// <summary>
    /// Устанавливает данный объект головой.
    /// </summary>
    public void SetHead()
    {
        head = true;
        Camera.main.GetComponent<CameraFollow>().target = tr;
    }
    /// <summary>
    /// Является ли данный сегмент головой.
    /// </summary>
    /// <returns>
    /// true, если да
    /// </returns>
    public bool IsHead()
    {
        return head;
    }
    /// <summary>
    /// Возвращает размер животного.
    /// </summary>
    /// <returns>
    /// Размер животного
    /// </returns>
    public float GetSize()
    {
        return size;
    }
    /// <summary>
    /// Живой ли объект
    /// </summary>
    /// <returns>
    /// true, если объект жив
    /// </returns>
    public bool IsAlive()
    {
        return alive;
    }
}
