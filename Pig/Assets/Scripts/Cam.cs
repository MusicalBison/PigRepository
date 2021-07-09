using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    float speed = 4f; // Скорость камеры
    public Transform target; // Переменная, отвечающая за позицию Тора

    void Start()
    {
        // Присваиваем позиции камеры позицию Тора
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
    }

    void Update()
    {
        Vector3 position = target.position; // Вспомогательная позиция
        position.z = transform.position.z;
        // Плавно перемещаем камеру к Тору
        transform.position = Vector3.Lerp(transform.position, position, speed * Time.deltaTime);
    }
}
