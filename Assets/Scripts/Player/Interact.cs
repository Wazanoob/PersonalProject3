using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Interact : MonoBehaviour
{
    private GameObject m_selectedObject;

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (m_selectedObject == null)
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                if (hit.transform.gameObject.CompareTag("Interactable"))
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        hit.transform.gameObject.SendMessage("Interact", SendMessageOptions.DontRequireReceiver);
                        m_selectedObject = hit.transform.gameObject;
                        m_selectedObject.layer = 2;
                    }
                }else if (hit.transform.gameObject.CompareTag("GPE"))
                {
                    if (Input.GetKeyDown(KeyCode.E))
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
                        MoveTo newTarget = m_selectedObject.GetComponent<MoveTo>();
                        Customers customer = hit.transform.gameObject.GetComponent<Customers>();
                        newTarget.m_targetCustomer = customer.targetCustomer;

                        m_selectedObject.SendMessage("SellToCustomer", SendMessageOptions.DontRequireReceiver);

                        m_selectedObject = null;
                        Debug.Log("Feed the boi");
                    }
                    else
                    {
                        m_selectedObject.SendMessage("Interact", SendMessageOptions.DontRequireReceiver);
                        m_selectedObject.layer = 0;
                        m_selectedObject = null;
                    }
                }
            }
        }
    }
}
