using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beetle : MonoBehaviour
{
    public float speed = 4f;
    bool isWait = false;
    bool isHidden = false;
    public float waitTime = 4f;
    public Transform point;
    public bool verticalMove = true;
    public bool moveLeftRight = false;
    float stepUp = 1f;
    // Start is called before the first frame update
    void Start()
    {
        if (moveLeftRight)
            stepUp = -1f;
        else
            stepUp = 1f;
        point.transform.position = new Vector3(transform.position.x, transform.position.y , transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if ( isWait == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, point.position, speed * Time.deltaTime);
        }
        if (transform.position == point.position)
        {
            if (verticalMove)
            {
                if (isHidden)
                {
                    point.transform.position = new Vector3(transform.position.x , transform.position.y + stepUp, transform.position.z);
                    isHidden = false;
                }
                else
                {
                    point.transform.position = new Vector3(transform.position.x , transform.position.y - stepUp, transform.position.z);
                    isHidden = true;
                }
            }
            else
            {
                if (isHidden)
                {
                    point.transform.position = new Vector3(transform.position.x + stepUp, transform.position.y , transform.position.z);
                    isHidden = false;
                }
                else
                {
                    point.transform.position = new Vector3(transform.position.x - stepUp, transform.position.y , transform.position.z);
                    isHidden = true;
                }
            }
            isWait = true;
            StartCoroutine(Waiting());
        }
        IEnumerator Waiting()
        {
            yield return new WaitForSeconds(waitTime);
            isWait = false;
        }
    }
}
