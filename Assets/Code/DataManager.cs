using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    private string ipAddressKey;

    private void Awake()
    {
        ipAddressKey = transform.parent.parent.name;
    }

    // 在Start方法中使用
    private void Start()
    {
        string savedIpAddress = LoadIpAddress();
        transform.parent.GetComponent<InputField>().text = savedIpAddress;
    }

    // 保存IP地址
    public void SaveIpAddress(string ipAddress)
    {
        PlayerPrefs.SetString(ipAddressKey, ipAddress);
        PlayerPrefs.Save();
    }

    // 读取IP地址
    public string LoadIpAddress()
    {
        return PlayerPrefs.GetString(ipAddressKey, "");
    }
}
