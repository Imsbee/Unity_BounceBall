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
        rb.gravityScale = 3;
    }

    void Die()
    {
        Mn.mn.BallDie();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        ShootingOff();
        if (col.gameObject.CompareTag("Dang")) Die(); // Ball과 충돌한 오브젝트가 Dang(외곽벽)이면 Die함수 호출

        Vector2 dir = col.transform.position - transform.position; // Ball과 충돌한 오브젝트까지의 방향 벡터 계산
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg; // 방향 벡터와 x축 사이의 각도를 계산해서 '도' 단위로 변환
        if (angle <= -45 && angle >= -135) // 각도가 -45 ~ -135도 인경우 실행
        {
            switch (col.collider.tag)
            {
                case "Ground": 
                    rb.velocity = new Vector2(rb.velocity.x, autoJumpPower); // 땅에 닿으면 y값을 수정해서 자동점프
                    source.Play(); // 땅에 부딪히면 효과음 발생
                    break;
                case "RightShoot": 
                    ShootingOn(new Vector2(col.transform.localScale.x, 0), col.transform.position);
                    // ShootingOn은 벡터2 타입만 받고 있으므로 x값을 받아오기 위해 new Vector2사용
                    source.Play();
                    break;
                case "LeftShoot":
                    ShootingOn(new Vector2(col.transform.localScale.x * -1, 0), col.transform.position);
                    // 오른쪽 슈터의 양의 값이니 왼쪽 슈터는 -1을 곱해서 반대 방향으로 움직임
                    source.Play();
                    break;
                case "Broken":
                    rb.velocity = new Vector2(rb.velocity.x, autoJumpPower);
                    Mn.mn.DisableObj(col.gameObject);
                    // Mn클래스의 함수를 이용해서 Broken 오브젝트 제거
                    source.Play();
                    break;
                case "JumpBoost":
                    rb.velocity = new Vector2(rb.velocity.x, jumpBoostPower);
                    // x값은 유지하고 y값만 변경
                    source.Play();
                    break;
                case "Fall":
                    rb.velocity = new Vector2(rb.velocity.x, autoJumpPower);
                    // Fall 오브젝트의 물리력(?)을 활성화 하고 중력을 적용 시킴
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
