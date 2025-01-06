using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum Accuracy
{
    Miss, Bad, Good, Great, None
}

public struct AccuracyTiming
{
    public float Bad;
    public float Good;
};

public class GameManager : MonoBehaviour
{
    public KeyCode[] InputKeys { get; set; }
    public Dictionary<Accuracy, int> accuracies;
    public BrokenEffect[] brokenEffects;

    private Dictionary<NotePosition, Dictionary<Note, Note>> accNotes;
    private AccuracyTiming accTiming;

    private void Start()
    {
        InputKeys = new KeyCode[4];
        InputKeys[0] = KeyCode.D;
        InputKeys[1] = KeyCode.F;
        InputKeys[2] = KeyCode.J;
        InputKeys[3] = KeyCode.K;

        accNotes = new Dictionary<NotePosition, Dictionary<Note, Note>>();
        accNotes.Add(NotePosition.L1, new Dictionary<Note, Note>());
        accNotes.Add(NotePosition.L2, new Dictionary<Note, Note>());
        accNotes.Add(NotePosition.R1, new Dictionary<Note, Note>());
        accNotes.Add(NotePosition.R2, new Dictionary<Note, Note>());

        accuracies = new Dictionary<Accuracy, int>();
        accuracies.Add(Accuracy.Miss, 0);
        accuracies.Add(Accuracy.Bad, 0);
        accuracies.Add(Accuracy.Good, 0);
        accuracies.Add(Accuracy.Great, 0);

        accTiming.Bad = 0.15f;
        accTiming.Good = 0.064f;
    }

    private void Update()
    {
        for (int i = 0; i < InputKeys.Length; ++i)
        {
            if (Input.GetKeyDown(InputKeys[i]))
            {
                CheckAccuracy(i);
                brokenEffects[i].LightEffect(true);
            }
            else if (Input.GetKeyUp(InputKeys[i]))
            {
                brokenEffects[i].LightEffect(false);
            }
        }
        Debug.Log($"Miss = {accuracies[Accuracy.Miss]}");
        Debug.Log($"Bad = {accuracies[Accuracy.Bad]}");
        Debug.Log($"Good {accuracies[Accuracy.Good]}");
        Debug.Log($"Great {accuracies[Accuracy.Great]}");
    }

    public void AddNote(Note note)
    {
        accNotes[note.position].Add(note, note);
    }

    public void DelNote(Note note)
    {
        accNotes[note.position].Remove(note);
    }

    private void CheckAccuracy(int index)
    {
        var noteMap = accNotes[IndexToNotePosition(index)];
        Queue<Note> delNotes = new Queue<Note>();
        foreach (var note in noteMap)
        {
            if (!note.Value.IsFailedNote)
            {
                if (note.Value.HoldingTime >= accTiming.Bad)
                {
                    accuracies[Accuracy.Bad]++;
                    note.Value.IsFailedNote = true;
                    note.Value.acc = Accuracy.Bad;
                }
                else if (note.Value.HoldingTime >= accTiming.Good)
                {
                    accuracies[Accuracy.Good]++;
                    note.Value.acc = Accuracy.Good;
                }
                else
                {
                    accuracies[Accuracy.Great]++;
                    note.Value.acc = Accuracy.Great;
                }
                delNotes.Enqueue(note.Value);
                note.Value.enabled = false;
            }
        }

        while(delNotes.Count > 0)
        {
            Note note = delNotes.Dequeue();
            note.DestroyNote();
        }
    }

    private NotePosition IndexToNotePosition(int index)
        => index switch
        {
            -1 => NotePosition.None,
            0 => NotePosition.L1,
            1 => NotePosition.L2,
            2 => NotePosition.R1,
            3 => NotePosition.R2,
        };
}
