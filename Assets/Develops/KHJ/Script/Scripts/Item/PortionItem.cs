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
    /// <summary> 포션아이템으로 인식시켜주는 collider </summary>
    private Collider portionCollider;
    /// <summary> 포션이 여러번 인식되지 않게 해주는 bool값 </summary>
    private bool isPortionDrink;

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
        isPortionDrink = false;
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

    void Update()
    {
        //포션을 기울이면 뚜껑의 개방 여부에 따라 용액이 흘러나오는 파티클을 실행해줌
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

            //포션을 이미 마신 상태가 아니고, 포션 아래쪽에 포션 리시버가 있다면 포션 리시버안의 함수를 실행
            if (!isPortionDrink)
            {
                RaycastHit hit;
                if (Physics.Raycast(particleSystemLiquid.transform.position, Vector3.down, out hit, 50.0f, ~0, QueryTriggerInteraction.Collide))
                {
                    Player player = hit.collider.GetComponent<Player>();
                    if (player != null)
                    {
                        if (Data is SkillPortionItemData)
                        {
                            SkillPortionItemData skillPortion = Data as SkillPortionItemData;
                            if (player.skillList.Contains(skillPortion.SkillData))
                            {
                                Debug.Log(skillPortion.Name + "스킬포션 마심");
                                isPortionDrink = true;
                            }
                            else
                            {
                                player.skillList.Add(skillPortion.SkillData);
                                Debug.Log(skillPortion.Name + "스킬포션 마심");
                                isPortionDrink = true;
                            }
                        }
                        else if (Data is HillingPortionItemData)
                        {
                            HillingPortionItemData hillPortion = Data as HillingPortionItemData;
                            player.currHp += hillPortion.HillHpValue;
                            player.currMp += hillPortion.HillMpValue;
                            Debug.Log(hillPortion.Name + "회복포션 마심");
                            isPortionDrink = true;
                        }
                        else
                        {
                            Debug.Log("그것 마심");
                        }
                    }
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

    /// <summary> 선택하여 뚜껑을 여는 함수 </summary>
    public void PlugOff()
    {
        if (m_PlugIn)
        {
            m_PlugIn = false;
            m_PlugRb.transform.SetParent(null);
            m_PlugRb.isKinematic = false;
            m_PlugRb.AddRelativeForce(new Vector3(0, 0, 120));
            popVFX.SetActive(true);
            //뚜껑을 열고 나서 InventoryBag에 넣는 것을 방지 하기위해 인식부분을 삭제 
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