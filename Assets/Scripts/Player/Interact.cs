using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Interact : MonoBehaviour
{
    private GameObject m_selectedObject;
    private AudioSource m_audioSource;
    [SerializeField] private AudioClip[] m_audioClip;


    //Penalty
    private int m_wrongFoodPenalty = 3;

    private void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0)
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
            }
            else
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
                                MoveTo selectedObject = m_selectedObject.GetComponent<MoveTo>();
                                selectedObject.m_targetCustomer = customer.targetCustomer;
                                customer.wantedSprite.enabled = false;
                                selectedObject.SellToCustomer();

                                if (customer.wantedFood != null)
                                {
                                    if (m_selectedObject.name == customer.wantedFood.name)
                                    {
                                        //Happy Customer
                                        customer.CashPopup(selectedObject.price, false);
                                    }
                                    else
                                    {
                                        //Sad Customer
                                        int newPrice = selectedObject.price - m_wrongFoodPenalty;
                                        customer.CashPopup(newPrice, true);
                                    }
                                }
                                else
                                {
                                    //Sad Customer
                                    int newPrice = selectedObject.price - m_wrongFoodPenalty;
                                    customer.CashPopup(newPrice, true);
                                }
                                m_selectedObject.transform.parent = customer.targetCustomer.parent;
                                GameManager.instance.foodAvailable.Remove(m_selectedObject);

                                m_selectedObject = null;

                                customer.isEating = true;
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
}
