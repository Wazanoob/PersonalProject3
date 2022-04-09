using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customers : MonoBehaviour
{
    //Reference
    private Animator m_anim;
    public Transform targetCustomer;

    //Sound
    private AudioSource m_audioSource;
    [SerializeField] private AudioClip m_audioBoing;


    private void OnEnable()
    {
        AudioManager.OnMad += MadAnimation;
        AudioManager.OnHappy += HappyAnimation;
    }

    private void OnDisable()
    {
        AudioManager.OnMad -= MadAnimation;
        AudioManager.OnHappy -= HappyAnimation;
    }

    private void Start()
    {
       m_anim = GetComponent<Animator>();
       m_audioSource = GetComponent<AudioSource>();
    }

    void MadAnimation()
    {
        m_anim.SetBool("AngryBoi", true);
    }

    void HappyAnimation()
    {
        m_anim.SetBool("AngryBoi", false);
    }

    void BoingSound()
    {
        m_audioSource.clip = m_audioBoing;
        m_audioSource.Play();
    }
}
