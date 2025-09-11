using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    HpController hpController;
    public int level;

    public float reqExp; //������ ���� �ʿ� ����ġ
    public float exp; //���� ����ġ ��
    private void Start()
    {
        hpController = GetComponent<HpController>();

        //hpController.MaxHp += 5 * (level - 1);
    }

    public void GetExp(float value)
    {
        exp += value;
        while(exp >= reqExp)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        exp -= reqExp;
        reqExp += 50f;

        level++;

        hpController.LevelUp();

        PlayerController player = GetComponent<PlayerController>();
        player.maxStamina += 10;
    }
}
