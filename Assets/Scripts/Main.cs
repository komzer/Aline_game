using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public Player player;
    public Text coinText;
    public Image[] hearts;
    public Sprite isLife, nonLife;
    public GameObject PauseScreen;
    public GameObject WinScreen;
    public GameObject loseScreen;
    float timer=0;
    public Text timeText;
    string currentLevelSave;
    public TimeWork timerWork;
    public float countDown;
    public GameObject inventoryPanel;
    public SoundEffecrtor soundEffector;
    public AudioSource musicSource, soundSource;

    public void Reloadlv()
    {
        Time.timeScale = 1f;
        player.enabled = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Win()
    {
        soundEffector.PlayWinSound();
        inventoryPanel.SetActive(false);
        GetComponent<Inventory>().RecountItems();

        Time.timeScale = 0f;
        player.enabled = false;
        WinScreen.SetActive(true);
        if (!PlayerPrefs.HasKey("Lvl") || PlayerPrefs.GetInt("Lvl") < SceneManager.GetActiveScene().buildIndex)
            PlayerPrefs.SetInt("Lvl", SceneManager.GetActiveScene().buildIndex);
        
        if (PlayerPrefs.HasKey("coins"))
        {
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + player.GetCoins());
        }
        else
        {
            PlayerPrefs.SetInt("coins", player.GetCoins());
        }
        currentLevelSave = SceneManager.GetActiveScene().buildIndex.ToString();
        if (PlayerPrefs.HasKey("bestTime"+ currentLevelSave) )
        {
            if (PlayerPrefs.GetFloat("bestTime" + currentLevelSave) > timer)
            {
                PlayerPrefs.SetFloat("bestTime" + currentLevelSave, timer);
            }
        }
        else
        {
            PlayerPrefs.SetFloat("bestTime" + currentLevelSave, timer);
        }

    }

    public void PauseOn()
    {
        Time.timeScale = 0f;
        player.enabled = false;
        PauseScreen.SetActive(true);
    }
    public void PauseOff()
    {
        Time.timeScale = 1f;
        player.enabled = true;
        PauseScreen.SetActive(false);
    }
        public void Lose()
    {
        soundEffector.PlayLoseSound();
        Time.timeScale = 0f;
        player.enabled = false; 
        loseScreen.SetActive(true);

        inventoryPanel.SetActive(false);
        GetComponent<Inventory>().RecountItems();
    }
    public void MenuLvl()
    {
        Time.timeScale = 1f;
        player.enabled = true;
        SceneManager.LoadScene("menu");
    }
    public void NextLvl()
    {
        Time.timeScale = 1f;
        player.enabled = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Start()
    {
        if ((int)timerWork == 2)
        {
            timer = countDown;
        }    

    }

    public void Update()
    {
        musicSource.volume = (float)PlayerPrefs.GetInt("MusicVolume") / 9;
        soundSource.volume = (float)PlayerPrefs.GetInt("SoundVolume") / 9;

        coinText.text = player.GetCoins().ToString();
        for (int i =0; i < hearts.Length; i++)
        {
            if (player.GetHP() > i)
                hearts[i].sprite = isLife;
            else
                hearts[i].sprite = nonLife;
        }

        if ((int)timerWork ==1)
        {
            timer += Time.deltaTime;
            timeText.text = timer.ToString("F2").Replace(",", ":");
        }else if((int)timerWork == 2)
        {
            timer -= Time.deltaTime;
            timeText.text = timer.ToString("F2").Replace(",", ":");
            if (timer <= 0)
            {
                Lose();
            }
        }
        else
        {
            timeText.gameObject.SetActive(false);
        }
    }
}

public enum TimeWork
{
    None, 
    StopWatch,
    Timer
}
