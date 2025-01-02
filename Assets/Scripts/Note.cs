using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Note : MonoBehaviour
{
    public float speed = 1;
    private void Start()
    {
        
    }
    private void Update()
    {
        var currPos = transform.position;
        float nextPos = Time.deltaTime * speed;
        transform.position = new Vector3(currPos.x, currPos.y, currPos.z + nextPos);
    }
}
