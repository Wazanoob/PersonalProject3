using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region singleton
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    //Free Position
    public bool isFirstFree;
    public bool isSecondFree;
    public bool isThirdFree;
    public bool isLastFree;

    //Object Selected
    public bool isObjectSelected = false;

    //Customers
    [SerializeField] private Customers m_customer;
    private IEnumerator m_newCustomer;

    //Food
    public List<GameObject> foodAvailable = new List<GameObject>();
    public GameObject lastFoodWanted;

    //Cash
    public int totalCash = 0;
    [SerializeField] private TextMeshProUGUI m_totalCashText;

    //Tuto
    public bool m_gameStart = false;
    [SerializeField] private GameObject m_tutoHighlight;
    [SerializeField] private GameObject m_tutoDrop;
    IEnumerator coroutine;

    //Ref
    [SerializeField] private Interact m_interact;
    [SerializeField] private AudioClip m_tutoClick;


    // Start is called before the first frame update
    void Start()
    {
        isFirstFree = true;
        isSecondFree = true;
        isThirdFree = true;
        isLastFree = true;

        m_tutoHighlight.SetActive(false);
        m_tutoDrop.SetActive(false);

        StartCoroutine(TutoHighlight());
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] customers = GameObject.FindGameObjectsWithTag("Customers");

        if (m_newCustomer == null && customers.Length < 5 && foodAvailable.Count > 4 && m_gameStart)
        {
            m_newCustomer = NewCustomer();
            StartCoroutine(m_newCustomer);
        }

        if (foodAvailable.Count == 0)
        {
            //ENDGAME
            Time.timeScale = 0;
        }

        m_totalCashText.text = "Cash : " + totalCash.ToString() + " $ ";

        if(Input.GetKeyDown(KeyCode.Space) && !m_gameStart)
        {
            AudioSource audioPlayer = m_interact.GetComponent<AudioSource>();

            if (m_tutoHighlight.activeInHierarchy)
            {
                audioPlayer.PlayOneShot(m_tutoClick);

                m_tutoHighlight.SetActive(false);
                Time.timeScale = 1;
            }else if (m_tutoDrop.activeInHierarchy)
            {
                audioPlayer.PlayOneShot(m_tutoClick);

                m_gameStart = true;
                m_tutoDrop.SetActive(false);
                Time.timeScale = 1;
            }
        }

        if (!m_gameStart && isObjectSelected)
        {
            if (coroutine == null)
            {
                coroutine = TutoDrop();
                StartCoroutine(coroutine);
            }
        }
    }

    IEnumerator TutoHighlight()
    {
        yield return new WaitForSeconds(3);
        Time.timeScale = 0;

        m_tutoHighlight.SetActive(true);
    }

    IEnumerator TutoDrop()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Spamming");

        Time.timeScale = 0;

        m_tutoDrop.SetActive(true);
    }

    IEnumerator NewCustomer()
    {
        int random = Random.Range(1, 6);
        yield return new WaitForSeconds(random);
        Instantiate(m_customer);

        m_newCustomer = null;
    }
}
