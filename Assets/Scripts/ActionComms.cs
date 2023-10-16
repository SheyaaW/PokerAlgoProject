using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AustinHarris.JsonRpc;

public class ActionComms : MonoBehaviour
{
    public static ActionComms Instance;
    class Rpc : JsonRpcService
    {
        [JsonRpcMethod]
        public int SendCommand(int command)
        {
            return command;
        }
    }

    Rpc rpc;

    void Start()
    {
        Instance = this;
        rpc = new Rpc();
    }

    public int SendCommand(int command)
    {
        return rpc.SendCommand(command);
    }
}