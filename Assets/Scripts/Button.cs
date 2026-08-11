using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonDetector : MonoBehaviour
{
    public Button myButton;
    
    void Start()
    {
        // 添加点击监听
        myButton.onClick.AddListener(OnButtonClick);
    }
    
    void OnButtonClick()
    {
        Debug.Log("按钮被点击了！");
        // 执行你的逻辑
    }
}