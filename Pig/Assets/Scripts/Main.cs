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
    
    // Метод, при вызове которого текущая сцена перезагружается
    public void Lose()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
}
