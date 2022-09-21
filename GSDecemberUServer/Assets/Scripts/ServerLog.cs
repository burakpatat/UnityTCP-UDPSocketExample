using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class ServerLog : MonoBehaviour
{
    public static ServerLog _instance;
    public static ServerLog instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (ServerLog)GameObject.FindObjectOfType(typeof(ServerLog));
                if (_instance == null)
                {
                    GameObject go = new GameObject("ServerLog:ServerLogIns");
                    _instance = go.AddComponent<ServerLog>();
                }
            }
            return _instance;
        }
        set
        {
            _instance = value;
        }
    }

    string PATH = @"D:\Projects\Server2020\ServerLog.txt";
    private void Start()
    {
        _instance = this;
    }
    public void WriteLog(string message)
    {
        StreamWriter logFile = new StreamWriter(PATH, true);
        logFile.WriteLine(message);
        logFile.Close();
    }
}
