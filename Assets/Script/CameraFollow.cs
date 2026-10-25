using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;

    private void LateUpdate()
    {
        if (player == null) return;

        transform.position = new Vector3(player.position.x, player.position.y, -10f);
    }
}
