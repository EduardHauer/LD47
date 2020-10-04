using UnityEngine;

public class GameData : MonoBehaviour
{
    private static bool exist = false;
    public static int lastCompleted = 0;
    public static int take = 0;

    private void Awake()
    {
        if (!exist)
            DontDestroyOnLoad(gameObject);
        else
            Destroy(gameObject);
        exist = true;
    }

    public static void UpdateLast(int id)
    {
        if (id > lastCompleted)
            lastCompleted = id;
    }
}
