using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG : MonoBehaviour
{
    float length, startPos;
    public Transform cam;
    public float paralaxEffect;
    void Start()
    {
        startPos = cam.transform.position.x; //стартовые позиции
        length = GetComponent<SpriteRenderer>().bounds.size.x;

    }


    void FixedUpdate()
    {
        float dist = cam.transform.position.x * paralaxEffect;
        float temp = cam.transform.position.x * (1 - paralaxEffect);
        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);
        if (temp > startPos + length) startPos += length;
        if (temp < startPos - length) startPos -= length;
    }
}
