using UnityEditor.Recorder.Input;
using UnityEditor.Recorder;
using UnityEditor;
using UnityEngine;
using System.Collections;

public class RecordManager : MonoBehaviour
{
    private const string recordfileLocation = "/Editors/Record/";

    private MovieRecorderSettings m_Settings = null;
    private RecorderController m_RecorderController;

    public void StartRecording(string fileName, out bool recordingFlag)
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
        m_Settings.OutputFile = Application.dataPath + recordfileLocation + fileName + " Video";

        // Setup Recording
        controllerSettings.AddRecorderSettings(m_Settings);
        controllerSettings.SetRecordModeToManual();
        controllerSettings.FrameRate = 60.0f;

        RecorderOptions.VerboseMode = false;
        m_RecorderController.PrepareRecording();
        m_RecorderController.StartRecording();

        recordingFlag = true;
    }

    public void StopRecording(float delay)
    {
        StartCoroutine(EndRecording(delay));
    }

    IEnumerator EndRecording(float delay)
    {
        yield return new WaitForSeconds(delay);

        m_RecorderController.StopRecording();
    }
}