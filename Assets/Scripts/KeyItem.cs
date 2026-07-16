using UnityEngine;

public class KeyItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }

        Door[] doors = FindObjectsOfType<Door>();

        foreach (Door door in doors)
        {
            door.SetHasKey(true);
        }

        Destroy(gameObject);
    }
}
