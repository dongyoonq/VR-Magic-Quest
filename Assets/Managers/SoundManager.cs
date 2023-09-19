using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static SFXPlayer;

public class SoundManager : MonoBehaviour
{
    public List<Sound> musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    private void Start()
    {
        musicSounds = new List<Sound>()
        {
            // Add Sounds
            //CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Bgm/TitleSceneBgm"), "Title1"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/BGM/TownBGM"), "TownBGM"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/BGM/DungeonBGM"), "DungeonBGM"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/BGM/EnemyBGM"), "EnemyBGM"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/BGM/BossBGM1"), "BossBGM1"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/BGM/BossBGM2"), "BossBGM2")
        };

        sfxSounds = new List<Sound>
        {
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Item/BreakStone"), "BreakStone"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Item/GhostAggro"), "GhostAggro"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Item/GhostAttack"), "GhostAttack"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Item/GhostDamaged"), "GhostDamaged"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Item/GhostDeath"), "GhostDeath"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Item/GhostLaugh"), "GhostLaugh"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Item/ItemBagIn"), "ItemBagIn"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Item/SFXCauldronBubbleLoop"), "SFXCauldronBubbleLoop"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Item/SFXCauldronBubbles01"), "SFXCauldronBubbles01"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Item/SFXCauldronBubbles02"), "SFXCauldronBubbles02"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Item/SFXCauldronBubbles03"), "SFXCauldronBubbles03"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Item/SFXCauldronBubbles04"), "SFXCauldronBubbles04"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Item/SFXCorkPop01"), "SFXCorkPop01"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Item/SFXCorkPop02"), "SFXCorkPop02"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Item/SFXImpactGlass01"), "SFXImpactGlass01"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Item/SFXImpactGlass02"), "SFXImpactGlass02"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Item/SFXImpactGlass03"), "SFXImpactGlass03"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Item/SFXImpactGlass04"), "SFXImpactGlass04"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Item/SFXLiquidPourLoop"), "SFXLiquidPourLoop"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Item/SwitchSound"), "SwitchSound"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Item/UiClickSound"), "UiClickSound"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Item/WalkSound1"), "WalkSound1"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Item/WalkSound2"), "WalkSound2"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Item/ZombieAttack"), "ZombieAttack"),
            CreateSound(GameManager.Resource.Load<AudioClip>("Sound/Item/ZombieClassic"), "ZombieClassic")
        };

        GameObject _musicSource = new GameObject();
        _musicSource.AddComponent<AudioSource>();
        _musicSource.name = "Music Source";
        _musicSource.transform.parent = transform;
        musicSource = _musicSource.GetComponent<AudioSource>();

        GameObject _sfxSource = new GameObject();
        _sfxSource.AddComponent<AudioSource>();
        _sfxSource.name = "SFX Source";
        _sfxSource.transform.parent = transform;
        sfxSource = _sfxSource.GetComponent<AudioSource>();

        PlayBGM("TownBGM");
        musicSource.loop = true;
    }

    public void PlayBGM(string name)
    {
        Sound sound = musicSounds.Find(x => x.name == name);

        if (sound == null)
        {

        }
        else
        {
            musicSource.clip = sound.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound sound = sfxSounds.Find(x => x.name == name);

        if (sound == null)
        {

        }
        else
        {
            sfxSource.PlayOneShot(sound.clip);
        }
    }

    public void PlaySFX(AudioClip audioClip)
    {
        Sound sound = sfxSounds.Find(x => x.name == audioClip.name);

        if (sound == null)
        {
            sound = CreateSound(audioClip, audioClip.name);
            sfxSounds.Add(sound);
            sfxSource.PlayOneShot(sound.clip);
        }
        else
        {
            sfxSource.PlayOneShot(sound.clip);
        }
    }

    Sound CreateSound(AudioClip clip, string name)
    {
        Sound sound = new Sound();
        sound.clip = clip;
        sound.name = name;
        return sound;
    }


    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }

    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}