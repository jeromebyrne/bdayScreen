using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class QuitOnEscape : MonoBehaviour
{
    private void Update()
    {
        if (IsEscapePressed())
        {
            #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
    }

    private bool IsEscapePressed()
    {
        #if ENABLE_INPUT_SYSTEM
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            return true;
        }
        #endif

        return Input.GetKeyDown(KeyCode.Escape);
    }
}
