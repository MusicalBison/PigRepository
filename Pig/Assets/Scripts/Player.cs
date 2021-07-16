﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed = 5f; // Скорость
    public float jumpHeight = 5f; // Высота прыжка
    private float moveInput;
    private bool facingRight = true;
    public float checkRadius = 0.3f;
    public Transform groundCheck; // Позиция GroundCheck
    bool isGrounded; // Наличие земли под ногами у игрока
    public float normalSpeed;
    Animator anim; // Переменная, отвечающая за анимацию
    int maxHp = 3; // Максимальное кол-во жизней
    int curHp; // Текущее кол-во жизней
    public Main main; // Публичная переменная типа Main
    bool isHit = false; // Вспомогательная переменная, которая равна true, когда Тор краснеет
    bool isGo = false;
    private int carrots = 0; // Монеты
    //public bool isFlip = false;

    public bool androidControl = false; // Переменная, отвечающая за тип управления (Клавиши/Джойстик)
    //public Joystick joustick; // Джойстик

    public AudioSource stepSound;
    public AudioSource[] carrotEatSounds;
    public AudioSource fallingInDirt;
    public AudioSource takingOfDirt;
    public AudioSource fallingInLeaves;
    public AudioSource takingOfLeaves;
    public AudioSource pigDamageSound;
    public AllAudio allAudio;

    public bool isDirt = false;
    public bool inDirt = false;
    public GameObject[] dirtArray;
    public bool isLeaves = false;
    public bool inLeaves = false;
    public GameObject[] leavesArray;


    public bool immobility = false;
    public bool upsideDown = false;

    public MyButton right, left;

    void Start() // Этот метод вызывается 1 раз в начале игры
    {
        speed = 0f;
        rb = GetComponent<Rigidbody2D>(); // Связываем переменную rb с нашим компонентом Rigidbody2D
        anim = GetComponent<Animator>(); // Связываем переменную anim с нашим Animator
        curHp = maxHp;

        for (int i = 0; i<dirtArray.Length; i++)
        {
            dirtArray[i].SetActive(false);
        }
        for (int i = 0; i < leavesArray.Length; i++)
        {
            leavesArray[i].SetActive(false);
        }
    }

    void Update() // Этот метод вызывается каждый кадр
    {
        Leaves();
        Dirting();
        Jump();
        CheckGround();
        // Смерть при падении
        if (transform.position.y < -100f)
        {
            Lose();
        }
        if (!immobility)
        {
            // Усанавливаем анимацию бездействия и ходьбы
            /*if (!androidControl)
            {
                if ((Input.GetAxis("Horizontal") == 0 || (right.buttonPressed == false && left.buttonPressed == false)) && isGrounded && !immobility)
                {
                    if (isLeaves) anim.SetInteger("State", 6);
                    else if (isDirt) anim.SetInteger("State", 5);
                    else anim.SetInteger("State", 1);
                    isGo = false;
                }
                else
                {
                    if (!isGo)
                    {
                        StartCoroutine(Steps());
                        isGo = true;
                    }

                    //Flip();
                    if (isGrounded)
                    {
                        if (isLeaves) anim.SetInteger("State", 8);
                        else if (isDirt) anim.SetInteger("State", 7);
                        else anim.SetInteger("State", 2);
                    }
                }
            }
            else
            {
                if (((right.buttonPressed == false && left.buttonPressed == false)) && isGrounded)
                {
                    isGo = false;
                    if (isLeaves) anim.SetInteger("State", 6);
                    else if (isDirt) anim.SetInteger("State", 5);
                    else anim.SetInteger("State", 1);
                }
                else
                {
                    if (!isGo)
                    {
                        StartCoroutine(Steps());
                        isGo = true;
                    }

                    //Flip();
                    if (isGrounded)
                    {
                        if (isLeaves) anim.SetInteger("State", 8);
                        else if (isDirt) anim.SetInteger("State", 7);
                        else anim.SetInteger("State", 2);
                    }
                }
            }*/
        }
    }
    private void FixedUpdate() // Этот метод вызывается определённое число раз в секунду
    {
        if (!immobility)
        {
            if (!androidControl)
            {
                moveInput = Input.GetAxis("Horizontal");
                rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
                if (facingRight == false && moveInput > 0)
                {
                    Flip();
                }
                else if (facingRight == true && moveInput < 0)
                {
                    Flip();
                }
                if (moveInput == 0)
                {
                    anim.SetBool("isRunning", false);
                }
                else
                {
                    anim.SetBool("isRunning", true);
                }
            }
            else
            {
                rb.velocity = new Vector2(speed, rb.velocity.y);
                if (speed != 0)
                {
                    anim.SetBool("isRunning", true);
                }
            }
        }
    }
    public void OnLeftButtonDown()
    {
        if (speed >= 0f && !immobility)
        {
            speed = -normalSpeed;
            transform.eulerAngles = new Vector3(transform.localRotation.x, 180, transform.localRotation.z);
        }
    }
    public void OnRightButtonDown()
    {
        if (speed <= 0f && !immobility)
        {
            speed = normalSpeed;
            transform.eulerAngles = new Vector3(transform.localRotation.x, 0, transform.localRotation.z);
        }
    }
    public void OnButtonUp()
    {
        speed = 0f;
        anim.SetBool("isRunning", false);
    }
    void Flip()
    {
            facingRight = !facingRight;
            Vector3 Scaler = transform.localScale;
            Scaler.x *= -1;
            transform.localScale = Scaler;
    }

    void Jump()
    {
        if (!androidControl && Input.GetKeyDown(KeyCode.Space) && isGrounded && !immobility)
        {
            rb.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse);
            anim.SetTrigger("takeOff");;
        }
    }

    public void OnButtonAndroid()
    {
        if (isGrounded && !immobility)
        {
            rb.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse);
            anim.SetTrigger("takeOff");
        }
    }

    // Метод, проверяющий на земле ли игрок
    void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, checkRadius);
        isGrounded = colliders.Length > 1;
        // Усанавливаем анимацию прыжка
        if (isGrounded)
        {
            anim.SetBool("isJumping", false);
        }
        else
        {
            anim.SetBool("isJumping", true);
        }
        /*if (!isGrounded) // isGrounded == false 
        {
            if (isLeaves) anim.SetInteger("State", 10);
            else if (isDirt) anim.SetInteger("State", 9);
            else anim.SetInteger("State", 4);
        }*/
    }
    // Метод, который пересчитывает текущее кол-во жизней
    public void RecountHp(int deltaHp)
    {
        curHp = curHp + deltaHp;
        //print("Жизни: " + curHp);

        if (deltaHp < 0)
        {
            pigDamageSound.Play();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Carrot") // ... монетой
        {
            Destroy(collision.gameObject);
            carrots++;
            allAudio.carrotsBool = true;
            
        }

        if (collision.tag == "PuddleOfMud")
        {
            inDirt = true;
        }
        if (collision.tag == "Leaves")
        {
            inLeaves = true;
        }
        if (collision.tag == "Barrier")
        {
            if (carrots < 15) main.Barrier();
        }
        if (collision.tag == "Exit")
        {
            main.Win();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "PuddleOfMud")
        {
            inDirt = false;
        }
        if (collision.tag == "Leaves")
        {
            inLeaves = false;
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

    void Dirting()
    {
        if (inDirt)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!upsideDown)
                {
                    fallingInDirt.Play();
                    upsideDown = true;
                    immobility = true;
                    //transform.position = new Vector3(collision.transform.position.x, transform.position.y, transform.position.z); // центр грязи
                    transform.localRotation = Quaternion.Euler(180, transform.localRotation.y, transform.localRotation.z);
                }
                else
                {
                    if (!isDirt) takingOfDirt.Play();
                    upsideDown = false;
                    immobility = false;
                    isDirt = true;
                    transform.position = new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z);
                    transform.localRotation = Quaternion.Euler(0, transform.localRotation.y, transform.localRotation.z);
                }
            }
        }
    }

    void Leaves()
    {
        if (inLeaves)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!upsideDown)
                {
                    fallingInLeaves.Play();
                    upsideDown = true;
                    immobility = true;
                    //transform.position = new Vector3(collision.transform.position.x, transform.position.y, transform.position.z); // центр грязи
                    transform.localRotation = Quaternion.Euler(180, transform.localRotation.y, transform.localRotation.z);
                }
                else
                {
                    if (!isLeaves) takingOfLeaves.Play();
                    upsideDown = false;
                    immobility = false;
                    if (isDirt) isLeaves = true;
                    transform.position = new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z);
                    transform.localRotation = Quaternion.Euler(0, transform.localRotation.y, transform.localRotation.z);
                }
            }
        }
    }

    public void DirtingLeavesAndroid()
    {
        if (inDirt)
        {
            if (!upsideDown)
            {
                fallingInDirt.Play();
                upsideDown = true;
                immobility = true;
                //transform.position = new Vector3(collision.transform.position.x, transform.position.y, transform.position.z); // центр грязи
                transform.localRotation = Quaternion.Euler(180, transform.localRotation.y, transform.localRotation.z);
            }
            else
            {
                if (!isDirt)
                {
                    takingOfDirt.Play();
                    // Если грязи не было, то она  появляется визуально
                    for (int i = 0; i < dirtArray.Length; i++)
                    {
                        dirtArray[i].SetActive(true);
                    }
                }

                upsideDown = false;
                immobility = false;
                isDirt = true;
                transform.position = new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z);
                transform.localRotation = Quaternion.Euler(0, transform.localRotation.y, transform.localRotation.z);
            }
        }

        if (inLeaves)
        {
            if (!upsideDown)
            {
                fallingInLeaves.Play();
                upsideDown = true;
                immobility = true;
                //transform.position = new Vector3(collision.transform.position.x, transform.position.y, transform.position.z); // центр грязи
                transform.localRotation = Quaternion.Euler(180, transform.localRotation.y, transform.localRotation.z);
            }
            else
            {
                if (!isLeaves)
                {
                    takingOfLeaves.Play();
                }
                        
                upsideDown = false;
                immobility = false;
                if (isDirt)
                {
                    isLeaves = true;
                    // Если листьев не было, то они появляются визуально
                    for (int i = 0; i < leavesArray.Length; i++)
                    {
                        leavesArray[i].SetActive(true);
                    }
                }
                transform.position = new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z);
                transform.localRotation = Quaternion.Euler(0, transform.localRotation.y, transform.localRotation.z);
            }
        
        }
    }    
}
