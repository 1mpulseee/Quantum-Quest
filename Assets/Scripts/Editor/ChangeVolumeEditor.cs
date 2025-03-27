using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(ChangeVolume))]
public class ChangeVolumeEditor : Editor
{
    ChangeVolume main;
    private SerializedProperty GlobalSlider, MusicSlider, EffectsSlider;
    void OnEnable()
    {
        GlobalSlider = serializedObject.FindProperty("GlobalSlider");
        MusicSlider = serializedObject.FindProperty("MusicSlider");
        EffectsSlider = serializedObject.FindProperty("EffectsSlider");
    }
    public override void OnInspectorGUI()
    {
        main = (ChangeVolume)target;
        base.OnInspectorGUI();

        serializedObject.Update();
        if ((main.Type & SoundType.Global) > 0)
        {
            EditorGUILayout.PropertyField(GlobalSlider);
        }
        if ((main.Type & SoundType.Music) > 0)
        {
            EditorGUILayout.PropertyField(MusicSlider);
        }
        if ((main.Type & SoundType.Effects) > 0)
        {
            EditorGUILayout.PropertyField(EffectsSlider);
        }
        serializedObject.ApplyModifiedProperties();
    }
}