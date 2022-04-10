using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        isFirstFree = true;
        isSecondFree = true;
        isThirdFree = true;
        isLastFree = true;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] customers = GameObject.FindGameObjectsWithTag("Customers");

        if (m_newCustomer == null && customers.Length <= 5)
        {
            m_newCustomer = NewCustomer();
            StartCoroutine(m_newCustomer);
        }

        if (foodAvailable.Count == 0)
        {
            //ENDGAME
            Time.timeScale = 0;
        }
    }

    IEnumerator NewCustomer()
    {
        yield return new WaitForSeconds(5);
        Instantiate(m_customer);

        m_newCustomer = null;
    }
}
