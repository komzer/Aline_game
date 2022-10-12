using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Btn_blue : MonoBehaviour
{
    public GameObject[] block;
    public Sprite btnDown;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "boxcoin")
        {
            print("go");
            GetComponent<SpriteRenderer>().sprite = btnDown;
            GetComponent<Collider2D>().enabled = false;
            foreach (GameObject obj in block)
            {
                obj.SetActive(true);
                 }
            }
    }
}
