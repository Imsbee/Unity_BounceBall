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
        pos.x += n * Time.deltaTime; // MoveGround�� x���� ������ �ð� �������� �̵�(�����Ӱ� �����ϰ�)

        transform.position = pos; // �̵��� ��ġ�� ����
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag == "Broken") // Broken�� �浹�ϸ�
        {
            n = n * -1; // �ݴ� �������� ��ȯ
        }
    }
}
