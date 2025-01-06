using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour
{
    public Animator lightHit = null;

    public void PlayEffect()
    {
        lightHit.SetTrigger("Light");
    }
}
