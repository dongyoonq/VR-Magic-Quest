using PDollarGestureRecognizer;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public List<SkillData> skillList;
    [SerializeField] public int hp;

    [NonSerialized] public bool isSkillUsed;

    public List<Gesture> trainingSet = new List<Gesture>();

    private void Start()
    {
        trainingSet = GameManager.Load.LoadGestures();
    }
}