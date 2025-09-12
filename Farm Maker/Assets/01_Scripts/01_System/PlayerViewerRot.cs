using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerViewerRot : MonoBehaviour
{
    public Transform playerTrans;
    public float rotSpeed;
    float rotY;
    float rotX;
    private void Update()
    {
        transform.position = playerTrans.position;

        float X = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        rotX += rotSpeed * X * Time.deltaTime;
        rotY += rotSpeed * y * Time.deltaTime;

        rotY = Mathf.Clamp(rotY, -90, 40); //���� ����

        transform.localEulerAngles = new Vector3(-rotY,rotX,0); //���콺�� �Ʒ��� ���ϸ� ������ ���� ���� ���콺�� ���� ���ϸ� ������ �Ʒ��� ���ϱ� ���� -�� ����

    }
}
