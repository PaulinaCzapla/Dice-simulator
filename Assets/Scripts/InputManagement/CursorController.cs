using UnityEngine;

namespace InputManagement
{
    public static class CursorController
    {
        public static void Hide()
        {
            Cursor.visible = false;
        }

        public static void Show()
        {
            Cursor.visible = true;
        }
    }
}