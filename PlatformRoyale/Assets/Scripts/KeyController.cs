using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyController : MonoBehaviour
{
    [SerializeField] private UnityEvent key;

    private Transform follow;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && follow == null)
        {
            follow = other.transform;
            if (other.GetComponent<PlayerController>())
                other.GetComponent<PlayerController>().SetKey();
            transform.parent = null;
            FindObjectOfType<AudioManager>().Play("Key");
            key.Invoke();
        }
    }

    private void Update()
    {
        if (follow != null)
            if ((transform.position - follow.position).magnitude > 1.5f)
            {
                float x = Mathf.Lerp(transform.position.x, follow.position.x, Time.deltaTime);
                float y = Mathf.Lerp(transform.position.y, follow.position.y, Time.deltaTime);
                transform.position = new Vector3(x, y, transform.position.z);
            }
    }
}
