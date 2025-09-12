using System.Collections;
using System.Collections.Generic;
using Ultrabolt.SkyEngine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController ps;

    private void Awake() //���� playerState�� �̱������� ����� ���� ��
    {
        if (ps == null)
            ps = this;
    }

    public enum State
    {
        idle,
        walk,
        run,
        sleep,
        KO
    }

    public State state;

    CharacterController cc;
    public float basicMoveSpeed; //�⺻ �̵� �ӵ�
    private float moveSpeed; //���� �̵� �ӵ�

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

    //�÷��̾� ü��
    HpController hp;

    //�÷��̾� ���
    public float maxStamina;
    private float stamina;

    //���¹̳� �Ҹ�Ǵ� ��;
    private float useStamina;

    //���¹̳� ��
    public Slider StaminaBar;

    //���¹̳� �Ƿ� ����
    private bool isTired;
    private void Start()
    {
        cc = GetComponent<CharacterController>();
        camera.transform.parent = CamPos;
        camera.transform.localPosition = Vector3.zero;

        hp = GetComponent<HpController>();

        moveSpeed = basicMoveSpeed;

        stamina = maxStamina;
        //stamina = -14.5f;

        state = State.idle;

        useStamina = 0.01f;

        StaminaBar.maxValue = maxStamina;
        StaminaBar.value = stamina;
    }

    private void Update()
    {
        if (state == State.sleep || state == State.KO) //�÷��̾ ���� �ڰ� �ְų� ������ ������ ���
        {
            return;
        }

        // 1. ������� �Է��� ����
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if( h == 0 && v == 0)
        {
            state = State.idle;
        }
        else if( state == State.idle )
        {
            state = State.walk;
        }
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
        else
            yVelocity += gravity * Time.deltaTime; //�߷� ����, �������� �ӵ�

        //����
        if (Input.GetKey(KeyCode.Space) && jumpAble)
        {
            yVelocity = jumpPower;
            jumpAble = false;
        }

        //�޸���
        if(Input.GetKey(KeyCode.LeftShift) && state != State.run && stamina > 0)
        {
            moveSpeed *= 1.5f;
            state = State.run;
        }
        else if ((Input.GetKeyUp(KeyCode.LeftShift) || stamina <= 0) && !isTired) //�޸��� ����
        {
            moveSpeed = basicMoveSpeed;
            state = State.idle;
        }

        if(state == State.run)
        {
            stamina -= 0.2f * Time.deltaTime;
        }


        dir.y = yVelocity;

        cc.Move(dir * moveSpeed * Time.deltaTime);
        Debug.Log($"���ǵ� : {moveSpeed}");

        float rotX = Input.GetAxis("Mouse X");

        Rot += rotX * rotSpeed * Time.deltaTime;

        // 2. ȸ�� �������� ��ü�� ȸ����Ŵ
        transform.eulerAngles = new Vector3(0, Rot, 0);


        if(stamina < 0 && !isTired)
        {
            moveSpeed /= 2;
            useStamina = 0.1f;
            isTired = true;
        }
        else if(isTired && stamina > 0)
        {
            moveSpeed = basicMoveSpeed;
            useStamina = 0.01f;
            isTired = false;
        }    

        stamina -= useStamina * Time.deltaTime; //�ǽð� ���¹̳� �Ҹ�
        Debug.Log(stamina);
        StaminaBar.value = stamina;

        if(stamina <= -15f) //���¹̳ʰ� -15�� �Ǿ��� ���
        {
            state = State.KO;
            hp.Recovery();
        }
    }

    public void Sleeping()
    {
        state = State.sleep;
        hp.Recovery();
    }

    public void EatFood(float food)
    {
        stamina += food;
    }

    public void WakeUp()
    {
        if (state == State.KO)
        {
            Debug.Log("����");
            stamina = maxStamina / 2;
        }
        else
        {
            stamina = maxStamina;
        }
        state = State.idle;
    }
}
