using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    [SerializeField] private int nextLVL = 0;
    [SerializeField] private int currentLVL = 0;

    public void NextLVL()
    {
        FindObjectOfType<AudioManager>().Play("Door");
        GameData.UpdateLast(currentLVL);
        SceneManager.LoadScene(nextLVL);
    }
}
