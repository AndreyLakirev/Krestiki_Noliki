using System;
using System.Collections.Generic;
using System.Text;
using Network;
using UnityEngine;
using Util;
using Random = System.Random;

namespace Control
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private GameObject gameFieldPrefab;
        [SerializeField] private GameObject borderPrefab;
        private static readonly GameField[] _gameFields = new GameField[9];
        public static readonly ConnectionHandler ConnectionHandler = new ConnectionHandler();

        private static void Interact(int index, FieldType fieldType)
        {
            if (_gameFields[index].Interact(fieldType)) PlayerController.Instance.ChangeTurn();
        }

        private void Start()
        {
            ConnectionHandler.onMessageConsume += s =>
            {
                string[] arguments = s.Split(':');
                if (arguments[0].Equals("StartGame"))
                {
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    {
                        StartGame(int.Parse(arguments[1]));
                    });
                }
                if (arguments[0].Equals("ClickOnField"))
                {
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    {
                        Interact(int.Parse(arguments[1]), (FieldType) int.Parse(arguments[2]));
                    });
                }
            };
            ConnectionHandler.onPlayerConnected += s =>
            {
                Random random = new Random();
                int enumLength = Enum.GetNames(typeof(FieldType)).Length;
                int firstRandom = random.Next(1, enumLength - 1);
                int secondRandom = RandomGenerator.GenerateRandomIntWithExcludingSet(1, enumLength - 1, new HashSet<int>{firstRandom});
                Debug.Log("FirstRandom: " + firstRandom);
                Debug.Log("SecondRandom: " + secondRandom);
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    StartGame(firstRandom);
                });
                ConnectionHandler.Send(Encoding.UTF8.GetBytes("StartGame:" + secondRandom));
            };
        }

        private void StartGame(int playerFieldType)
        {
            GameObject.Find("MenuCanvas").SetActive(false);
            PlayerController.Instance.fieldType = (FieldType)playerFieldType;
            if (PlayerController.Instance.fieldType.Equals(FieldType.Cross)) PlayerController.Instance.ChangeTurn();
            Instantiate(borderPrefab, transform);
            for (int i = 0; i < 3; i++)
            {
                for (int w = 0; w < 3; w++)
                {
                    GameObject obj = Instantiate(gameFieldPrefab, transform);
                    obj.name += i * 3 + w;
                    GameField gameField = obj.GetComponent<GameField>();
                    obj.transform.position = new Vector3(w * 6 - 6, -i * 6 + 6, 0);
                    _gameFields[i * 3 + w] = gameField;
                }
            }
        }

        private void OnDestroy()
        {
            ConnectionHandler.Dispose();
        }

        public void CloseGame()
        {
            Application.Quit();
        }
    }
}
