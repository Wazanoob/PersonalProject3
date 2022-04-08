using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    //Reference
    private AudioManager m_audioManager;


    // Start is called before the first frame update
    void Start()
    {
        m_audioManager = AudioManager.instance;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Interact()
    {
        m_audioManager.NextSong();
    }

}
