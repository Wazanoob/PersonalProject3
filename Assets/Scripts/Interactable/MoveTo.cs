using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTo : MonoBehaviour
{
    //References
    private Interactable m_interactable;
    private Rigidbody m_rigidBody;
    private GameManager m_gameManager;
    public Transform m_targetPlayer;
    public Transform m_targetCustomer;

    //Move
    [SerializeField] private Vector3 m_velocity = Vector3.one;
    [SerializeField] private Vector3 m_defaultDistance;
    [SerializeField] private float m_distance;

    private bool m_isSold = false;

    // Start is called before the first frame update
    void Start()
    {
        m_interactable = GetComponent<Interactable>();
        m_rigidBody = GetComponent<Rigidbody>();
        m_gameManager = GameManager.instance;
    }

    void LateUpdate()
    {
        if (m_interactable.isSelected && !m_isSold)
        {
            MoveToTarget();
        }else if(m_isSold)
        {
            MoveToCustomer();
        }

        //if (Input.GetKeyDown(KeyCode.E) && m_interactable.isHighlighted)
        //{
        //    m_rigidBody.isKinematic = true;
        //    m_interactable.isSelected = true;
        //    m_gameManager.isObjectSelected = true;
        //    m_interactable.isHighlighted = false;
        //}
        //else if (Input.GetKeyDown(KeyCode.E) && m_interactable.isSelected)
        //{
        //    m_rigidBody.isKinematic = false;
        //    m_gameManager.isObjectSelected = false;
        //    m_interactable.isSelected = false;
        //}

    }

    void MoveToTarget()
    {
        if (!m_targetPlayer) return;
        Vector3 toPosition = m_targetPlayer.position + (m_targetPlayer.rotation * m_defaultDistance);
        Vector3 currentPosition = Vector3.SmoothDamp(transform.position, toPosition, ref m_velocity, m_distance);
        transform.position = currentPosition;
        transform.LookAt(m_targetPlayer);
    }

    void MoveToCustomer()
    {
        if (!m_targetCustomer) return;
        transform.position = m_targetCustomer.position;
        transform.rotation = new Quaternion(0, -45, 0, 0);

        Interact();
    }

    void Interact()
    {
        if (m_interactable.isHighlighted)
        {
            m_rigidBody.isKinematic = true;
            m_interactable.isSelected = true;
            m_gameManager.isObjectSelected = true;
            m_interactable.isHighlighted = false;
        }else if (m_interactable.isSelected)
        {
            m_rigidBody.isKinematic = false;
            m_gameManager.isObjectSelected = false;
            m_interactable.isSelected = false;
        }
    }

    public void SellToCustomer()
    {
        m_isSold = true;
    }
}
