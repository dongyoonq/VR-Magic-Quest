using PDollarGestureRecognizer;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadManager : MonoBehaviour
{
    public const string gesturefileLocation = "/Editors/RecognizeData/";

    public List<Gesture> LoadGestures()
    {
        List<Gesture> gestureList = new List<Gesture>();

        string[] gestureFiles = Directory.GetFiles(Application.dataPath + gesturefileLocation, "*.xml");

        foreach (var item in gestureFiles)
        {
            gestureList.Add(GestureIO.ReadGestureFromFile(item));
        }

        return gestureList;
    }
}