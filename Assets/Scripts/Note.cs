using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Note : MonoBehaviour
{
    public float speed = 400;
    private void Update()
    {
        var nextPos = Vector3.back * Time.deltaTime * speed;
        transform.localPosition += nextPos;
    }
}
