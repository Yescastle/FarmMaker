using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController cc;
    public float moveSpeed;

    //ȸ�� ���ǵ�(�¿�) ���� ȸ���� ī�޶� ���� ����
    public float rotSpeed;

    //������
    public float jumpPower;
    private float yVelocity;
    private bool jumpAble = true;
    float gravity = -15f;

    public Transform CamPos;
    public GameObject camera;

    float Rot = 0; //ȸ����
    private void Start()
    {
        cc = GetComponent<CharacterController>();
        camera.transform.parent = CamPos;
        camera.transform.localPosition = Vector3.zero;
    }
    private void Update()
    {
        // 1. ������� �Է��� ����
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 2. �̵� ������ ����
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        // �÷��̾��� ȸ������ �������� �̵� ���� ��ȯ
        dir = transform.TransformDirection(dir);

        // 2-2. ����, ���� ���̾���, �ٽ� �ٴڿ� �����ߴٸ�
        if (cc.collisionFlags == CollisionFlags.Below && jumpAble == false)
        {
            yVelocity = 0;
            jumpAble = true;
        }

        if (Input.GetKey(KeyCode.Space) && jumpAble)
        {
            yVelocity = jumpPower;
            jumpAble = false;
        }

       
        yVelocity += gravity * Time.deltaTime;

        dir.y = yVelocity;

        cc.Move(dir * moveSpeed * Time.deltaTime);

        float rotX = Input.GetAxis("Mouse X");

        Rot += rotX * rotSpeed * Time.deltaTime;

        // 2. ȸ�� �������� ��ü�� ȸ����Ŵ
        transform.eulerAngles = new Vector3(0, Rot, 0);
    }
}
