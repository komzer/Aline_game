using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Menu : MonoBehaviour
{
    public Button[] lvls;
    public Text coinText;
    public Text BestTime1;
    public Text BestTime2;
    public Text BestTime3;
    public Slider musicSlider, soundSlider;
    public Text musicText, soundText;
    public AudioSource musicSource, soundSource;


    void Start()
    {

      

        if (PlayerPrefs.HasKey("Lvl"))
        {
            for(int i=0; i < lvls.Length; i++)
            {
                if (i <= PlayerPrefs.GetInt("Lvl"))
                    lvls[i].interactable = true;
                else
                    lvls[i].interactable = false;
            }
        }
        if (PlayerPrefs.HasKey("bestTime1"))
        {
            BestTime1.text = PlayerPrefs.GetFloat("bestTime1").ToString("F2");
        }
        else
        {
            BestTime1.text = "0:00";
        }
        if (PlayerPrefs.HasKey("bestTime2"))
        {
            BestTime2.text = PlayerPrefs.GetFloat("bestTime2").ToString("F2");
        }
        else
        {
            BestTime2.text = "0:00";
        }

        if (!PlayerPrefs.HasKey("hp"))
            PlayerPrefs.SetInt("hp",0);
        if (!PlayerPrefs.HasKey("blueGem"))
            PlayerPrefs.SetInt("blueGem", 0);
        if (!PlayerPrefs.HasKey("greenGem"))
            PlayerPrefs.SetInt("greenGem", 0);

        if (!PlayerPrefs.HasKey("MusicVolume"))
            PlayerPrefs.SetInt("MusicVolume", 3);
        if (!PlayerPrefs.HasKey("SoundVolume"))
            PlayerPrefs.SetInt("SoundVolume", 8);
        musicSlider.value = PlayerPrefs.GetInt("MusicVolume");
        soundSlider.value = PlayerPrefs.GetInt("SoundVolume");

    }

    void Update()
    {
        PlayerPrefs.SetInt("MusicVolume", (int)musicSlider.value);
        PlayerPrefs.SetInt("SoundVolume", (int)soundSlider.value);
        musicText.text = musicSlider.value.ToString();
        soundText.text = soundSlider.value.ToString();

        musicSource.volume = (float)PlayerPrefs.GetInt("MusicVolume") / 9;
        soundSource.volume = (float)PlayerPrefs.GetInt("SoundVolume") / 9;

        if (PlayerPrefs.HasKey("coins"))
        {
            coinText.text = PlayerPrefs.GetInt("coins").ToString();
        }
        else
        {
            coinText.text = "0";
        }

    
    }

    public void DelKeys()
    {
        PlayerPrefs.DeleteAll();
    }


   public void OpenScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void Buy_hp(int cost)
    {
        if (PlayerPrefs.GetInt("coins") >= cost)
        {
            PlayerPrefs.SetInt("hp", PlayerPrefs.GetInt("hp")+1);
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") - cost );
        }
    }
    public void Buy_blueGet(int cost)
    {
        if (PlayerPrefs.GetInt("coins") >= cost)
        {
            PlayerPrefs.SetInt("blueGem", PlayerPrefs.GetInt("blueGem") + 1);
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") - cost);
        }
    }
    public void Buy_greenGet(int cost)
    {
        if (PlayerPrefs.GetInt("coins") >= cost)
        {
            PlayerPrefs.SetInt("greenGem", PlayerPrefs.GetInt("greenGem") + 1);
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") - cost);
        }
    }
}
 