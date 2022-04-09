using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    //Reference
    private Outline m_highlight;

    public bool isSelected = false;
    public bool isHighlighted = false;

    void Start()
    {
        m_highlight = GetComponent<Outline>();
    }

    private void Update()
    {
        if (m_highlight.enabled == true && isHighlighted == false)
        {
            m_highlight.enabled = false;
        }
    }
    void OnMouseEnter()
    {
        if (!GameManager.instance.isObjectSelected)
        {
            isHighlighted = true;
            m_highlight.enabled = true;
        }
    }

    void OnMouseExit()
    {
        isHighlighted = false;
        m_highlight.enabled = false;
    }
}
