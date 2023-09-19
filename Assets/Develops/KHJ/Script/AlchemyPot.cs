using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class AlchemyPot : MonoBehaviour
{
    [System.Serializable]
    public class BrewEvent : UnityEvent<PortionRecipeData> { };

    public PortionRecipeData[] recipes;
    public Transform respawnPosition;

    public BrewEvent OnBrew;

    [Header("Audio")]
    public AudioSource AmbientSoundSource;
    public AudioClip[] SplashClips;

    bool m_CanBrew = true;

    public List<RuneItemData> m_CurrentIngredientsIn = new List<RuneItemData>();

    float m_StartingVolume;

    private void Start()
    {
        m_StartingVolume = AmbientSoundSource.volume;
        AmbientSoundSource.volume = m_StartingVolume * 0.2f;
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject ingredient = other.gameObject;
        RuneItem Rune = ingredient.GetComponent<RuneItem>();
        Ladle ladle = ingredient.GetComponent<Ladle>();

        if (ladle != null)
        {
            Brew();
        }
        else
        {
            Vector3 contactPosition = other.attachedRigidbody.gameObject.transform.position;
            contactPosition.y = gameObject.transform.position.y;

            /*
            SFXPlayer.Instance.PlaySFX(SplashClips[Random.Range(0, SplashClips.Length)], contactPosition, new SFXPlayer.PlayParameters()
            {
                Pitch = Random.Range(0.8f, 1.2f),
                SourceID = 17624,
                Volume = 1.0f
            }, 0.2f, true);
            */

            if (Rune != null)
            {
                if (m_CurrentIngredientsIn.Count > 3)
                {
                    Rigidbody rb = ingredient.GetComponentInParent<Rigidbody>();

                    if (rb != null)
                    {
                        rb.transform.position = respawnPosition.position;
                    }
                    else
                    {
                        ingredient.transform.position = respawnPosition.position;
                    }

                    return;
                }

                m_CurrentIngredientsIn.Add(Rune.Data as RuneItemData);
                Destroy(ingredient);
            }
            else
            {
                Rigidbody rb = ingredient.GetComponentInParent<Rigidbody>();

                if (rb != null)
                {
                    rb.transform.position = respawnPosition.position;
                }
                else
                {
                    ingredient.transform.position = respawnPosition.position;
                }
            }
        }
    }

    public void Brew()
    {
        if (!m_CanBrew)
            return;
        if (m_CurrentIngredientsIn.Count < 3)
            return;

        m_CanBrew = false;

        Debug.Log("Brew");

        PortionRecipeData brewRecipe = null;

        foreach (PortionRecipeData recipe in recipes)
        {
            int cnt = 0;

            foreach (RuneItemData data in m_CurrentIngredientsIn)
            {
                if (data == recipe.ingredientRune1)
                    cnt++;
                else if (data == recipe.ingredientRune2)
                    cnt++;
                else if (data == recipe.ingredientRune3)
                    cnt++;

                if (cnt == 3)
                {
                    brewRecipe = recipe;

                    ResetCauldron();
                    OnBrew.Invoke(brewRecipe);
                    return;
                }
            }
        }

        OnBrew.Invoke(brewRecipe);
        ResetCauldron();
    }

    void ResetCauldron()
    {
        m_CurrentIngredientsIn.Clear();
    }
}
