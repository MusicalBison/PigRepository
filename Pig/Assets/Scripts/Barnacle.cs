using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barnacle : MonoBehaviour
{
    public float speed = 4f; // Скорость жука
    bool isWait = false; // Ожидание
    bool isHidden = true; // Скрытие
    public float waitTime = 3f; // Время ожидания
    public Transform point; // Позиция, куда двигается жук

    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.85f, transform.position.z);
        point.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
    }

    void Update()
    {
        if (isWait == false)
            transform.position = Vector3.MoveTowards(transform.position, point.transform.position, speed * Time.deltaTime);
        if (transform.position == point.position)
        {
            if (isHidden)
            {
                point.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
                isHidden = false;
            }
            else
            {
                point.transform.position = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
                isHidden = true;
            }

            isWait = true;
            StartCoroutine(Waiting());
        }
    }

    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(waitTime);
        isWait = false;
    }
}
