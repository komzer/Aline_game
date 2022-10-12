using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
     int hp=0, blueGem=0, greenGem = 0;
    public Sprite[] numbers;
    public Sprite is_hp, no_hp, is_blueGem, no_blueGem, is_greenGem, no_greenGem, is_key, no_key;
    public Image hp_img, blueGem_img, greenGem_img, key_img;
    public Player player;

    private void Start()
    {
        if (PlayerPrefs.GetInt("hp") > 0)
        {
            hp = PlayerPrefs.GetInt("hp");
            hp_img.sprite = is_hp;
            hp_img.transform.GetChild(0).GetComponent<Image>().sprite = numbers[hp];
        }
        if (PlayerPrefs.GetInt("blueGem") > 0)
        {
            blueGem = PlayerPrefs.GetInt("blueGem");
            blueGem_img.sprite = is_blueGem ;
            blueGem_img.transform.GetChild(0).GetComponent<Image>().sprite = numbers[blueGem];
        }
        if (PlayerPrefs.GetInt("greenGem") > 0)
        {
            greenGem = PlayerPrefs.GetInt("greenGem");
            greenGem_img.sprite = is_greenGem;
            greenGem_img.transform.GetChild(0).GetComponent<Image>().sprite = numbers[greenGem];
        }
    }
    public void Add_hp()
    {
        hp++;
        hp_img.sprite = is_hp;
        hp_img.transform.GetChild(0).GetComponent<Image>().sprite = numbers[hp];
    }

    public void Add_blueGem()
    {
        blueGem++;
        blueGem_img.sprite = is_blueGem;
        blueGem_img.transform.GetChild(0).GetComponent<Image>().sprite = numbers[blueGem];
    }
    public void Add_greenGem()
    {
        greenGem++;
        greenGem_img.sprite = is_greenGem;
        greenGem_img.transform.GetChild(0).GetComponent<Image>().sprite = numbers[greenGem];
    }
    public void Add_key()
    {
        key_img.sprite = is_key;
    }

    public void Use_hp()
    {
        if (hp > 0 && player.GetHP()<3)
        {
            hp--;
            player.RecountHp(1);
            hp_img.transform.GetChild(0).GetComponent<Image>().sprite = numbers[hp];
            if (hp == 0)
                hp_img.sprite = no_hp;
        }
    }
    public void Use_BlueGem()
    {
        if (blueGem > 0)
        {
            blueGem--;
            player.BlueGem();
            blueGem_img.transform.GetChild(0).GetComponent<Image>().sprite = numbers[blueGem];
            if (blueGem == 0)
                blueGem_img.sprite = no_blueGem;
        }
    }
    public void Use_GreenGem()
    {
            if (greenGem > 0)
            {
                greenGem--; 
                player.GreenGem();
                greenGem_img.transform.GetChild(0).GetComponent<Image>().sprite = numbers[greenGem];
                if (greenGem == 0)
                    greenGem_img.sprite = no_greenGem;
            }
    }

    public void RecountItems()
    {
        PlayerPrefs.SetInt("hp", hp);
        PlayerPrefs.SetInt("blueGem", blueGem);
        PlayerPrefs.SetInt("greenGem", greenGem);
    } 
 
}
 