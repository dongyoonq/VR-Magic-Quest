using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

public class AlcheyPot : MonoBehaviour
{/*
    [System.Serializable]
    public class Recipe
    {
        public string name;
        public string[] ingredients;
    }

    [System.Serializable]
    public class BrewEvent : UnityEvent<Recipe> { };

    public Recipe[] Recipes;

    private VisualEffect splashVFX;
    public VisualEffect brewEffect;

    public BrewEvent OnBrew;

    [Header("Audio")]
    public AudioSource AmbientSoundSource;
    public AudioSource BrewingSoundSource;
    public AudioClip[] SplashClips;

    bool m_CanBrew = false;

    List<string> m_CurrentIngredientsIn = new List<string>();
    int m_Temperature = 0;
    int m_Rotation = -1;

    float m_StartingVolume;

    private CauldronEffects m_CauldronEffect;

    private void Start()
    {
        m_CauldronEffect = GetComponent<CauldronEffects>();
        splashVFX = SplashEffect.GetComponent<VisualEffect>();

        m_StartingVolume = AmbientSoundSource.volume;
        AmbientSoundSource.volume = m_StartingVolume * 0.2f;
    }

    void OnTriggerEnter(Collider other)
    {
        CauldronIngredient ingredient = other.attachedRigidbody.GetComponentInChildren<CauldronIngredient>();


        Vector3 contactPosition = other.attachedRigidbody.gameObject.transform.position;
        contactPosition.y = gameObject.transform.position.y;

        SplashEffect.transform.position = contactPosition;

        SFXPlayer.Instance.PlaySFX(SplashClips[Random.Range(0, SplashClips.Length)], contactPosition, new SFXPlayer.PlayParameters()
        {
            Pitch = Random.Range(0.8f, 1.2f),
            SourceID = 17624,
            Volume = 1.0f
        }, 0.2f, true);

        splashVFX.Play();

        RespawnableObject respawnableObject = ingredient;
        if (ingredient != null)
        {
            m_CurrentIngredientsIn.Add(ingredient.IngredientType);
        }
        else
        {
            m_CurrentIngredientsIn.Add("INVALID");
            respawnableObject = other.attachedRigidbody.GetComponentInChildren<RespawnableObject>();
        }

        if (respawnableObject != null)
        {
            respawnableObject.Respawn();
        }
        else
        {
            Destroy(other.attachedRigidbody.gameObject, 0.5f);
        }
    }

    public void Brew()
    {
        if (!m_CanBrew)
            return;

        brewEffect.SendEvent("StartLongSpawn");
        CauldronAnimator.SetTrigger("Brew");

        Recipe recipeBewed = null;
        foreach (Recipe recipe in Recipes)
        {
            if (recipe.temperature != m_Temperature || recipe.rotation != m_Rotation)
                continue;

            List<string> copyOfIngredient = new List<string>(m_CurrentIngredientsIn);
            int ingredientCount = 0;
            foreach (var ing in recipe.ingredients)
            {
                if (copyOfIngredient.Contains(ing))
                {
                    ingredientCount += 1;
                    copyOfIngredient.Remove(ing);
                }
            }

            if (ingredientCount == recipe.ingredients.Length)
            {
                recipeBewed = recipe;
                break;
            }
        }

        ResetCauldron();

        StartCoroutine(WaitForBrewCoroutine(recipeBewed));
    }

    IEnumerator WaitForBrewCoroutine(Recipe recipe)
    {
        BrewingSoundSource.Play();
        AmbientSoundSource.volume = m_StartingVolume * 0.2f;
        m_CanBrew = false;
        yield return new WaitForSeconds(3.0f);
        brewEffect.SendEvent("EndLongSpawn");
        CauldronAnimator.SetTrigger("Open");
        BrewingSoundSource.Stop();

        OnBrew.Invoke(recipe);
        m_CanBrew = true;
        AmbientSoundSource.volume = m_StartingVolume;
    }

    void ResetCauldron()
    {
        m_CurrentIngredientsIn.Clear();

    }

    public void Open()
    {
        CauldronAnimator.SetTrigger("Open");
        m_CanBrew = true;
        AmbientSoundSource.volume = m_StartingVolume;
    }*/
}
