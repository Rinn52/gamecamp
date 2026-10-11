using UnityEngine;
using UnityEngine.InputSystem;

public class Title : MonoBehaviour
{
    private bool isStartingGame;

    void Update()
    {
        if (!isStartingGame && Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            isStartingGame = true;
            GameManager.Instance.LoadGameScene();
        }
    }

}
