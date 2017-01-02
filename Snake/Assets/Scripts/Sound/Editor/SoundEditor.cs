using UnityEngine;
using System.Collections;
using UnityEditor;

[ExecuteInEditMode]
[CustomEditor(typeof(SoundPattern))]
public class SoundEditor : Editor {

//    [MenuItem ("Snake/Create new Sound Editor")]
    static void CreateSoundEditor()
    {
        string path = EditorUtility.SaveFilePanel("Create Sound Editor",
                                                  "Assets/Scripts/Tools/Sounds/Resources",
                                                  "SoundEditor.asset", "asset");
        if (path == "")
            return;
        path = FileUtil.GetProjectRelativePath(path);
        SoundPattern sp = SoundPattern.instance;
        AssetDatabase.CreateAsset(sp, path);
        AssetDatabase.SaveAssets();
    }

    private SoundPattern Target
    {
        get { return target as SoundPattern;} 
    }

    [MenuItem("Snake/Sound Editor")]
    static void OpenSoundEditor()
    {
        AssetDatabase.OpenAsset(SoundPattern.instance);
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("В этом окне Вы можете менять все звуки, " +
            "присутвующие в игре.", MessageType.Info);
        DrawClip(ref Target.addingClip, "Добавление сегмента");
        DrawClip(ref Target.cuttingClip, "Откусывание части змейки");
        DrawClip(ref Target.collisionClip, "Столкновение");
        DrawClip(ref Target.gameOverClip, "Проигрыш");
        DrawClip(ref Target.countdownClip, "Стартовый отсчёт");


        if (GUI.changed)                                //если что-то изменилось
            EditorUtility.SetDirty(target);             //устанавливаем Dirty-флаг для сохранения данных на диск
    }


    private void DrawClip(ref AudioClip clip, string name)
    {
        clip = EditorGUILayout.ObjectField(name, clip, typeof(AudioClip), false) as AudioClip;
    }
}
