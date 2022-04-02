using System;
using Control;
using UnityEngine;
using UnityEngine.UI;

namespace Network
{
    public class JoinMenu : MonoBehaviour
    {
        [SerializeField] private Text message;
        [SerializeField] private InputField inputField;

        public void Connect()
        {
            message.text = "";
            try
            {
                string[] strings = inputField.text.Split(':');
                GameController.ConnectionHandler.Connect(strings[0], int.Parse(strings[1]));
                gameObject.SetActive(false);
            }
            catch (Exception e)
            {
                message.text = e.Message;
            }
        }
    }
}
