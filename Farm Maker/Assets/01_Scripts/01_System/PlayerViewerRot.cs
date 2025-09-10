using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerViewerRot : MonoBehaviour
{
    public float rotSpeed;
    float rotY;

    private void Update()
    {
        float y = Input.GetAxis("Mouse Y");

        rotY += rotSpeed * y * Time.deltaTime;

        rotY = Mathf.Clamp(rotY, -90, 40); //���� ����

        transform.localEulerAngles = new Vector3(-rotY,0,0); //���콺�� �Ʒ��� ���ϸ� ������ ���� ���� ���콺�� ���� ���ϸ� ������ �Ʒ��� ���ϱ� ���� -�� ����

    }
}
