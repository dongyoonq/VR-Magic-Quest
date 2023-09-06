using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

/// <summary> 수량 아이템 - 포션 아이템 </summary>
public class PortionItem : CountableItem
{
    public PortionItem(PortionItemData data) : base(data) { }

    static int NextFreeUniqueId = 3000;

    public string PotionType = "Default";
    public GameObject plugObj;
    public ParticleSystem particleSystemLiquid;
    public float fillAmount = 0.8f;
    public GameObject popVFX;
    [FormerlySerializedAs("meshRenderer")]
    public MeshRenderer MeshRenderer;

    [Header("Audio")]
    public AudioClip PouringClip;
    public AudioClip[] PoppingPlugAudioClip;

    bool m_PlugIn = true;
    Rigidbody m_PlugRb;
    MaterialPropertyBlock m_MaterialPropertyBlock;
    private Collider portionCollider;

    int m_UniqueId;

    AudioSource m_AudioSource;
    float m_StartingFillAmount;

    void OnEnable()
    {
        particleSystemLiquid.Stop();

        m_MaterialPropertyBlock = new MaterialPropertyBlock();
        m_MaterialPropertyBlock.SetFloat("LiquidFill", fillAmount);

        MeshRenderer.SetPropertyBlock(m_MaterialPropertyBlock);
        m_PlugRb = plugObj.GetComponent<Rigidbody>();
        popVFX.SetActive(false);
        m_StartingFillAmount = fillAmount;
        m_PlugIn = true;
        portionCollider = GetComponent<Collider>();
    }

    void Start()
    {
        m_AudioSource = SFXPlayer.Instance.GetNewSource();
        m_AudioSource.gameObject.transform.SetParent(particleSystemLiquid.transform, false);
        m_AudioSource.gameObject.SetActive(true);

        m_AudioSource.clip = PouringClip;
        m_AudioSource.maxDistance = 2.0f;
        m_AudioSource.minDistance = 0.2f;
        m_AudioSource.loop = true;

        m_UniqueId = NextFreeUniqueId++;
    }

    void OnDestroy()
    {
        Destroy(m_AudioSource.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Dot(transform.up, Vector3.down) > 0 && fillAmount > 0 && m_PlugIn == false)
        {
            if (particleSystemLiquid.isStopped)
            {
                particleSystemLiquid.Play();
                m_AudioSource.Play();
            }

            fillAmount -= 0.1f * Time.deltaTime;

            float fillRatio = fillAmount / m_StartingFillAmount;

            m_AudioSource.pitch = Mathf.Lerp(1.0f, 1.4f, 1.0f - fillRatio);

            RaycastHit hit;
            if (Physics.Raycast(particleSystemLiquid.transform.position, Vector3.down, out hit, 50.0f, ~0, QueryTriggerInteraction.Collide))
            {
                PotionReceiver receiver = hit.collider.GetComponent<PotionReceiver>();

                if (receiver != null)
                {
                    receiver.ReceivePotion(PotionType);
                }
            }

        }
        else
        {
            particleSystemLiquid.Stop();
            m_AudioSource.Stop();
        }

        MeshRenderer.GetPropertyBlock(m_MaterialPropertyBlock);
        m_MaterialPropertyBlock.SetFloat("LiquidFill", fillAmount);
        MeshRenderer.SetPropertyBlock(m_MaterialPropertyBlock);
    }

    public void PlugOff()
    {
        if (m_PlugIn)
        {
            m_PlugIn = false;
            m_PlugRb.transform.SetParent(null);
            m_PlugRb.isKinematic = false;
            m_PlugRb.AddRelativeForce(new Vector3(0, 0, 120));
            popVFX.SetActive(true);
            Destroy(portionCollider);

            m_PlugIn = false;

            plugObj.transform.parent = null;

            SFXPlayer.Instance.PlaySFX(PoppingPlugAudioClip[Random.Range(0, PoppingPlugAudioClip.Length)], m_PlugRb.transform.position, new SFXPlayer.PlayParameters()
            {
                Pitch = Random.Range(0.9f, 1.1f),
                Volume = 1.0f,
                SourceID = -99
            });
        }
    }
}