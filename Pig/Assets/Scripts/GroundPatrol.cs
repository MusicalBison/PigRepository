using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPatrol : MonoBehaviour
{
    public float speed = 1.5f;
    public Transform groundDetect; // Переменная, содержащая в себе компонент Transform объекта GroundDetect
    bool moveLeft = true; // Движение влево = true

    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetect.position, Vector2.down, 1f);

        if (groundInfo.collider == false) // Если под лазером нет платформы
        {
            if (moveLeft) // Если слизень двигался влево
            {
                transform.eulerAngles = new Vector3(0, 180, 0); // Разворачиваем слизня вправо
                moveLeft = false;
            }
            else // Иначе
            {
                transform.eulerAngles = new Vector3(0, 0, 0); // Разворачиваем влево
                moveLeft = true;
            }
        }
    }
}
