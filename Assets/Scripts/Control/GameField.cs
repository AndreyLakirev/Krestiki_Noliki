using Content;
using UnityEngine;

namespace Control
{
    public class GameField : MonoBehaviour, IFieldInteractable
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        private FieldType _fieldType = FieldType.Empty;

        public bool Interact(FieldType fieldType)
        {
            if (_fieldType != FieldType.Empty) return false;
            switch (fieldType)
            {
                case FieldType.Empty:
                    return false;
                case FieldType.Circle:
                    fieldType = FieldType.Circle;
                    spriteRenderer.sprite = SpriteHolder.Instance.circleSprite;
                    break;
                case FieldType.Cross:
                    fieldType = FieldType.Cross;
                    spriteRenderer.sprite = SpriteHolder.Instance.crossSprite;
                    break;
                default:
                    return false;
            }
            _fieldType = fieldType;
            return true;
        }
    }
}
