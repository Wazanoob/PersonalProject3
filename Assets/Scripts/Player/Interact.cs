using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Interact : MonoBehaviour
{
    private GameObject m_selectedObject;
    private AudioSource m_audioSource;
    [SerializeField] private AudioClip[] m_audioClip;

    private void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (m_selectedObject == null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (Physics.Raycast(transform.position, transform.forward, out hit))
                {
                    if (hit.transform.gameObject.CompareTag("Interactable"))
                    {
                        hit.transform.gameObject.SendMessage("Interact", SendMessageOptions.DontRequireReceiver);
                        m_selectedObject = hit.transform.gameObject;
                        m_selectedObject.layer = 2;

                        m_audioSource.PlayOneShot(m_audioClip[0]);
                    }
                    else if (hit.transform.gameObject.CompareTag("GPE"))
                    {
                        hit.transform.gameObject.SendMessage("Interact", SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
        }else
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (Physics.Raycast(transform.position, transform.forward, out hit))
                {
                    if (hit.transform.gameObject.CompareTag("Customers"))
                    {
                        Customers customer = hit.transform.gameObject.GetComponent<Customers>();
                        if (!customer.isEating)
                        {
                            MoveTo newTarget = m_selectedObject.GetComponent<MoveTo>();
                            newTarget.m_targetCustomer = customer.targetCustomer;
                            newTarget.SellToCustomer();
                            customer.isEating = true;
                            m_selectedObject.transform.parent = customer.targetCustomer.parent;

                            GameManager.instance.foodAvailable.Remove(m_selectedObject);

                            if (m_selectedObject.name == customer.wantedFood.name)
                            {
                                //Happy Customer
                                Debug.Log("Miam");
                            }else
                            {
                                //Sad Customer
                                Debug.Log("Erk");
                            }

                            m_selectedObject = null;

                            customer.ExitAnimation();
                        }
                    }
                    else
                    {
                        m_selectedObject.SendMessage("Interact", SendMessageOptions.DontRequireReceiver);
                        m_selectedObject.layer = 0;
                        m_selectedObject = null;

                        m_audioSource.PlayOneShot(m_audioClip[1]);
                    }
                }
            }
        }
    }
}
