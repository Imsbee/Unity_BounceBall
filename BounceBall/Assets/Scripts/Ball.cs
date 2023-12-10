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
    bool isShooting;
    string blockTag;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        source = GetComponent<AudioSource>();
    }

    void Start()
    {
        blockTag = "";
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
        rb.gravityScale = 0; // 공이 밑으로 추락 하면 안 되기 때문에 중력 값을 0으로 설정
        transform.position = pos + dir; // 공의 위치를 충돌한 오브젝트의 바로 옆으로 설정
        rb.velocity = dir * shootSpeed; // 공의 속도를 올려서 발사
    }

    void ShootingOff()
    {
        isShooting = false;
        rb.gravityScale = 3;
    }

    void Die()
    {
        GameManager.gameManager.BallDie();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        ShootingOff(); // 오브젝트와 충돌하면 다시 원래 상태로 돌아와야 하기 때문에 호출
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
                    GameManager.gameManager.DisableObj(col.gameObject);
                    // Mn클래스의 함수를 이용해서 Broken 오브젝트 제거
                    source.Play();
                    break;
                case "JumpBoost":
                    rb.velocity = new Vector2(rb.velocity.x, jumpBoostPower);
                    // x값은 유지하고 y값만 변경
                    source.Play();
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
            GameManager.gameManager.GetStar();
            GameManager.gameManager.DisableObj(col.transform.parent.gameObject);
        }
    }
}
