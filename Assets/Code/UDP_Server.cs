using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Unity.VisualScripting;
using DG.Tweening;

public class UDP_Server
{
    public int sourcePort { get; private set; } // Sometimes we need to define the source port, since some devices only accept messages coming from a predefined sourceport.
    public int remotePort { get; private set; }

    public string[] targetIPs = new string[9];

    IPEndPoint[] remoteEndPoints = new IPEndPoint[9];

    // udpclient object
    UdpClient[] clients = new UdpClient[9];

    Thread receiveThread;
    // public
    // public string IP = "127.0.0.1"; default local
    public int port = 25666; // define > init

    // Information
    public string[] lastReceivedUDPPacket = new string[9];
    public string allReceivedUDPPackets = ""; // Clean up this from time to time!

    public bool newdatahereboys = false;

    public UIController UIControllers;
    public Transform UIControllers_trans;

    public void init(int RemotePort) // If sourceport is not set, its being chosen randomly by the system
    {
        int i = 0;
        sourcePort = -1;
        remotePort = RemotePort;

        UIControllers_trans = GameObject.Find("UI").transform;
        UIControllers = UIControllers_trans.GetComponent<UIController>();

        for (i = 0; i < 9; i++)
        {
            UIControllers.TargetIpsNumbers[i].GetComponent<DataManager>().SaveIpAddress(UIControllers.TargetIpsNumbers[i].text);
            if (!string.IsNullOrEmpty(UIControllers.TargetIpsNumbers[i].text))
            {
                Debug.Log("Set IP OK");
                targetIPs[i] = UIControllers.TargetIpsNumbers[i].text;
            }
        }

        for (i = 0; i < targetIPs.Length; i++)
        {
            if (!string.IsNullOrEmpty(targetIPs[i]))
            {
                remoteEndPoints[i] = new IPEndPoint(IPAddress.Parse(targetIPs[i]), remotePort);
                if (sourcePort <= -1)
                {
                    clients[i] = new UdpClient();
                    Debug.Log("Sending to " + targetIPs[i] + ": " + remotePort);
                }
                else
                {
                    clients[i] = new UdpClient(sourcePort);
                    Debug.Log("Sending to " + targetIPs[i] + ": " + remotePort + " from Source Port: " + sourcePort);
                }
            }
        }

        UIControllers.TargetIPList.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, -1200f), 0.5f, false).SetEase(Ease.InOutQuint)
            .OnComplete(() => UIControllers.TargetIPList.gameObject.SetActive(false));

        receiveThread = new Thread(
            new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();

    }

    private void ReceiveData()
    {
        int i = 0;
        byte[][] data = new byte[9][];

        string text;
        //client = sender.client;
        while (true)
        {
            try
            {
                // Bytes empfangen.
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);

                for (i = 0; i < targetIPs.Length; i++)
                {
                    if (clients[i] != null)
                    {
                        data[i] = clients[i].Receive(ref anyIP);

                        // Bytes mit der UTF8-Kodierung in das Textformat kodieren.
                        text = Encoding.UTF8.GetString(data[i]);

                        // Den abgerufenen Text anzeigen.

                        //Debug.Log(text);
                        newdatahereboys = true;
                        //PlayerPrefs.SetString("ReceivedData", text);

                        // Latest UDPpacket
                        lastReceivedUDPPacket[i] = text;

                        // ....
                        allReceivedUDPPackets = allReceivedUDPPackets + text;
                    }
                }

            }
            catch (Exception err)
            {
                Debug.Log(err.ToString());
            }
        }
    }

    // sendData in different ways. Can be extended accordingly
    public void sendString(string message)
    {
        int i = 0;

        try
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            for (i = 0; i < targetIPs.Length; i++)
            {
                if (clients[i] != null)
                    clients[i].Send(data, data.Length, remoteEndPoints[i]);
            }

        }
        catch (Exception err)
        {
            Debug.Log(err.ToString());
        }
    }

    public void sendInt32(Int32 myInt)
    {
        int i = 0;

        try
        {
            byte[] data = BitConverter.GetBytes(myInt);
            for (i = 0; i < targetIPs.Length; i++)
            {
                if (clients[i] != null)
                    clients[i].Send(data, data.Length, remoteEndPoints[i]);
            }
        }
        catch (Exception err)
        {
            Debug.Log(err.ToString());
        }
    }

    public void sendInt32Array(Int32[] myInts)
    {
        int i = 0;

        try
        {
            byte[] data = new byte[myInts.Length * sizeof(Int32)];
            Buffer.BlockCopy(myInts, 0, data, 0, data.Length);
            for (i = 0; i < targetIPs.Length; i++)
            {
                if (clients[i] != null)
                    clients[i].Send(data, data.Length, remoteEndPoints[i]);
            }
        }
        catch (Exception err)
        {
            Debug.Log(err.ToString());
        }
    }

    public void sendInt16Array(Int16[] myInts)
    {
        int i = 0;

        try
        {
            byte[] data = new byte[myInts.Length * sizeof(Int16)];
            Buffer.BlockCopy(myInts, 0, data, 0, data.Length);
            for (i = 0; i < targetIPs.Length; i++)
            {
                if (clients[i] != null)
                    clients[i].Send(data, data.Length, remoteEndPoints[i]);
            }
        }
        catch (Exception err)
        {
            Debug.Log(err.ToString());
        }
    }

    public string getLatestUDPPacket(int index)
    {
        allReceivedUDPPackets = "";
        return lastReceivedUDPPacket[index];
    }

    public void ClosePorts()
    {
        int i = 0;

        Debug.Log("closing receiving UDP on port: " + port);

        if (receiveThread != null)
            receiveThread.Abort();

        for (i = 0; i < targetIPs.Length; i++)
        {
            if (clients[i] != null)
                clients[i].Close();
        }
    }
}