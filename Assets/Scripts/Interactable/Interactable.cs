using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private Outline highlight;

    // Start is called before the first frame update
    void Start()
    {
        highlight = GetComponent<Outline>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnMouseEnter()
    {
        highlight.enabled = true;
    }

    void OnMouseExit()
    {
        highlight.enabled = false;
    }
}
