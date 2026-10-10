using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Title : MonoBehaviour
{
    private enum MenuState
    {
        TOP,
        GameReady,
    }

    private bool IsDecide => Keyboard.current.spaceKey.isPressed;
    private bool IsCancel => Keyboard.current.backspaceKey.isPressed;

    private MenuState currentMenu = MenuState.TOP;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(IsDecide)
        {
            // ゲームシーンに移動
            GameManager.Instance.LoadGameScene();
        }
    }

}
