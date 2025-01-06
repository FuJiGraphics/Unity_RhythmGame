using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenEffect : MonoBehaviour
{
    public Animator noteHit = null;
    private string Hit = "Hit";
    public Animator judgeAnimator = null;
    public UnityEngine.UI.Image judgeImage = null;
    public Sprite[] judgeSprites = null;
    public Animator LightAnimator = null;

    public void JudgeEffect(int num)
    {
        judgeImage.sprite = judgeSprites[num];
        judgeAnimator.SetTrigger(Hit);
    }

    public void PlayEffect()
    {
        noteHit.SetTrigger(Hit);
    }

    public void LightEffect(bool enabled)
    {
        LightAnimator.SetBool(Hit, enabled);
    }
}
