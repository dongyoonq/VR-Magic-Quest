using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : BookUI
{
    [SerializeField] MonsterSpawnerHelper monsterSpawnerHelper;
    [SerializeField] MagicWand wand;
    [SerializeField] Ladle ladle;
    [SerializeField] GameObject hammer;

    [SerializeField] Image fadeScreen;
    [SerializeField] Transform returnTransform;
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider sfxSlider;

    private void Awake()
    {
        bgmSlider.onValueChanged.AddListener(x => MusicValueChanged(x));
        sfxSlider.onValueChanged.AddListener(x => SFXValueChanged(x));
    }

    public void CreateWand()
    {
        Instantiate(wand, Camera.main.transform.position + (Camera.main.transform.forward * 2f), Quaternion.identity);
    }

    public void CreateLadle()
    {
        Instantiate(ladle, Camera.main.transform.position + (Camera.main.transform.forward * 2f), Quaternion.identity);
    }

    public void CreateHammer()
    {
        Instantiate(hammer, Camera.main.transform.position + (Camera.main.transform.forward * 2f), Quaternion.identity);
    }

    public void ReturnHome()
    {
        monsterSpawnerHelper.ResetMonsterSpawn();

        player.StartCoroutine(ReturnRoutine());
    }

    IEnumerator ReturnRoutine()
    {
        player.ActiveLocomotion(false);
        bookCanvasUI.ActiveBookUI(false);

        Color startColor = fadeScreen.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 1f);

        float time = 0f;
        float duration = 1f;

        while (time < duration)
        {
            fadeScreen.color = Color.Lerp(startColor, targetColor, time / duration);
            time += Time.deltaTime;

            yield return null;
        }

        player.transform.position = returnTransform.position;
        yield return new WaitForSeconds(1f);

        time = 0f;

        startColor = fadeScreen.color;
        targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        while (time < duration)
        {
            fadeScreen.color = Color.Lerp(startColor, targetColor, time / duration);
            time += Time.deltaTime;

            yield return null;
        }

        player.ActiveLocomotion(true);

        GameManager.Sound.PlayBGM("TownBGM");
    }

    public void MusicValueChanged(float value)
    {
        GameManager.Sound.MusicVolume(value);
    }

    public void SFXValueChanged(float value)
    {
        GameManager.Sound.SFXVolume(value);
    }
}
