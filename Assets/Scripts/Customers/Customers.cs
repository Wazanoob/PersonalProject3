using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customers : MonoBehaviour
{
    //Reference
    private Animator m_anim;
    private GameManager m_gameManager;
    [SerializeField] private ParticleSystem m_particleEffect;
    [SerializeField] private GameObject[] m_visuals;
    public Transform targetCustomer;

    //Sound
    private AudioSource m_audioSource;
    [SerializeField] private AudioClip m_audioBoing;

    //Feed him
    public bool isEating = false;
    public GameObject wantedFood;

    //Position
    enum Position{Start, Last, Third, Second, First }
    private Position m_myPosition;

    private void OnEnable()
    {
        AudioManager.OnMad += MadAnimation;
        AudioManager.OnHappy += HappyAnimation;
    }

    private void OnDisable()
    {
        AudioManager.OnMad -= MadAnimation;
        AudioManager.OnHappy -= HappyAnimation;
    }

    private void Start()
    {
        m_myPosition = Position.Start;

        
        m_anim = GetComponent<Animator>();
        m_audioSource = GetComponent<AudioSource>();
        m_gameManager = GameManager.instance;

        int random = Random.Range(0, m_gameManager.foodAvailable.Count);
        wantedFood = m_gameManager.foodAvailable[random];

        if (AudioManager.instance.isMad)
        {
            MadAnimation();
        }
    }

    private void Update()
    {
        if (m_gameManager.isLastFree || m_gameManager.isThirdFree || m_gameManager.isSecondFree || m_gameManager.isFirstFree)
        {
            NextPosition(m_myPosition);
        }
    }


    Position NextPosition(Position nextPos)
    {
        switch (nextPos)
        {
            case Position.Start:
                if (m_gameManager.isLastFree)
                {
                    m_anim.SetBool("isLastFree", true);
                    nextPos = Position.Last;
                    m_gameManager.isLastFree = false;
                }
                break;
            case Position.Last:
                if (m_gameManager.isThirdFree)
                {
                    m_anim.SetBool("isThirdFree", true);
                    nextPos = Position.Third;
                    m_gameManager.isLastFree = true;
                    m_gameManager.isThirdFree = false;
                }
                break;
            case Position.Third:
                if (m_gameManager.isSecondFree)
                {
                    m_anim.SetBool("isSecondFree", true);
                    nextPos = Position.Second;
                    m_gameManager.isThirdFree = true;
                    m_gameManager.isSecondFree = false;
                }
                break;
            case Position.Second:
                if (m_gameManager.isFirstFree)
                {
                    m_anim.SetBool("isFirstFree", true);
                    nextPos = Position.First;
                    m_gameManager.isSecondFree = true;
                    m_gameManager.isFirstFree = false;
                }
                break;
            case Position.First:
                break;
        }
        m_myPosition = nextPos;
        return nextPos;
    }

    public void ExitAnimation()
    {
        StartCoroutine(DestroyObject());
    }

    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(2);
        foreach (GameObject visual in m_visuals)
        {
            visual.SetActive(false);
        }
        Instantiate(m_particleEffect, transform);

        yield return new WaitForSeconds(1);
        if (m_myPosition == Position.First)
        {
            m_gameManager.isFirstFree = true;
        }
        else if (m_myPosition == Position.Second)
        {
            m_gameManager.isSecondFree = true;
        }else if (m_myPosition == Position.Third)
        {
            m_gameManager.isThirdFree = true;
        }else if (m_myPosition == Position.Last)
        {
            m_gameManager.isLastFree = true;
        }
        Destroy(gameObject);
    }

    void MadAnimation()
    {
        m_anim.SetBool("AngryBoi", true);
    }

    void HappyAnimation()
    {
        m_anim.SetBool("AngryBoi", false);
    }

    void BoingSound()
    {
        m_audioSource.clip = m_audioBoing;
        m_audioSource.Play();
    }
}
