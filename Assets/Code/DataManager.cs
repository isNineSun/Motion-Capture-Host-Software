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

    // ��Start������ʹ��
    private void Start()
    {
        string savedIpAddress = LoadIpAddress();
        transform.parent.GetComponent<InputField>().text = savedIpAddress;
    }

    // ����IP��ַ
    public void SaveIpAddress(string ipAddress)
    {
        PlayerPrefs.SetString(ipAddressKey, ipAddress);
        PlayerPrefs.Save();
    }

    // ��ȡIP��ַ
    public string LoadIpAddress()
    {
        return PlayerPrefs.GetString(ipAddressKey, "");
    }
}
