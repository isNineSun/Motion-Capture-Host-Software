using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

public class Gamemanager : MonoBehaviour
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
    public int Remoteport = 25666;
    public string datafromnode;

    public static UDP_Server sender = new UDP_Server();

    void Start()
    {
        Application.targetFrameRate = 120;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return))
            sender.sendString("This should be delivered");

        if (sender.newdatahereboys)
        {
            //datafromnode = sender.getLatestUDPPacket((int)BodyID.Head);
            //Debug.Log(datafromnode);
        }
    }

    public void InitUPDServer()
    {
        sender.init(Remoteport);
        sender.sendString("Hello from Start. " + Time.realtimeSinceStartup);
    }

    public void OnDisable()
    {
        sender.ClosePorts();
    }

    public void OnApplicationQuit()
    {
        sender.ClosePorts();
    }

}