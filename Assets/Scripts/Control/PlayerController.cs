using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Control
{
    public class PlayerController : MonoBehaviour
    {
        public FieldType fieldType = FieldType.Empty;
        [SerializeField] private bool turn;

        public static PlayerController Instance { get; private set; }

        private void Start()
        {
            Setup();
        }

        private void FixedUpdate()
        {
            if (Input.GetKey(KeyCode.Escape)) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            if (!turn) return;
            if (!Input.GetMouseButton(0)) return;
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider == null) return;
            if (!hit.collider.gameObject.GetComponent<IFieldInteractable>().Interact(fieldType)) return;
            ChangeTurn();
            string gameObjectName = hit.collider.gameObject.name;
            string message = "ClickOnField:" + gameObjectName[gameObjectName.Length - 1] + ":" + (int) fieldType;
            Debug.Log("Message to send: " + message);
            GameController.ConnectionHandler.Send(Encoding.UTF8.GetBytes(message));
        }

        private void Setup()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            Instance = this;
        }
        public void ChangeTurn()
        {
            turn = !turn;
        }
    }
}
