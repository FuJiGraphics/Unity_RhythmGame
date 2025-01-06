using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public float bpm = 0.0f;
    public float noteSpeed = 10.0f;
    public Note note;
    public BrokenEffect[] brokenEffects;
    public float MusicSink;

    private Transform[] m_ChildSpawnLines;
    private float m_ElapsedTime;
    private SongFile m_SongFile;
    private float m_SongStartTime;
    private int m_CurrentNoteIndex;
    private AudioSource m_AudioSource;
    private bool m_IsPlaying;

    private void Start()
    {
        // Test
        m_SongFile = new SongFile();
        m_ChildSpawnLines = GetComponentsInChildren<Transform>();
        m_ElapsedTime = 0.0f;
        m_SongStartTime = 2.0f;
        m_CurrentNoteIndex = 0;
        m_AudioSource = GetComponent<AudioSource>();
        m_AudioSource.Stop();
        m_IsPlaying = false;
        this.Play();
    }

    private void Update()
    {
        if (m_SongFile == null || m_SongFile.Objects.Length == 0)
            return;

        if (!m_IsPlaying && Time.time >= MusicSink)
        {
            m_AudioSource.Play();
            m_IsPlaying = true;
        }

        m_ElapsedTime = (Time.time - m_SongStartTime) * 1000.0f;
        while (m_CurrentNoteIndex < m_SongFile.Objects.Length && 
            m_SongFile.Objects[m_CurrentNoteIndex].Timing <= m_ElapsedTime + GetNoteTravelTime())
        {
            SpawnNote(m_SongFile.Objects[m_CurrentNoteIndex]);
            m_CurrentNoteIndex++;
        }
    }

    private void SpawnNote(ChartObject chartObject)
    {
        Transform spawnLine = GetSpawnLine(chartObject.Position);

        if (spawnLine != null)
        {
            Note newNote = GameObject.Instantiate(note, spawnLine.position, 
                new Quaternion(90.0f, 90.0f, 0.0f, 0.0f));
            newNote.position = chartObject.Position;
            newNote.speed = noteSpeed;
            newNote.effect = brokenEffects[(int)chartObject.Position];
        }
    }

    private Transform GetSpawnLine(NotePosition position)
    {
        switch (position)
        {
            case NotePosition.L1: return m_ChildSpawnLines[1];
            case NotePosition.L2: return m_ChildSpawnLines[2];
            case NotePosition.R1: return m_ChildSpawnLines[3];
            case NotePosition.R2: return m_ChildSpawnLines[4];
            default: return null;
        }
    }

    private float GetNoteTravelTime()
    {
        return 60.0f / bpm;
    }

    public void Play()
    {
        bool opened = m_SongFile.Parse("Assets/Charts/ETIA. feat.Jenga - Say A Vengeance (LeiN-) [bbu2's Another].osu");
        Debug.Log(opened);
        if (opened)
        {
            bpm = 185.0f;
            m_ElapsedTime = 0.0f;
        }
    }
}
