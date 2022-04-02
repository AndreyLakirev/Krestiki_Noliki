using System;
using Control;
using UnityEngine;
using UnityEngine.UI;

namespace Network
{
    public class CreateMenu : MonoBehaviour
    {
        [SerializeField] private Text status;
        [SerializeField] private InputField inputField;
        [SerializeField] private Text localAddress;
        [SerializeField] private Text externalAddress;

        public void CreateConnection()
        {
            try
            {
                int port = int.Parse(inputField.text);
                GameController.ConnectionHandler.Host(port);
                status.text = "ON";
                string localIp = ConnectionUtil.GetLocalIPAddress();
                localAddress.text = localIp + ":" + port;
                externalAddress.text = ConnectionUtil.GetExternalIPAddress() + ":" + port;
                GUIUtility.systemCopyBuffer = localIp + ":" + port;
            }
            catch (Exception e)
            {
                Debug.Log(e);
                status.text = "ERROR: " + e.Message;
            }
        }
    }
}
