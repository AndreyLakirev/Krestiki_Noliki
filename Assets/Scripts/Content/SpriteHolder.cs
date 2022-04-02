using UnityEngine;

namespace Content
{
    public class SpriteHolder : MonoBehaviour
    {
        public Sprite crossSprite;
        public Sprite circleSprite;
        public Sprite borderSprite;

        public static SpriteHolder Instance { get; private set; }

        private void Start()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            Instance = this;
        }
    }
}
