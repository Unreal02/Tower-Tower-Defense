using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CSVReader
{
    public static List<List<string>> Read(string fileName)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(fileName);
        List<string> stringList = textAsset.ToString().Split('\n').ToList();
        return stringList.Select(list => list.Split(',').ToList()).ToList();
    }
}
