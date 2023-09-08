using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class AlchemyPot : MonoBehaviour
{
    [Serializable]
    public class Recipe
    {
        public string name;
        public string[] ingredients;
    }

    [System.Serializable]
    public class BrewEvent : UnityEvent<Recipe> { };

    public Recipe[] Recipes;
    public Transform respawnPosition;

    public BrewEvent OnBrew;

    [Header("Audio")]
    public AudioSource AmbientSoundSource;
    public AudioClip[] SplashClips;

    bool m_CanBrew = false;

    public List<string> m_CurrentIngredientsIn = new List<string>();

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

            SFXPlayer.Instance.PlaySFX(SplashClips[Random.Range(0, SplashClips.Length)], contactPosition, new SFXPlayer.PlayParameters()
            {
                Pitch = Random.Range(0.8f, 1.2f),
                SourceID = 17624,
                Volume = 1.0f
            }, 0.2f, true);

            if (Rune != null)
            {
                m_CurrentIngredientsIn.Add(Rune.Data.Name);
                Destroy(ingredient);
            }
            else
            {
                m_CurrentIngredientsIn.Add("INVALID");

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
        if (m_CanBrew)
            return;
        if (m_CurrentIngredientsIn.Count == 0)
            return;
        Debug.Log("Brew");

        Recipe recipeBewed = null;
        foreach (Recipe recipe in Recipes)
        {
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

            if (ingredientCount == recipe.ingredients.Length && m_CurrentIngredientsIn.Count == recipe.ingredients.Length)
            {
                recipeBewed = recipe;
                break;
            }
        }
        ResetCauldron();
        
        OnBrew.Invoke(recipeBewed);
    }

    void ResetCauldron()
    {
        m_CurrentIngredientsIn.Clear();
    }
}
