using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toched : MonoBehaviour
{
    Rigidbody2D rb;
        private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = UnityEngine.Camera.main.ScreenToWorldPoint(touch.position);
            touchPos.z = 0f;
            if (touchPos.x > UnityEngine.Camera.main.transform.position.x)
                rb.velocity = new Vector2(8, rb.position.y);
            else
                rb.velocity = new Vector2(-8, rb.position.y);
        }
    }
}
