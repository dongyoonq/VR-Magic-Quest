using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "Quest")]
public class Quest : ScriptableObject
{
    [SerializeField] public List<QuestData> Questlist;
}
