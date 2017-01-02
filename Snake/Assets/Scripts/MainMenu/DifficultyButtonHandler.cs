using UnityEngine;
/// <summary>
/// Прослушиватель кнопки смены уровня сложности в меню
/// </summary>
public class DifficultyButtonHandler : MonoBehaviour {
    /// <summary>
    /// Текущая сложность
    /// </summary>
    private int difficulty = 1;
    /// <summary>
    /// Текст на кнопке
    /// </summary>
    [SerializeField]
    private UILabel text;

    /// <summary>
    /// Обработчик события клика
    /// </summary>
    void OnClick()
    {
        difficulty++;                           //увеличиваем сложность
        if (difficulty == 3) difficulty = 0;    //если дошли до конца - сбрасываем на "простой"
        UpdateText();                           //обновляем текст
    }
    /// <summary>
    /// Устанавливает сложность игры.
    /// </summary>
    /// <param name="complexity">Новая сложность.</param>
    public void SetDifficulty(int difficulty)
    {
        this.difficulty = difficulty;           //запомним сложность
        UpdateText();                           //обновим тест на кнопке
    }
    /// <summary>
    /// Обновляет текст на кнопке.
    /// </summary>
    private void UpdateText()
    {
        switch (difficulty)
        {
            case 0:
                text.text = "Easy difficulty";
                break;
            case 1:
                text.text = "Normal difficulty";
                break;
            case 2:
                text.text = "Hard difficulty";
                break;
        }
    }
    /// <summary>
    /// Возвращает текущую выбранную сложность
    /// </summary>
    /// <returns></returns>
    public int GetDifficulty()
    {
        return difficulty;
    }
}
