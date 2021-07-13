using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public Text carrotText;
    public Player player;
    public Image[] hearts; // Массив сердечек
    public Sprite is_hp, no_hp;

    public GameObject winPanel, losePanel, losePanel2, enterPanel, barrierPanel;

    public GameObject exit;

    void Start()
    {
        Time.timeScale = 0;
        player.enabled = false;
    }

    // Метод, при вызове которого текущая сцена перезагружается
    public void Lose()
    {
        losePanel.SetActive(true);
        Time.timeScale = 0;
        player.enabled = false;
    }

    public void Win()
    {
        if (!player.isLeaves) losePanel2.SetActive(true);
        else winPanel.SetActive(true);
        Time.timeScale = 0;
        player.enabled = false;
    }

    void Update()
    {
        carrotText.text = player.GetCarrots().ToString();

        if (player.GetHearts() > 0)
        {
            hearts[0].sprite = is_hp;
            if (player.GetHearts() > 1)
            {
                hearts[1].sprite = is_hp;
                if (player.GetHearts() > 2)
                {
                    hearts[2].sprite = is_hp;
                }
                else hearts[2].sprite = no_hp;
            }
            else hearts[1].sprite = no_hp;
        }
        else hearts[0].sprite = no_hp;
    }

    public void OpenScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Enter()
    {
        Time.timeScale = 1;
        player.enabled = true;
        enterPanel.SetActive(false);
    }

    public void Barrier()
    {
        Time.timeScale = 0;
        player.enabled = false;
        barrierPanel.SetActive(true);
    }

    public void ToExit()
    {
        player.transform.position = new Vector3(exit.transform.position.x, exit.transform.position.y, player.transform.position.z);
        Time.timeScale = 1;
        player.enabled = true;
        barrierPanel.SetActive(false);
    }    
}
