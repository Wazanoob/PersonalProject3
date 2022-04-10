using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region singleton
    public static AudioManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    //Event
    public delegate void AngryBoi();
    public static event AngryBoi OnMad;

    public delegate void HappyBoi();
    public static event HappyBoi OnHappy;

    //Reference
    [SerializeField] private AudioSource[] m_speakers;
    [SerializeField] private AudioSource m_globalSpeaker;
    [SerializeField] private AudioClip[] m_clips;
    [SerializeField] private ParticleSystem[] m_musicNotes;
    int m_lastRandom = 0;

    //Skin
    [SerializeField] private Material m_customerColor;

    public bool isMad;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_speakers[0].isPlaying && !m_globalSpeaker.isPlaying)
        {
            NextSong();
        }
        
        if (isMad)
        {
            m_customerColor.color += new Color(0.1f, 0, 0) * Time.deltaTime;
        }else if (m_customerColor.color.r > 0)
        {
            m_customerColor.color -= new Color(1f, 0, 0) * Time.deltaTime;
        }
    }

    public void NextSong()
    {
        int random;

        do
        {
            random = Random.Range(0, m_clips.Length);
        } while (random == m_lastRandom);

        if (m_globalSpeaker.isPlaying)
        {
            m_globalSpeaker.Stop();
        }
        else if(random == 7 || random == 8)
        {
            AngryMusic(random);
        }
        else
        {
            HappyMusic(random);
        }
    }

    private void AngryMusic(int clip)
    {
        foreach (AudioSource speakers in m_speakers)
        {
            speakers.Stop();
        }

        foreach (ParticleSystem musicNotes in m_musicNotes)
        {
            var main = musicNotes.main;
            main.startSize = 0.5f;
            main.startSpeed = 0.8f;
        }

        m_globalSpeaker.clip = m_clips[clip];
        m_globalSpeaker.Play();
        m_lastRandom = clip;

        isMad = true;
        OnMad();
    }

    private void HappyMusic(int clip)
    {
        if (m_lastRandom == 7 || m_lastRandom == 8)
        {
            isMad = false;
            OnHappy();

            foreach (ParticleSystem musicNotes in m_musicNotes)
            {
                var main = musicNotes.main;
                main.startSize = 0.1f;
                main.startSpeed = 0.5f;
            }
        }

        for (int i = 0; i < m_speakers.Length; i++)
        {
            m_speakers[i].clip = m_clips[clip];
            m_speakers[i].Play();
            m_lastRandom = clip;
        }
    }
}
