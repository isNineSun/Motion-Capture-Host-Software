using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using UnityEngine;

public class BodyContrloller : MonoBehaviour
{
    public float XRotationOffset = 0f; // ������ʼX��Ƕ�
    public float YRotationOffset = 0f; // ������ʼY��Ƕ�
    public float ZRotationOffset = 90f; // ������ʼZ��Ƕ�
    private enum BodyID
    {
        Head,
        LeftUpArm,
        LeftLowArm,
        RightUpArm,
        RightLowArm,
        LeftUpLeg,
        LeftLowLeg,
        RightUpLeg,
        RightLowLeg
    }

    [Header("�������")]
    public Transform Head;
    public Transform leftUpArm;
    public Transform leftlowArm;
    public Transform rightUpArm;
    public Transform rightlowArm;
    public Transform leftUpLeg;
    public Transform leftlowLeg;
    public Transform rightUpLeg;
    public Transform rightlowLeg;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        BodyTransUpdate();
    }

    //������������Ϊ��������
    private void ParseBodyInfo(int index, ref Transform body)
    {
        string BodyInfo = Gamemanager.sender.getLatestUDPPacket(index);

        if (!string.IsNullOrEmpty(BodyInfo))
        {
            string[] axisData = BodyInfo.Split(',');
            float Roll = float.Parse(axisData[0]);
            float Pitch = float.Parse(axisData[1]);
            float Yaw = float.Parse(axisData[2]);
            // ���ַ�������ת��Ϊ������

            BodyRotation(index, ref body, Roll, Pitch, Yaw);
        }

    }

    private void BodyRotation(int index, ref Transform body, float roll, float pitch, float yaw)
    {
        UnityEngine.Quaternion armRotation = new UnityEngine.Quaternion();
        UnityEngine.Quaternion armRotationOffset = new UnityEngine.Quaternion();

        if (index >= 1)
        {
            // ������ֵת��ΪQuaternion
            armRotation = UnityEngine.Quaternion.Euler(roll, yaw, -pitch);
            armRotationOffset = UnityEngine.Quaternion.Euler(XRotationOffset, YRotationOffset, ZRotationOffset);
        }
        else
        {
            armRotation = UnityEngine.Quaternion.Euler(roll, yaw, pitch);
            armRotationOffset = UnityEngine.Quaternion.Euler(0, 0, 0);
        }

        // Normalizeȷ���Ƕ���[-180, 180]��Χ��
        //armRotation.Normalize();

        body.rotation = armRotation * armRotationOffset;
    }

    private void BodyTransUpdate()
    {
        ParseBodyInfo((int)BodyID.Head, ref Head);
        ParseBodyInfo((int)BodyID.LeftUpArm, ref leftUpArm);
        ParseBodyInfo((int)BodyID.LeftLowArm, ref leftlowArm);
    }
}
