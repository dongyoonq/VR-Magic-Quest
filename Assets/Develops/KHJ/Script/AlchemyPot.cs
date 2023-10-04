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
            PortionRecipeData makeRecipe = new PortionRecipeData();
            makeRecipe.ingredientRune1 = recipe.ingredientRune1;
            makeRecipe.ingredientRune2 = recipe.ingredientRune2;
            makeRecipe.ingredientRune3 = recipe.ingredientRune3;

            foreach (RuneItemData data in m_CurrentIngredientsIn)
            {
                if (data == makeRecipe.ingredientRune1)
                    makeRecipe.ingredientRune1 = null;
                else if (data == makeRecipe.ingredientRune2)
                    makeRecipe.ingredientRune2 = null;
                else if (data == makeRecipe.ingredientRune3)
                    makeRecipe.ingredientRune3 = null;

                if (makeRecipe.ingredientRune1 == null && makeRecipe.ingredientRune2 == null && makeRecipe.ingredientRune3 == null)
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
