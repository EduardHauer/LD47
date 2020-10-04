using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private Color finished = new Color(0, 0, 1);
    [SerializeField] private Color play = new Color(0, 0, 0);
    [SerializeField] private Color closed = new Color(1, 0, 0);
    [SerializeField] private SpriteRenderer[] levels = new SpriteRenderer[0];
    [SerializeField] private EventTrigger[] buttons = new EventTrigger[0];

    private void Start()
    {
        for (int i = 0; i < levels.Length && i < GameData.lastCompleted; i++)
        {
            levels[i].color = finished;
            buttons[i].enabled = true;
        }
        if (GameData.lastCompleted < levels.Length)
        {
            levels[GameData.lastCompleted].color = play;
            buttons[GameData.lastCompleted].enabled = true;
        }
        for (int i = GameData.lastCompleted + 1; i < levels.Length; i++)
        {
            levels[i].color = closed;
            buttons[i].enabled = false;
        }
    }

    public void LoadLevel(int id)
    {
        SceneManager.LoadScene(id);
    }

    public void Click()
    {
        FindObjectOfType<AudioManager>().Play("Click");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
