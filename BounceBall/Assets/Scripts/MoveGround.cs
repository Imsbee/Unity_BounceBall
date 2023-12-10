using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGround : MonoBehaviour
{
    int n = 1;
    Vector2 pos;

    void Awake()
    {
        pos = transform.position;
    }
    
    void FixedUpdate()
    {
        pos.x += n * Time.deltaTime; // MoveGround의 x값을 일정한 시간 간격으로 이동(프레임과 무관하게)

        transform.position = pos; // 이동된 위치를 적용
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag == "Broken") // Broken과 충돌하면
        {
            n = n * -1; // 반대 방향으로 전환
        }
    }
}
