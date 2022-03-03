using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

// 라운드를 관리합니다.
public class RoundManager : MonoBehaviour
{
    [Serializable]
    public class Spawn
    {
        public float time;
        public GameObject enemy;
    }

    [Serializable]
    public class Round
    {
        public Spawn[] spawns;
    }

    public Round[] rounds;

    private int next = 0;
    private int currentRound = 0;
    private float currentTime = 0;
    private bool onRound = false;
    private Text text;

    void Start()
    {
        text = GameObject.Find("Next Round Button").GetComponentInChildren<Text>();
    }

    void Update()
    {
        if (onRound)
        {
            currentTime += Time.deltaTime;
            if (next < rounds[currentRound].spawns.Length && currentTime >= rounds[currentRound].spawns[next].time)
            {
                Instantiate(rounds[currentRound].spawns[next].enemy, Vector3.zero, Quaternion.identity, transform);
                next++;
            }
            else if (next >= rounds[currentRound].spawns.Length)
            {
                if (transform.childCount == 0)
                {
                    onRound = false;
                    currentRound++;
                }
            }
            text.text = "라운드 진행 중";
        }
        else text.text = "다음 라운드";
    }

    public void OnClickNextRound()
    {
        if (currentRound >= rounds.Length) return;
        if (onRound) return;
        currentTime = 0;
        next = 0;
        onRound = true;
    }

    public int GetCurrentRound()
    {
        return currentRound;
    }
}

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
