using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SynergyManager : MonoBehaviour
{
    [Serializable]
    public class IdxCountPair
    {
        public int idx;
        public int count;
    }

    [Serializable]
    public class Synergy
    {
        public IdxCountPair[] idxCountPairs;
    }

    public Synergy[] synergyData;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

[CustomPropertyDrawer(typeof(SynergyManager.IdxCountPair))]
public class IdxCountPairDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty idx = property.FindPropertyRelative("idx");
        SerializedProperty count = property.FindPropertyRelative("count");
        Rect newPosition = EditorGUI.PrefixLabel(position, label);
        float width = newPosition.width;
        newPosition.width = width * 0.5f - 2f;
        newPosition.height -= 2f;

        EditorGUI.BeginProperty(newPosition, label, idx);
        EditorGUI.PropertyField(newPosition, idx, new GUIContent());
        newPosition.x += width * 0.5f;
        newPosition.width = width * 0.5f;
        EditorGUI.PropertyField(newPosition, count, new GUIContent());
        EditorGUI.EndProperty();
    }
}
