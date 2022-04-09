using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Reference
    [SerializeField] private float m_mouseSensitivity;
    [SerializeField] private Transform m_playerBody;

    private float m_xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float m_mouseX = Input.GetAxisRaw("Mouse X") * m_mouseSensitivity * Time.deltaTime;
        float m_mouseY = Input.GetAxisRaw("Mouse Y") * m_mouseSensitivity * Time.deltaTime;

        m_xRotation -= m_mouseY;
        m_xRotation = Mathf.Clamp(m_xRotation, -15f, 90f);

        transform.localRotation = Quaternion.Euler(m_xRotation, 0f, 0f);
        m_playerBody.Rotate(Vector3.up * m_mouseX);

    }
}
