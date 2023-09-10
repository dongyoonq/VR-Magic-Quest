using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MP4Loader : MonoBehaviour
{
    [Header("MP4 Player Panel, File Name")]
    [SerializeField] private RectTransform panelMP4Player;
    [SerializeField] private TMP_Text textFileName;

    [Header("MP4 Play Time (Slider, Text)")]
    [SerializeField] private Slider sliderPlaybar;
    [SerializeField] private TMP_Text textCurrentPlayetime;
    [SerializeField] private TMP_Text textMaxPlaytime;

    [Header("Play Video & Audio")]
    [SerializeField] private RawImage rawImageDrawVideo;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private AudioSource audioSource;

    public void OnLoad(FileInfo file)
    {
        panelMP4Player.gameObject.SetActive(true);
        textFileName.text = file.Name;
        ResetPlaytimeUI();
        StartCoroutine(LoadVideo(file.FullName));
    }

    private IEnumerator LoadVideo(string fullPath)
    {
        videoPlayer.url = "file://" + fullPath;

        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSource);

        audioSource.clip = null;

        rawImageDrawVideo.texture = videoPlayer.targetTexture;

        videoPlayer.Prepare();

        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        Play();
    }

    public void OffLoad()
    {
        Stop();

        panelMP4Player.gameObject.SetActive(false);
    }

    public void Play()
    {
        videoPlayer.Play();
        audioSource.Play();

        StartCoroutine("OnPlaytimeUI");
    }

    public void Pause()
    {
        videoPlayer.Pause();
        audioSource.Pause();
    }

    public void Stop()
    {
        videoPlayer.Stop();
        audioSource.Stop();

        StopCoroutine("OnPlaytimeUI");
        ResetPlaytimeUI();
    }

    private void ResetPlaytimeUI()
    {
        sliderPlaybar.value = 0;
        textCurrentPlayetime.text = "00:00:00";
        textMaxPlaytime.text = "00:00:00";
    }

    private IEnumerator OnPlaytimeUI()
    {
        int hour = 0;
        int minutes = 0;
        int seconds = 0;

        while (true)
        {
            hour = (int)videoPlayer.time / 3600;
            minutes = (int)(videoPlayer.time % 3600) / 60;
            seconds = (int)(videoPlayer.time % 3600) % 60;
            textCurrentPlayetime.text = $"{hour:D2}:{minutes:D2}:{seconds:D2}";

            hour = (int)videoPlayer.length / 3600;
            minutes = (int)(videoPlayer.length % 3600) / 60;
            seconds = (int)(videoPlayer.length % 3600) % 60;
            textMaxPlaytime.text = $"{hour:D2}:{minutes:D2}:{seconds:D2}";

            sliderPlaybar.value = (float)(videoPlayer.time / videoPlayer.length);

            yield return new WaitForSeconds(1f);
        }
    }
}