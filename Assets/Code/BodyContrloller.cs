using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using UnityEngine;

public class BodyContrloller : MonoBehaviour
{
    public float XRotationOffset = 0f; // 调整初始X轴角度
    public float YRotationOffset = 0f; // 调整初始Y轴角度
    public float ZRotationOffset = 90f; // 调整初始Z轴角度
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

    [Header("身体骨骼")]
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

    //解析单条数据为浮点数据
    private void ParseBodyInfo(int index, ref Transform body)
    {
        string BodyInfo = Gamemanager.sender.getLatestUDPPacket(index);

        if (!string.IsNullOrEmpty(BodyInfo))
        {
            string[] axisData = BodyInfo.Split(',');
            float Roll = float.Parse(axisData[0]);
            float Pitch = float.Parse(axisData[1]);
            float Yaw = float.Parse(axisData[2]);
            // 将字符串数据转换为浮点数

            BodyRotation(index, ref body, Roll, Pitch, Yaw);
        }

    }

    private void BodyRotation(int index, ref Transform body, float roll, float pitch, float yaw)
    {
        UnityEngine.Quaternion armRotation = new UnityEngine.Quaternion();
        UnityEngine.Quaternion armRotationOffset = new UnityEngine.Quaternion();

        if (index >= 1)
        {
            // 将三轴值转化为Quaternion
            armRotation = UnityEngine.Quaternion.Euler(roll, yaw, -pitch);
            armRotationOffset = UnityEngine.Quaternion.Euler(XRotationOffset, YRotationOffset, ZRotationOffset);
        }
        else
        {
            armRotation = UnityEngine.Quaternion.Euler(roll, yaw, pitch);
            armRotationOffset = UnityEngine.Quaternion.Euler(0, 0, 0);
        }

        // Normalize确保角度在[-180, 180]范围内
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
