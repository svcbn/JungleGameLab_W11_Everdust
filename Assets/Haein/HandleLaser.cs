using UnityEngine;

public class HandleLaser : MonoBehaviour
{
    public int Dir { get; set; } = 0;

    private void Update()
    {
        if (PlayerManager.Instance.player is null) return;
        if (Dir == 0) return;
        var moveSpeed = Mathf.Max(Mathf.Abs(PlayerManager.Instance.player.transform.position.x - transform.position.x),10f);
        transform.Translate(Vector3.right * (Dir * moveSpeed * Time.deltaTime));
    }
}
