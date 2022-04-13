using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] private AudioSource m_audioPlayer;
    IEnumerator coroutine;

    //Menu
    [SerializeField] private GameObject m_menu;

    //Summup
    [SerializeField] private GameObject m_summup;
    private bool isEnded = false;

    //Ref
    [SerializeField] private Interact m_interact;
    [SerializeField] private AudioClip m_tutoClick;
    [SerializeField] private AudioClip m_tutoText;
    [SerializeField] private Animator m_doorAnim;


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

        if (m_newCustomer == null && customers.Length < 5 && foodAvailable.Count > customers.Length && m_gameStart)
        {
            m_newCustomer = NewCustomer();
            StartCoroutine(m_newCustomer);
        }
        else if (foodAvailable.Count == 3)
        {
            m_doorAnim.SetBool("Open Door", false);
        }

        if (foodAvailable.Count == 0)
        {
            //ENDGAME
            Summup();
        }

        m_totalCashText.text = "Cash : " + totalCash.ToString() + " $ ";

        if(Input.GetKeyDown(KeyCode.Space) && !m_gameStart)
        {
            if (m_tutoHighlight.activeInHierarchy)
            {
                m_audioPlayer.PlayOneShot(m_tutoClick);

                m_tutoHighlight.SetActive(false);
                Time.timeScale = 1;
            }else if (m_tutoDrop.activeInHierarchy)
            {
                m_audioPlayer.PlayOneShot(m_tutoClick);

                m_doorAnim.SetBool("Open Door",true);
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

        if (Input.GetKeyDown(KeyCode.Escape) && !isEnded)
        {
            Menu();
        }
    }

    public void Menu()
    {
        if (m_menu.activeInHierarchy)
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            m_menu.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.Confined;
            m_menu.SetActive(true);
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("ShopLevel");
    }

    private void Summup()
    {
        isEnded = true;

        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
        m_summup.SetActive(true);


    }

    IEnumerator TutoHighlight()
    {
        yield return new WaitForSeconds(3);
        Time.timeScale = 0;
        m_tutoHighlight.SetActive(true);

        m_audioPlayer.PlayOneShot(m_tutoText);
    }

    IEnumerator TutoDrop()
    {
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0;
        m_tutoDrop.SetActive(true);
        m_audioPlayer.PlayOneShot(m_tutoText);
    }

    IEnumerator NewCustomer()
    {
        yield return new WaitForSeconds(4);
        Instantiate(m_customer);
        m_newCustomer = null;
    }
}
