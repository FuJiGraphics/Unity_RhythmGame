using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public float bpm = 0.0f;
    public float noteSpeed = 10.0f;
    public Note note;

    private Transform[] m_ChildSpawnLines;
    private float m_ElapsedTime = 0.0f;
    private float m_NormalBpm = 0.0f;

    private void Start()
    {
        // Test
        m_ChildSpawnLines = GetComponentsInChildren<Transform>();
        this.Play();
    }

    private void Update()
    {
        m_ElapsedTime += Time.deltaTime;
        if (m_ElapsedTime >= 60.0f / bpm)
        {
            m_ElapsedTime -= 60.0f / bpm;
            var transform = m_ChildSpawnLines[0];
            transform.position = new Vector3(0.0f, transform.position.y, transform.position.z);
            Note newNote = GameObject.Instantiate(note, transform);
            newNote.speed = noteSpeed;
        }
    }

    public void Play()
    {
        m_ElapsedTime = 0.0f;
        m_NormalBpm = 60.0f / bpm;
    }
}
