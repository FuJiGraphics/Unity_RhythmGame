using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Note : MonoBehaviour
{
    public float speed = 400;
    public NotePosition position = NotePosition.None;
    public float DestroyTime { get; set; } = 0.3f;
    public bool IsJudgeActive { get; set; } = false;
    public bool IsFailedNote { get; set; } = false;
    public float HoldingTime { get; private set; } = 0.0f;
    public BrokenEffect effect = null;
    public Accuracy acc = Accuracy.None;

    private float judgeLine = -4.2f;
    private float hitTime = 0.0f;
    private GameManager manager = null;

    public void DestroyNote()
    {
        manager.DelNote(this);
        if (!IsFailedNote)
        {
            effect.PlayEffect();
        }
        effect.JudgeEffect((int)acc);
        Destroy(this.gameObject);
    }

    private void Start()
    {
        GameObject findGo = GameObject.FindWithTag("GameManager");
        if (findGo != null)
        {
            manager = findGo.GetComponent<GameManager>();
        }
    }

    private void Update()
    {
        if (IsJudgeActive)
        {
            HoldingTime = Mathf.Abs(Time.time - hitTime);
            if (HoldingTime >= DestroyTime)
            {
                HoldingTime = DestroyTime;
                IsFailedNote = true;
                manager.accuracies[Accuracy.Miss]++;
                acc = Accuracy.Miss;
                this.DestroyNote();
            }
            return;
        }

        Vector3 nextDir = Vector3.back * Time.deltaTime * speed;
        Vector3 fixedPos = transform.localPosition + nextDir;
        if (fixedPos.z <= judgeLine)
        {
            fixedPos.z = -4.5f;
            IsJudgeActive = true;
            hitTime = Time.time;
            manager.AddNote(this);
        }
        transform.localPosition = fixedPos;
    }
}
