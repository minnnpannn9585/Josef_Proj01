using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    // 存储物品和数量的键值对容器
    private Dictionary<string, int> playerInventory = new Dictionary<string, int>();


    /// <summary>
    /// 添加物品类型
    /// </summary>
    public void AddItem(string itemName)
    {
        if (playerInventory.ContainsKey(itemName))
        {
            return;
        }
        else
        {
            playerInventory.Add(itemName, 0);
        }
    }
    /// <summary>
    /// 添加物品数量
    /// </summary>
    public void AddItem(string itemName, int amount)
    {
        if (amount <= 0) return;

        if (playerInventory.ContainsKey(itemName))
        {
            playerInventory[itemName] += amount;
        }
        else
        {
            playerInventory.Add(itemName, amount);
        }

        Debug.Log($"增加了 {amount} 个 [{itemName}]，当前总数: {playerInventory[itemName]}");
    }

    /// <summary>
    /// 消耗/移除物品
    /// </summary>
    /// <returns>如果扣除成功返回 true，数量不足或没有该物品返回 false</returns>
    public bool RemoveItem(string itemName, int amount)
    {
        if (!playerInventory.ContainsKey(itemName) || playerInventory[itemName] < amount)
        {
            Debug.LogWarning($"无法扣除 [{itemName}]，数量不足！");
            return false;
        }

        playerInventory[itemName] -= amount;
        Debug.Log($"扣除了 {amount} 个 [{itemName}]，剩余: {playerInventory[itemName]}");


        return true;
    }

    /// <summary>
    /// 查询某个物品的数量
    /// </summary>
    public int GetItemCount(string itemName)
    {
        if (playerInventory.ContainsKey(itemName))
        {
            return playerInventory[itemName];
        }
        return 0; // 没有该物品则返回 0
    }

    /// <summary>
    /// 打印当前背包所有物品（调试用）
    /// </summary>
    public void PrintInventory()
    {
        Debug.Log("--- 当前背包物品列表 ---");
        foreach (KeyValuePair<string, int> item in playerInventory)
        {
            Debug.Log($"物品: {item.Key} | 数量: {item.Value}");
        }
    }
}
