using System;
using System.Collections.Generic;
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
        public string enemy;
        public GameObject enemyObject;
    }

    [Serializable]
    public class Round
    {
        public List<Spawn> spawns;
    }

    [Serializable]
    public class Rounds
    {
        public List<Round> rounds;
    }

    private Rounds roundInfo;
    public string roundInfoFileName;

    private int next = 0;
    private int currentRound = 0;
    private float currentTime = 0;
    private bool onRound = false;
    private Text text;

    void Start()
    {
        text = GameObject.Find("Next Round Button").GetComponentInChildren<Text>();

        // json 파싱
        TextAsset textAsset = Resources.Load<TextAsset>(roundInfoFileName);
        roundInfo = JsonUtility.FromJson<Rounds>(textAsset.ToString());

        // Spawn.enemy(string)에서 Spawn.enemyObject(GameObject)로 변환
        foreach (Round round in roundInfo.rounds)
        {
            foreach (Spawn spawn in round.spawns)
            {
                spawn.enemyObject = Resources.Load<GameObject>("Objects/" + spawn.enemy);
            }
        }
    }

    void Update()
    {
        if (onRound)
        {
            currentTime += Time.deltaTime;
            if (next < roundInfo.rounds[currentRound].spawns.Count && currentTime >= roundInfo.rounds[currentRound].spawns[next].time)
            {
                Instantiate(roundInfo.rounds[currentRound].spawns[next].enemyObject, Vector3.zero, Quaternion.identity, transform);
                next++;
            }
            else if (next >= roundInfo.rounds[currentRound].spawns.Count)
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
        if (currentRound >= roundInfo.rounds.Count) return;
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
