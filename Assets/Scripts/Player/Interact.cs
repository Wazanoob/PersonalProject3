using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Interact : MonoBehaviour
{
    //Reference
    [SerializeField] private TextMeshProUGUI m_interactText;

    // Start is called before the first frame update
    void Start()
    {
        m_interactText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.transform.gameObject.CompareTag("Interactable"))
            {
                if(Input.GetKeyDown(KeyCode.E))
                {
                    hit.transform.gameObject.SendMessage("Interact", SendMessageOptions.DontRequireReceiver);
                }
            }
        }
    }
}
