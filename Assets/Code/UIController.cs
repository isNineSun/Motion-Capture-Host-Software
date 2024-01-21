using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Networking.Types;

public struct Body_axis
{
    public Text x_axis;
    public Text y_axis;
    public Text z_axis;
}

public class UIController : MonoBehaviour
{
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

    [Header("菜单管理")]
    public Transform TargetIPList;

    [Header("目标设备IP地址")]
    public Text[] TargetIpsNumbers = new Text[9];

    [Header("身体部位UI数据")]
    public Text[] HeadInfo = new Text[3];
    public Text[] leftUpArmInfo = new Text[3];
    public Text[] leftlowArmInfo = new Text[3];
    public Text[] rightUpArmInfo = new Text[3];
    public Text[] rightlowArmInfo = new Text[3];
    public Text[] leftUpLegInfo = new Text[3];
    public Text[] leftlowLegInfo = new Text[3];
    public Text[] rightUpLegInfo = new Text[3];
    public Text[] rightlowLegInfo = new Text[3];

    // Start is called before the first frame update
    void Start()
    {
        TargetIPList.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -1200f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        BodyInfoUpdate();
    }

    //解析单条数据为三轴字符串
    private void ParseBodyInfo2String(int index, ref Text[] Axis_str)
    {
        string BodyInfo = Gamemanager.sender.getLatestUDPPacket(index);
        if (!string.IsNullOrEmpty(BodyInfo))
        {
            string[] axisData = BodyInfo.Split(',');
            // 继续处理 axisData
            Axis_str[0].text = axisData[0];
            Axis_str[1].text = axisData[1];
            Axis_str[2].text = axisData[2];
        }
    }

    //身体数据更新
    private void BodyInfoUpdate()
    {
        ParseBodyInfo2String((int)BodyID.Head, ref HeadInfo);
        ParseBodyInfo2String((int)BodyID.LeftUpArm, ref leftUpArmInfo);
        ParseBodyInfo2String((int)BodyID.LeftLowArm, ref leftlowArmInfo);
        ParseBodyInfo2String((int)BodyID.RightUpArm, ref rightUpArmInfo);
        ParseBodyInfo2String((int)BodyID.RightLowArm, ref rightlowArmInfo);
        ParseBodyInfo2String((int)BodyID.LeftUpLeg, ref leftUpLegInfo);
        ParseBodyInfo2String((int)BodyID.LeftLowLeg, ref leftlowLegInfo);
        ParseBodyInfo2String((int)BodyID.RightUpLeg, ref rightUpLegInfo);
        ParseBodyInfo2String((int)BodyID.RightLowLeg, ref rightlowLegInfo);
    }

    public void Trigger_set_panel_active(GameObject panel)
    {
        RectTransform panelRectTrans = panel.transform.GetComponent<RectTransform>();

        if (panel.activeSelf)
        {
            panelRectTrans.DOAnchorPos(new Vector2(0, -1200f), 0.5f, false).SetEase(Ease.InOutQuint).OnComplete(() => panel.SetActive(false));
        }
        else
        {
            panel.SetActive(true);
            panelRectTrans.DOAnchorPos(new Vector2(0, 0), 1f, false).SetEase(Ease.OutElastic, 0.1f);
        }
    }

    public void Trigger_Button_animation(GameObject button)
    {
        RectTransform ButtonRectTrans = button.transform.GetComponent<RectTransform>();

        ButtonRectTrans.DOScale(new Vector3(1.2f, 1.2f, 1.5f), 0.1f).SetEase(Ease.OutSine)
            .OnComplete(() => ButtonRectTrans.DOScale(new Vector3(1f, 1f, 1f), 0.2f).SetEase(Ease.InQuad));
    }

    public void Quit()
    {
        Application.Quit();
    }
}
