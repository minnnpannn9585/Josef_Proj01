using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonDetector : MonoBehaviour
{
    public Button myButton;
    public Board board;
    void Start()
    {
        // 添加点击监听
        myButton.onClick.AddListener(OnButtonClick);
    }
    
    void OnButtonClick()
    {
        if(board != null)
        {
            board.boardMessage = board.helpMessage;
            board.targetUI.SetActive(false);
        }
    }
}