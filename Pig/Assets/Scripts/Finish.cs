using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    public bool pinguinAtHome = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Telega")
        {
            collision.gameObject.SetActive(false);
            pinguinAtHome = true;
        }
    }
}
