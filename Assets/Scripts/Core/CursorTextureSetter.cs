using UnityEngine;

namespace UIManagementDemo.Core
{
    public class CursorTextureSetter : MonoBehaviour
    {
        [SerializeField] private Texture2D _cursorTexture;

        private void Start()
        {
            ChangeCursor();
        }

        private void ChangeCursor()
        {
            Cursor.SetCursor(_cursorTexture, Vector2.zero, CursorMode.Auto);
        }
    }
}