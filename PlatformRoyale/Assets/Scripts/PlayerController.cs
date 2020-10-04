using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 0;
    [SerializeField] private float jumpForce = 0;
    [SerializeField] private Vector2 groundOrigin = Vector2.zero;
    [SerializeField] private LayerMask groundMask = 0;
    [SerializeField] private Vector2 clampX = Vector2.zero;
    [SerializeField] private Vector2 clampY = Vector2.zero;
    [SerializeField] private GameObject[] xLevel = new GameObject[0];
    [SerializeField] private GameObject[] yLevel = new GameObject[0];
    [SerializeField] private UnityEvent jump;
    [SerializeField] private UnityEvent warp;

    private float direction;
    private bool grounded = false;
    private Rigidbody2D rigidBody2D;
    private int xLvl = 0;
    private int yLvl = 0;
    private bool keys = false;
    private DoorController door;
    private float jumpFail = 0;

    private void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    public void Movement(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<float>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (jumpFail > 0 && context.started)
        {
            FindObjectOfType<AudioManager>()?.Play("Jump");
            rigidBody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpFail = -0.05f;
            grounded = false;
            jump.Invoke();
        }
    }

    private void Update()
    {
        if (jumpFail >= 0)
            grounded = Physics2D.Raycast((Vector2)transform.position + groundOrigin, Vector2.down, 0.1f, groundMask);
        else
            jumpFail += Time.deltaTime;
        if (grounded)
            jumpFail = 1;
        else if (jumpFail > 0)
            jumpFail = Mathf.Clamp(jumpFail - Time.deltaTime, 0, 1);

        rigidBody2D.velocity = new Vector3(speed * direction, rigidBody2D.velocity.y);
        Vector3 newPos = transform.position;
        if (newPos.x > clampX.y)
        {
            newPos.x = clampX.x;
            xLevel[xLvl].SetActive(false);
            xLvl = (xLvl + 1) % xLevel.Length;
            xLevel[xLvl].SetActive(true);
            FindObjectOfType<AudioManager>().Play("Warp");
            warp.Invoke();

        }
        else if (newPos.x < clampX.x)
        {
            newPos.x = clampX.y;
            xLevel[xLvl].SetActive(false);
            xLvl = (xLevel.Length + xLvl - 1) % xLevel.Length;
            xLevel[xLvl].SetActive(true);
            FindObjectOfType<AudioManager>().Play("Warp");
            warp.Invoke();
        }
        if (newPos.y > clampY.y)
        {
            newPos.y = clampY.x;
            yLevel[yLvl].SetActive(false);
            yLvl = (yLvl + 1) % yLevel.Length;
            yLevel[yLvl].SetActive(true);
            FindObjectOfType<AudioManager>().Play("Warp");
            warp.Invoke();
        }
        else if (newPos.y < clampY.x)
        {
            newPos.y = clampY.y;
            yLevel[yLvl].SetActive(false);
            yLvl = (yLevel.Length + yLvl - 1) % yLevel.Length;
            yLevel[yLvl].SetActive(true);
            FindObjectOfType<AudioManager>().Play("Warp");
            warp.Invoke();
        }
        transform.position = newPos;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<DoorController>() != null)
        {
            door = other.GetComponent<DoorController>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<DoorController>() != null)
        {
            door = null;
        }
    }

    //private void OnCollisionStay2D(Collision2D other)
    //{
    //    if (other.collider.tag == "Ground")
    //        grounded = true;
    //}

    //private void OnCollisionExit2D(Collision2D other)
    //{
    //    if (other.collider.tag == "Ground")
    //        grounded = false;
    //}

    public bool HasKey()
    {
        return keys;
    }

    public void SetKey()
    {
        keys = true;
    }

    public void Enter(InputAction.CallbackContext context)
    {
        if (context.started && door != null && HasKey() && grounded)
        {
            Debug.Log("WOOOOW");
            door.NextLVL();
        }
    }

    public void ScreenShot()
    {
        ScreenCapture.CaptureScreenshot("ScreenShot_" + GameData.take + ".png");
        GameData.take++;
    }
}
