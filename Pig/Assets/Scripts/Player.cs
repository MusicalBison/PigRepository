﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed = 5f; // Скорость
    public float jumpHeight = 5f; // Высота прыжка
    public Transform groundCheck; // Позиция GroundCheck
    bool isGrounded; // Наличие земли под ногами у игрока
    Animator anim; // Переменная, отвечающая за анимацию
    int maxHp = 3; // Максимальное кол-во жизней
    int curHp; // Текущее кол-во жизней
    public Main main; // Публичная переменная типа Main
    bool isHit = false; // Вспомогательная переменная, которая равна true, когда Тор краснеет
    bool isGo = false;
    private int carrots = 0; // Монеты
    //public bool isFlip = false;

    public bool androidControl = false; // Переменная, отвечающая за тип управления (Клавиши/Джойстик)
    public Joystick joustick; // Джойстик

    public AudioSource stepSound;
    public AudioSource[] carrotEatSounds;

    void Start() // Этот метод вызывается 1 раз в начале игры
    {
        rb = GetComponent<Rigidbody2D>(); // Связываем переменную rb с нашим компонентом Rigidbody2D
        anim = GetComponent<Animator>(); // Связываем переменную anim с нашим Animator
        curHp = maxHp;
    }

    void Update() // Этот метод вызывается каждый кадр
    {
        Jump();
        CheckGround();
        // Усанавливаем анимацию бездействия и ходьбы
        if (!androidControl)
        {
            if (Input.GetAxis("Horizontal") == 0 && isGrounded)
            {
                isGo = false;
                anim.SetInteger("State", 1);
            }
            else
            {
                if (!isGo)
                {
                    StartCoroutine(Steps());
                    isGo = true;
                }

                Flip();
                if (isGrounded)
                {
                    anim.SetInteger("State", 2);
                }
            }

            // Смерть при падении
            if (transform.position.y < -35f)
            {
                Lose();
            }
        }
        else
        {
            if (joustick.Horizontal <= 0.3f && joustick.Horizontal >= -0.3f && isGrounded)
            {
                isGo = false;
                anim.SetInteger("State", 1);
            }
            else
            {
                if (!isGo)
                {
                    StartCoroutine(Steps());
                    isGo = true;
                }

                Flip();
                if (isGrounded)
                {
                    anim.SetInteger("State", 2);
                }
            }

            // Смерть при падении
            if (transform.position.y < -35f)
            {
                Lose();
            }
        }
        
    }

    void FixedUpdate() // Этот метод вызывается определённое число раз в секунду
    {
        if (!androidControl)
            rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y);
        else
        {
            if (joustick.Horizontal >= 0.3f)
                rb.velocity = new Vector2(speed, rb.velocity.y);
            else if (joustick.Horizontal <= -0.3f)
                rb.velocity = new Vector2(-speed, rb.velocity.y);
            else
                rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    // Метод, отвечающий за поворот
    void Flip()
    {
        //if (isFlip)
        //isFlip = true;
        if (!androidControl)
        {
            if (Input.GetAxis("Horizontal") > 0)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }

            if (Input.GetAxis("Horizontal") < 0)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
        }
        else // Если управляем джойстиком
        {
            if (joustick.Horizontal >= 0.3f) // Поворот направо
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            if (joustick.Horizontal <= -0.3f) // Поворот налево
                transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        
    }

    // Метод, отвечающий за прыжок
    void Jump()
    {
        if (!androidControl && Input.GetKeyDown(KeyCode.Space) && isGrounded)
            rb.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse);
    }

    // Прыжок, который вызывается при нажатии на кнопку на экране
    public void JumpAndroid()
    {
        if (isGrounded)
        {
            rb.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse);
        }
    }

    // Метод, проверяющий на земле ли игрок
    void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.2f);
        isGrounded = colliders.Length > 1;
        // Усанавливаем анимацию прыжка
        if (!isGrounded) // isGrounded == false
        {
            anim.SetInteger("State", 3);
        }
    }

    // Метод, который пересчитывает текущее кол-во жизней
    public void RecountHp(int deltaHp)
    {
        curHp = curHp + deltaHp;
        print("Жизни: " + curHp);

        if (deltaHp < 0)
        {
            //StopCoroutine(OnHit());
            isHit = true;
            StartCoroutine(OnHit()); // Запуск корутины OnHit
        }

        if (curHp <= 0)
        {
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false; // Падение Тора
            // Вызываем метод Lose с задержкой 1.5 секунды
            Invoke("Lose", 1.5f);
        }
    }

    // Метод, который вызывает перезагрузку текущей сцены
    void Lose()
    {
        main.GetComponent<Main>().Lose();
    }

    // Корутина, плавно изменяющая цвет Тора во время получения урона
    IEnumerator OnHit()
    {
        // Покраснение и возврат к исходному цвету
        if (isHit == true)
        {
            GetComponent<SpriteRenderer>().color = new Color(1f, GetComponent<SpriteRenderer>().color.g - 0.04f, GetComponent<SpriteRenderer>().color.b - 0.04f);
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(1f, GetComponent<SpriteRenderer>().color.g + 0.04f, GetComponent<SpriteRenderer>().color.b + 0.04f);
        }

        // Проверка полного покраснения
        if (GetComponent<SpriteRenderer>().color.g <= 0) isHit = false;

        yield return new WaitForSeconds(0.02f); // Ожидание 0.02 секунды

        if (GetComponent<SpriteRenderer>().color.g != 1)
            StartCoroutine(OnHit()); // Вызов текущей корутины
    }

    // Метод, который вызывается, когда наш герой соприкасается с...
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Carrot") // ... монетой
        {
            Destroy(collision.gameObject);
            carrots++;
            int a = Random.Range(0, 2);
            if (a == 0) carrotEatSounds[0].Play();
            else carrotEatSounds[1].Play();

        }
    }

    IEnumerator Steps()
    {
        if (isGrounded && Input.GetAxis("Horizontal") != 0) stepSound.Play();
        yield return new WaitForSeconds(speed/20);
        if (isGo) StartCoroutine(Steps()); // Если идёт, но не повернулся //  && !isFlip
        //if (isFlip) isFlip = false;
    }

    // Охранник, который говорит нам сколько у игрока морковок
    public int GetCarrots()
    {
        return carrots;
    }

    // Охранник, который говорит нам сколько у игрока жизней
    public int GetHearts()
    {
        return curHp;
    }
}
