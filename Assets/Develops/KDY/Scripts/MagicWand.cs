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
    private const string gesturefileLocation = "/Editors/RecognizeData/";
    private const string recordfileLocation = "/Editors/Record/";

    public GameObject linePrefab;
    public Transform movementSource;
    public UnityEvent<string> OnRecognized;

    public float recognitionThreshold = 0.9f;
    public float newPositionThresholdDistance = 0.05f;
    public bool creationMode = true;
    public string newGestureName;

    private bool isMoving = false;
    private List<Vector3> positionList = new List<Vector3>();
    private List<Gesture> trainingSet = new List<Gesture>();
    private MovieRecorderSettings m_Settings = null;
    private RecorderController m_RecorderController;
    private Player owner;
    private Collider col;

    private void Start()
    {
        col = GetComponent<Collider>();
        OnRecognized.AddListener(CastingSpell);

        string[] gestureFiles = Directory.GetFiles(Application.dataPath + gesturefileLocation, "*.xml");
       
        foreach (var item in gestureFiles)
        {
            trainingSet.Add(GestureIO.ReadGestureFromFile(item));
        }
    }

    private void Update()
    {
        if (isMoving)
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

        if (args.interactorObject.transform != null)
        {
            owner = args.interactorObject.transform.GetComponentInParent<Player>();
        }

        base.OnSelectEntered(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        col.isTrigger = false;
        owner = null;

        base.OnSelectExited(args);
    }

    protected override void OnActivated(ActivateEventArgs args)
    {
        base.OnActivated(args);

        StartMovement();
    }

    protected override void OnDeactivated(DeactivateEventArgs args)
    {
        base.OnDeactivated(args);

        EndMovement();
    }

    private void StartMovement()
    {
        if (creationMode)
            StartRecording();

        isMoving = true;
        positionList.Clear();

        positionList.Add(movementSource.position);

        if (linePrefab)
        {
            Destroy(GameManager.Resource.Instantiate(linePrefab, movementSource.position, Quaternion.identity), 1.5f);
        }
    }

    private void EndMovement()
    {
        isMoving = false;

        // Create The Gesture From The Position List
        Point[] pointArray = new Point[positionList.Count];

        for (int i = 0; i < positionList.Count; i++)
        {
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(positionList[i]);
            pointArray[i] = new Point(screenPoint.x, screenPoint.y, 0);
        }

        Gesture newGesture = new Gesture(pointArray);

        // Add a new Gesture to training set
        if (creationMode)
        {
            newGesture.Name = newGestureName;
            trainingSet.Add(newGesture);

            string fileName = Application.dataPath + gesturefileLocation + newGestureName + ".xml";
            GestureIO.WriteGesture(pointArray, newGestureName, fileName);
            Debug.Log($"{fileName}, {newGestureName}, Success Create Gesture");

            CastingSpell(newGestureName);

            StartCoroutine(EndRecording());
        }
        // recognize
        else
        {
            Result result = PointCloudRecognizer.Classify(newGesture, trainingSet.ToArray());
            Debug.Log($"{result.GestureClass} : {result.Score}");

            if (result.Score > recognitionThreshold)
            {
                OnRecognized?.Invoke(result.GestureClass);
            }
        }
    }

    private void UpdateMovement()
    {
        Vector3 lastPosition = positionList[positionList.Count - 1];

        if (Vector3.Distance(movementSource.position, lastPosition) > newPositionThresholdDistance)
        {
            positionList.Add(movementSource.position);

            if (linePrefab)
            {
                Destroy(GameManager.Resource.Instantiate(linePrefab, movementSource.position, Quaternion.identity), 1.5f);
            }
        }
    }

    private void CastingSpell(string name)
    {
        foreach (SkillData skills in owner.skillList)
        {
            if (skills.recognizeGestureName == name)
            {
                GameManager.Resource.Instantiate(skills.skillPrefab, Camera.main.transform.position + (Camera.main.transform.forward * 6f), Quaternion.identity);
            }
        }
    }


    private void StartRecording()
    {
        var controllerSettings = ScriptableObject.CreateInstance<RecorderControllerSettings>();
        m_RecorderController = new RecorderController(controllerSettings);

        // Video
        m_Settings = ScriptableObject.CreateInstance<MovieRecorderSettings>();
        m_Settings.name = "My Video Recorder";
        m_Settings.Enabled = true;

        // This example performs an MP4 recording
        m_Settings.OutputFormat = MovieRecorderSettings.VideoRecorderOutputFormat.MP4;
        m_Settings.VideoBitRateMode = VideoBitrateMode.High;

        m_Settings.ImageInputSettings = new GameViewInputSettings
        {
            OutputWidth = 1920,
            OutputHeight = 1080
        };

        m_Settings.AudioInputSettings.PreserveAudio = true;

        // Simple file name (no wildcards) so that FileInfo constructor works in OutputFile getter.
        m_Settings.OutputFile = Application.dataPath + recordfileLocation + newGestureName + " Video";

        // Setup Recording
        controllerSettings.AddRecorderSettings(m_Settings);
        controllerSettings.SetRecordModeToManual();
        controllerSettings.FrameRate = 60.0f;

        RecorderOptions.VerboseMode = false;
        m_RecorderController.PrepareRecording();
        m_RecorderController.StartRecording();
    }

    private IEnumerator EndRecording()
    {
        yield return new WaitForSeconds(5f);

        m_RecorderController.StopRecording();
    }
}
