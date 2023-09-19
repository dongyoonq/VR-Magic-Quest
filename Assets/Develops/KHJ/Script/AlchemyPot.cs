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
        AmbientSoundSource.volume = m_StartingVolume * 0.5f;
        AmbientSoundSource.Play();
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
            /*
            Vector3 contactPosition = other.attachedRigidbody.gameObject.transform.position;
            contactPosition.y = gameObject.transform.position.y;*/
            GameManager.Sound.PlaySFX(SplashClips[Random.Range(0, SplashClips.Length)]);            

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
        if (m_CurrentIngredientsIn.Count < 1)
            return;

        PortionRecipeData brewRecipe = null;

        foreach (PortionRecipeData recipe in recipes)
        {
            PortionRecipeData makerecipe = recipe;
            foreach (RuneItemData data in m_CurrentIngredientsIn)
            {
                if (data == makerecipe.ingredientRune1)
                    makerecipe.ingredientRune1 = null;
                else if (data == makerecipe.ingredientRune2)
                    makerecipe.ingredientRune2 = null;
                else if (data == makerecipe.ingredientRune3)
                    makerecipe.ingredientRune3 = null;

                if (makerecipe.ingredientRune1 == null && makerecipe.ingredientRune2 == null && makerecipe.ingredientRune3 == null)
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
