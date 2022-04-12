using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Customers : MonoBehaviour
{
    //Reference
    private Animator m_anim;
    private GameManager m_gameManager;
    [SerializeField] private ParticleSystem m_particleEffect;
    [SerializeField] private GameObject[] m_visuals;
    public Transform targetCustomer;
    public Transform m_targetPopup;

    //Sound
    private AudioSource m_audioSource;
    [SerializeField] private AudioClip m_audioBoing;

    //Feed him
    public bool isEating = false;
    public GameObject wantedFood;
    public Sprite[] spriteList;
    public Image wantedSprite;

    //Mad
    private bool m_isMad;

    //CashOut
    private float m_cashPenalty =  0;

    //CashPopup
    [SerializeField] private TextMeshPro m_popup;
    private TextMeshPro m_cashPopup;
    private float popupTime = 1.5f;

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

        if (AudioManager.instance.isMad)
        {
            MadAnimation();
        }
    }

    private void Update()
    {
        if (m_gameManager.isLastFree || m_gameManager.isThirdFree || m_gameManager.isSecondFree || m_gameManager.isFirstFree)
        {
            StartCoroutine(RandomWait());
        }

        if (m_cashPopup != null)
        {
            DestroyPopup();
        }

        if (m_isMad)
        {
            m_cashPenalty += (Time.deltaTime) / 3;
        }else
        {
            m_cashPenalty += (Time.deltaTime) / 10;
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

                    WantedFood();
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
    public void CashPopup(int amount, bool isMad)
    {


        int cashOut = amount - Mathf.FloorToInt(m_cashPenalty);
        cashOut = Mathf.Clamp(cashOut, 1, amount);

        TextMeshPro textMesh = Instantiate(m_popup, m_targetPopup.position, Quaternion.identity);

        if (isMad)
        {
            textMesh.faceColor = new Color(1f, 0f, 0f, 1f);
            MadAnimation();
        }else
        {
            textMesh.faceColor = new Color(0f, 1f, 0f, 1f);
        }

        textMesh.text = (cashOut).ToString() + " $ ";
        m_gameManager.totalCash += cashOut;

        Transform transform = textMesh.transform;
        transform.SetParent(m_targetPopup);

        m_cashPopup = textMesh;
    }

    private void DestroyPopup()
    {
        m_cashPopup.transform.localPosition += new Vector3(0, 1, 0) * Time.deltaTime;
        m_cashPopup.alpha -= 1 * Time.deltaTime;

        popupTime -= Time.deltaTime;

        if (popupTime <= 0f)
        {
            Destroy(m_cashPopup.gameObject);
        }
    }

    private void WantedFood()
    {
        if (m_gameManager.lastFoodWanted != null)
        {
            do
            {
                int random = Random.Range(0, m_gameManager.foodAvailable.Count);
                wantedFood = m_gameManager.foodAvailable[random];
            } while (wantedFood == m_gameManager.lastFoodWanted);
        }else
        {
            int random = Random.Range(0, m_gameManager.foodAvailable.Count);
            wantedFood = m_gameManager.foodAvailable[random];
        }

        m_gameManager.lastFoodWanted = wantedFood;

        //Hardcoding
        //Dont have time to implement it well sorry :((
        if (wantedFood.name == "Cupcake_Oreo")
        {
            wantedSprite.sprite = spriteList[5];
        }
        else if (wantedFood.name == "Cupcake_Cherry")
        {
            wantedSprite.sprite = spriteList[4];
        }
        else if (wantedFood.name == "Cheesecake_Blueberry")
        {
            wantedSprite.sprite = spriteList[7];
        }
        else if (wantedFood.name == "Cheesecake_Chocolate")
        {
            wantedSprite.sprite = spriteList[6];
        }
        else if (wantedFood.name == "MacaronBox")
        {
            wantedSprite.sprite = spriteList[0];
        }
        else if (wantedFood.name == "Donut_White")
        {
            wantedSprite.sprite = spriteList[1];
        }
        else if (wantedFood.name == "Donut_Pink")
        {
            wantedSprite.sprite = spriteList[2];
        }
        else if (wantedFood.name == "Donut_Black")
        {
            wantedSprite.sprite = spriteList[3];
        }
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
        m_isMad = true;
    }

    void HappyAnimation()
    {
        m_anim.SetBool("AngryBoi", false);
        m_isMad = false;
    }

    void BoingSound()
    {
        m_audioSource.clip = m_audioBoing;
        m_audioSource.Play();
    }

    IEnumerator RandomWait()
    {
        float random = Random.Range(2f, 10f);
        yield return new WaitForSeconds(random);
        NextPosition(m_myPosition);
    }
}
