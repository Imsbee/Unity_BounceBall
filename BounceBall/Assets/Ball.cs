using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    Rigidbody2D rb;
    AudioSource source;

    public float autoJumpPower;
    public float moveSpeed;
    public float shootSpeed;
    public float jumpBoostPower;
    public float fallGravity;
    bool isShooting;
    string blockTag;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        blockTag = "";
        source = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        if (!isShooting)
        {
            rb.velocity = new Vector2(moveSpeed * Input.GetAxis("Horizontal"), rb.velocity.y);
        }
        else
        {
            
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
            {
                ShootingOff();
            }
        }
    }

    void ShootingOn(Vector2 dir, Vector2 pos)
    {
        isShooting = true;
        rb.gravityScale = 0;
        transform.position = pos + dir;
        rb.velocity = dir * shootSpeed;
    }

    void ShootingOff()
    {
        isShooting = false;
        rb.gravityScale = 5;
    }

    void Die()
    {
        Mn.mn.BallDie();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        ShootingOff();
        if (col.gameObject.CompareTag("Dang")) Die();

        Vector2 dir = col.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (angle <= -45 && angle >= -135)
        {
            switch (col.collider.tag)
            {
                case "Ground":
                    rb.velocity = new Vector2(rb.velocity.x, autoJumpPower);
                    source.Play();
                    break;
                case "RightShoot":
                    ShootingOn(new Vector2(col.transform.localScale.x, 0), col.transform.position);
                    break;
                case "LeftShoot":
                    ShootingOn(new Vector2(col.transform.localScale.x * -1, 0), col.transform.position);
                    break;
                case "Broken":
                    rb.velocity = new Vector2(rb.velocity.x, autoJumpPower);
                    Mn.mn.DisableObj(col.gameObject);
                    break;
                case "JumpBoost":
                    rb.velocity = new Vector2(rb.velocity.x, jumpBoostPower);
                    break;
                case "Fall":
                    rb.velocity = new Vector2(rb.velocity.x, autoJumpPower);
                    col.collider.GetComponent<Rigidbody2D>().isKinematic = false;
                    col.collider.GetComponent<Rigidbody2D>().gravityScale = fallGravity;
                    break;
                default:
                    break;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Star"))
        {
            Mn.mn.GetStar();
            Mn.mn.DisableObj(col.transform.parent.gameObject);
        }
    }
}
