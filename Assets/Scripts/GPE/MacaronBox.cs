using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacaronBox : MonoBehaviour
{
    [SerializeField] private Transform m_targetPosition;

    public Vector3 velocity = Vector3.one;
    public Vector3 defaultDistance;
    public float distance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!m_targetPosition) return;
        Vector3 toPosition = m_targetPosition.position + (m_targetPosition.rotation * defaultDistance);
        Vector3 currentPosition = Vector3.SmoothDamp(transform.position, toPosition, ref velocity, distance);
        transform.position = currentPosition;
        transform.LookAt(m_targetPosition);
    }

    void Interact()
    {

    }
}
