using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using PDollarGestureRecognizer;
using System.IO;
using UnityEngine.Events;
using System.Buffers;
using UnityEditor.Recorder.Input;
using UnityEditor.Recorder;
using UnityEditor;

public class MagicWand : XRGrabInteractable
{
    public TrailRenderer vfx;
    public Transform movementSource;
    public UnityEvent<string, float> OnRecognized;

    public float recognitionThreshold = 0.8f;
    public float newPositionThresholdDistance = 0.05f;
    public bool creationMode = true;
    public string newGestureName;

    private bool isMoving = false;
    private bool isRecording = false;
    private float moveDistance;
    private List<Vector3> positionList = new List<Vector3>();
    private Player owner;
    private Collider col;

    private void Start()
    {
        col = GetComponent<Collider>();
        OnRecognized.AddListener(CastingSpell);
    }

    private void Update()
    {
        if (isMoving && !owner.isSkillUsed)
        {
            UpdateMovement();
        }
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        isMoving = false;

        base.OnHoverEntered(args);
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        col.isTrigger = true;

        Debug.Log(args.interactorObject.transform.name);

        if (args.interactorObject.transform != null)
        {
            owner = args.interactorObject.transform.GetComponentInParent<Player>();
            Debug.Log(owner);
        }

        base.OnSelectEntered(args);

        owner.controllerManager.UpdateLocomotionActions();
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        col.isTrigger = false;
        owner = null;

        base.OnSelectExited(args);
    }

    protected override void OnActivated(ActivateEventArgs args)
    {
        if (owner.isSkillUsed)
            return;

        vfx.gameObject.SetActive(true);
        vfx.Clear();

        base.OnActivated(args);

        StartMovement();
    }

    protected override void OnDeactivated(DeactivateEventArgs args)
    {
        if (owner.isSkillUsed)
            return;

        vfx.gameObject.SetActive(false);

        base.OnDeactivated(args);

        EndMovement();
    }

    private void StartMovement()
    {
        if (isRecording)
            return;

        if (creationMode)
        {
            GameManager.Record.StartRecording(newGestureName, out isRecording);
        }

        isMoving = true;
        positionList.Clear();

        positionList.Add(movementSource.position);
    }

    private void EndMovement()
    {
        if (!isMoving)
            return;

        // Create The Gesture From The Position List
        Point[] pointArray = new Point[positionList.Count];

        for (int i = 0; i < positionList.Count; i++)
        {
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(positionList[i]);
            pointArray[i] = new Point(screenPoint.x, screenPoint.y, 0);
        }

        Gesture newGesture = new Gesture(pointArray);

        Vector3 startPos = new Vector3(positionList[0].x, 0f, positionList[0].y);
        Vector3 endPos = new Vector3(positionList[positionList.Count - 1].x, 0f, positionList[positionList.Count - 1].y);
        moveDistance = Vector3.Distance(startPos, endPos);

        // Add a new Gesture to training set
        if (creationMode)
        {
            newGesture.Name = newGestureName;

            if (owner.trainingSet.Count < 8)
            {
                owner.trainingSet.Add(newGesture);
            }
            else
            {
                Debug.Log("The maximum number of motions to learn is up to 8");
            }

            string fileName = Application.dataPath + LoadManager.gesturefileLocation + newGestureName + ".xml";
            GestureIO.WriteGesture(pointArray, newGestureName, fileName);
            Debug.Log($"{fileName}, {newGestureName}, Success Create Gesture");

            // CastingSpell(newGestureName, 1f);

            GameManager.Record.StopRecording(5f);
            IEnumerator StopFlag() { yield return new WaitForSeconds(5f); isRecording = false; }
            StartCoroutine(StopFlag());
        }
        // recognize
        else
        {
            Result result = PointCloudRecognizer.Classify(newGesture, owner.trainingSet.ToArray());
            Debug.Log($"{result.GestureClass} : {result.Score}");

            if (result.Score > recognitionThreshold)
            {
                OnRecognized?.Invoke(result.GestureClass, result.Score);
            }
        }

        isMoving = false;
    }

    private void UpdateMovement()
    {
        Vector3 lastPosition = positionList[positionList.Count - 1];

        if (Vector3.Distance(movementSource.position, lastPosition) > newPositionThresholdDistance)
        {
            positionList.Add(movementSource.position);
        }
    }

    private void CastingSpell(string name, float value)
    {
        foreach (SkillData skills in owner.skillList)
        {
            if (skills.recognizeGestureName == name)
            {
                if (skills.skillPrefab is TeleportSkill)
                    skills.skillPrefab.CastingSpell(owner, moveDistance);
                else
                    skills.skillPrefab.CastingSpell(owner, value, movementSource);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(movementSource.position, movementSource.up);
    }
}
