using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleLaser : MonoBehaviour
{
    private int dir;
    // Start is called before the first frame update
    void Start()
    {
        if (transform.position.x < 0)
        {
            dir = 1;
        }
        else
        {
            dir = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerManager.Instance.player == null) return;
        float moveSpeed = Mathf.Max(Mathf.Abs(PlayerManager.Instance.player.transform.position.x - transform.position.x),
            10f);
        transform.Translate(Vector3.right * dir * moveSpeed * Time.deltaTime);
    }
}
