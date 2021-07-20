using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Refrigerator : MonoBehaviour
{
    public GameObject door;
    public Sprite refrigeratorOpen, refrigeratorClose;
    public bool isActive = false;

    public void OpenCloseRefrigerator()
    {
        if (!isActive)
        {
            GetComponent<SpriteRenderer>().sprite = refrigeratorOpen;
            door.SetActive(true);
            GetComponent<SpriteRenderer>().sortingOrder = 0;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = refrigeratorClose;
            door.SetActive(false);
            GetComponent<SpriteRenderer>().sortingOrder = 6;
        }
        isActive = !isActive;
    }
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
