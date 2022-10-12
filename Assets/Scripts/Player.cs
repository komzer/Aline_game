using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed;
    public float jumpHeight;
    public Transform GroundCheck;
    bool isGrounded;
    Animator anim;
    int curHp;
    int maxHp = 3;
    bool isHit = false;
    public Main main;
    public bool key = false;
    bool canTp = true;
    public bool inWater = false;
    bool isClimbing = false;
    int coin = 0;
    bool canHit = true;
    public GameObject blueGem, greenGem;
    int gameCount = 0;
    float hitTimer = 0f;
    public Image PlayerCountdown;
    float insideTimer = -1f;
    public float insideTimerUp = 5f;
    public Image insideCountDown;
    public Inventory inventory;
    public SoundEffecrtor soundEffector;
    public Joystick joystick;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( collision.gameObject.tag == "key")
        {
            Destroy(collision.gameObject);
            key = true;
            inventory.Add_key();
        }
        if (collision.gameObject.tag == "door")
        {
            if (collision.gameObject.GetComponent<Door>().isOpen && canTp)
            {
                collision.gameObject.GetComponent<Door>().Teleport(gameObject);
                canTp = false;
                StartCoroutine(Tpwait());
            }
            else if (key)
            {
                collision.gameObject.GetComponent<Door>().Unlock();
            }
        }
        if (collision.gameObject.tag == "coin")
        {
            Destroy(collision.gameObject);
            coin++;
            soundEffector.PlayCoinSound();
        }
        if (collision.gameObject.tag == "heart")
        {
            Destroy(collision.gameObject);
            //RecountHp(1);
            inventory.Add_hp();
        }
        if (collision.gameObject.tag == "muchomor")
        {
            Destroy(collision.gameObject);
            RecountHp(-1);
        }
        if (collision.gameObject.tag == "blueGem")
        {
            Destroy(collision.gameObject);
            // StartCoroutine(NoHit());
            inventory.Add_blueGem();
        }
        if (collision.gameObject.tag == "greenGem")
        {
            Destroy(collision.gameObject);
            //StartCoroutine(SpeedBonus());
            inventory.Add_greenGem();
        }
        if ( collision.gameObject.tag == "TimerButtonStart")
        {
            insideTimer = 0f;
        }
        if (collision.gameObject.tag == "TimerButtonStop")
        {
            insideTimer = -1f;
            insideCountDown.fillAmount = 0f;
        }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        curHp = maxHp;
    }

    void Update()
    {
        if (inWater && !isClimbing)
        {
            anim.SetInteger("State", 4);
            isGrounded = true;
            if (joystick.Horizontal>=0.2f || joystick.Horizontal <= -0.2f )
            {
                Flip();
            }
        }
        else
        {
            if (joystick.Horizontal < 0.2f && joystick.Horizontal > -0.2f && (isGrounded) && !isClimbing)
            {
                anim.SetInteger("State", 1);
            }
            else
            {
                Flip();
                if (isGrounded && !isClimbing)
                {
                    anim.SetInteger("State", 2);
                }
            }
        }
        Checkground();
     

        if (insideTimer >= 0f)
        {
            insideTimer += Time.deltaTime;
            if( insideTimer >= insideTimerUp)
            {
                insideTimer = 0f;
                RecountHp(-1);
            }else
            {
                insideCountDown.fillAmount = 1 - (insideTimer / insideTimerUp);
            }
        }
        
    }
    public void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse);
            soundEffector.PlayJumpSound();
        }
    }
    void FixedUpdate()
    {
        //rb.velocity = new Vector2(Input.GetAxis("Horizontal")*speed, rb.velocity.y);
        rb.velocity = new Vector2(joystick.Horizontal * speed, rb.velocity.y);
    }
    void Flip()
    {
        if (joystick.Horizontal >= 0.3f  )
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        if ( joystick.Horizontal <= -0.3f )
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }
    void Checkground()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(GroundCheck.position, 0.2f);
        isGrounded = colliders.Length > 1;
        if (!isGrounded && !isClimbing)
        {
            anim.SetInteger("State", 3);
        }
    }
    public void RecountHp(int deltaHp)
    {
        if ( deltaHp < 0 && canHit)
        {
            curHp = curHp + deltaHp;
            StopAllCoroutines();
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
            isHit = true;   
            StartCoroutine(OnHit());
        }
        if (deltaHp > 0)
        {
            if (curHp < maxHp)
            {
                curHp = curHp + deltaHp;
            }
        }
        if (curHp <= 0)
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
            Invoke("Lose", 2f);
        }
       
    }
    IEnumerator OnHit()
    {

        if (isHit)
        {
            GetComponent<SpriteRenderer>().color = new Color(1f, GetComponent<SpriteRenderer>().color.g - 0.08f, GetComponent<SpriteRenderer>().color.b - 0.08f);
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(1f, GetComponent<SpriteRenderer>().color.g + 0.08f, GetComponent<SpriteRenderer>().color.b + 0.08f);
        }

        if ( (GetComponent<SpriteRenderer>().color.g == 1f) && !isHit)
        {
            StopCoroutine(OnHit());
        }
        if (GetComponent<SpriteRenderer>().color.g <= 0)
        {
            isHit = false;
        }


            yield return new WaitForSeconds(0.02f);
           StartCoroutine(OnHit());
    }
    void Lose()
    {
        main.GetComponent<Main>().Lose();
    }
    IEnumerator Tpwait()
    {
        yield return new WaitForSeconds(1f);
        canTp = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "ledder")
        {
            isClimbing = true;
            rb.bodyType = RigidbodyType2D.Kinematic;
            if (joystick.Vertical < 0.3f && joystick.Vertical > -0.3f)
            {
                anim.SetInteger("State", 5);
            }
            else
            {
                anim.SetInteger("State", 6);
                transform.Translate(Vector3.up * joystick.Vertical * speed * 0.5f * Time.deltaTime);
            }

        }
        if (collision.gameObject.tag == "icy")
        { if (rb.gravityScale == 1f)
            {
                rb.gravityScale = 7f;
                speed *= 0.25f;
            }
        }
        if ( collision.gameObject.tag == "lava")
        {
            hitTimer += Time.deltaTime;
            if (hitTimer >= 3f)
            {
                hitTimer = 0f;
                PlayerCountdown.fillAmount = 1f;
                RecountHp(-1);
            }
            else
            {
                PlayerCountdown.fillAmount = 1 - (hitTimer / 3f);
            }
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "ledder")
        {
            isClimbing = false;
            rb.bodyType = RigidbodyType2D.Dynamic;
            
            
        }
        if (collision.gameObject.tag == "icy")
        {
            if (rb.gravityScale == 7f)
            {
                rb.gravityScale = 1f;
                speed *= 4f;
            }
        }
        if (collision.gameObject.tag == "lava")
        {
            hitTimer = 0f;
            PlayerCountdown.fillAmount = 0f;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
         if( collision.gameObject.tag == "trampoline")
        {
            StartCoroutine(TrampolineAnim(collision.gameObject.GetComponentInParent<Animator>()));
        }
         if (collision.gameObject.tag == "quicksand")
        {
            speed *= 0.25f;
            rb.mass = 10f;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if ( collision.gameObject.tag == "quicksand")
        {
            speed *= 4f;
            rb.mass *= 0.1f;
        }
    }

    void CheckGames(GameObject obj)
    {
        if (gameCount == 1)
        {
            obj.transform.localPosition = new Vector3(0f, 0.6f, obj.transform.localPosition.z);

        }else if(gameCount == 2)
        {
            blueGem.transform.localPosition = new Vector3(-0.5f, 0.5f, obj.transform.localPosition.z);
            greenGem.transform.localPosition = new Vector3(0.5f, 0.5f, obj.transform.localPosition.z);
        }
    }
    
    public int GetCoins()
    {
        return coin;
    }
    public int GetHP()
    {
        return curHp;
    }
    public void BlueGem()
    {
        StartCoroutine(NoHit());
    }
    public void GreenGem()
    {
        StartCoroutine(SpeedBonus());
    }
    IEnumerator Invis(SpriteRenderer spr, float time)
    {
        spr.color = new Color(1f, 1f, 1f, spr.color.a - time * 2);
        yield return new WaitForSeconds(time);
        if (spr.color.a > 0)
        {
            StartCoroutine(Invis(spr, time));
        }
    }

    IEnumerator TrampolineAnim(Animator an)
    {
        an.SetBool("isJump", true);
        yield return new WaitForSeconds(0.5f);
        an.SetBool("isJump", false);

    }
    IEnumerator NoHit()
    {
        gameCount++;
        blueGem.SetActive(true);
        CheckGames(blueGem);

        canHit = false;
        blueGem.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        yield return new WaitForSeconds(4f);
        StartCoroutine(Invis(blueGem.GetComponent<SpriteRenderer>(), 0.02f));
        yield return new WaitForSeconds(1f);
        canHit = true;

        gameCount--;
        blueGem.SetActive(false);
        CheckGames(greenGem);
    }

    IEnumerator SpeedBonus()
    {
        gameCount++;
        greenGem.SetActive(true);
        CheckGames(greenGem);


        speed = speed * 2;
        greenGem.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        yield return new WaitForSeconds(4f);
        StartCoroutine(Invis(greenGem.GetComponent<SpriteRenderer>(), 0.02f));
        yield return new WaitForSeconds(1f);
        speed = speed / 2;

        gameCount--;
        greenGem.SetActive(false);
        CheckGames(blueGem);
    }
}
