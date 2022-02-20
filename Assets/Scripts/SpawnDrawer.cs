using UnityEngine;
using UnityEditor;

// Unity 에디터 화면에 보이는 적 생성 관련된 UI입니다.
[CustomPropertyDrawer(typeof(RoundManager.Spawn))]
public class SpawnDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty time = property.FindPropertyRelative("time");
        SerializedProperty enemy = property.FindPropertyRelative("enemy");
        Rect newPosition = EditorGUI.PrefixLabel(position, label);
        float width = newPosition.width;
        newPosition.width = width * 0.4f - 2f;
        newPosition.height -= 2f;

        EditorGUI.BeginProperty(newPosition, label, time);
        EditorGUI.PropertyField(newPosition, time, new GUIContent());
        newPosition.x += width * 0.4f;
        newPosition.width = width * 0.6f;
        EditorGUI.ObjectField(newPosition, enemy, new GUIContent());
        EditorGUI.EndProperty();
    }
}
