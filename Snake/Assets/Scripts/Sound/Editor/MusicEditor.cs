using UnityEngine;
using System.Collections;
using UnityEditor;

[ExecuteInEditMode]                         //исполняется в режиме редактора
[CustomEditor(typeof(MusicPattern))]      //реализует интерфейс GarbagePattern
public class MusicEditor : Editor {

    //[MenuItem ("Snake/Create new Music Editor")]
    /// <summary>
    /// Создаёт новый реестр музыки.
    /// Не ипользуйте эту функцию, пока работает предыдущий реестр.
    /// Два риестра, работающие одновременно могут сломать вашу жизнь. И, возможно, игру.
    /// </summary>
    static void CreateMusicEditor()
    {
        string path = EditorUtility.SaveFilePanel(                          //вызываем окно сохранения
                                                  "Create Music Editor",                            //имя окна
                                                  "Assets/Scripts/Tools/Sound/Resources",   //путь
                                                  "MusicEditor.asset", "asset");                    //название по умолчанию и тип
        if (path == "")                                 //если нажали отмену
            return;                                     //уходим
        path = FileUtil.GetProjectRelativePath(path);   //получаем отностительный путь
        MusicPattern mp = MusicPattern.instance;    //берём ссылку на существующий, или создаём новый патерн
        AssetDatabase.CreateAsset(mp, path);            //создаём ассет по заданному пути
        AssetDatabase.SaveAssets();                     //сохраняем его.
    }
    
    /// <summary>
    /// Получаем текущий объект в виде GarbagePattern.
    /// </summary>
    /// <value>Текущий объект в виде GarbagePattern.</value>
    private MusicPattern Target
    {
        get { return target as MusicPattern; }
    }
    
    /// <summary>
    /// Открывает сущесвующий ассет реестра мусора.
    /// </summary>
    [MenuItem("Snake/Music Editor")]
    static void OpenGarbageEditor()
    {
        AssetDatabase.OpenAsset(MusicPattern.instance);   //Открываем ассет по его ссылке
    }

    /// <summary>
    /// Эта функция имплементирована для создания собственного инспектора.
    /// Внутри неё добавлены все пользовательские элементы Gui
    /// </summary>
    public override void OnInspectorGUI()
    {
        //Приветствие
        EditorGUILayout.HelpBox("Это окно позволит вам задовать треки, " +
            "для разных экранов игры.", MessageType.Info);

        DrawClip(ref Target.mainMusic, "Главная тема");

        if (GUI.changed)                                //если что-то изменилось
            EditorUtility.SetDirty(target);             //устанавливаем Dirty-флаг для сохранения данных на диск
    }

    private void DrawClip(ref AudioClip clip, string name)
    {
        clip = EditorGUILayout.ObjectField(name, clip, typeof(AudioClip), false) as AudioClip;
    }


    }
    