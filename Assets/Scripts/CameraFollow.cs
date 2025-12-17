using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    // LateUpdate спрацьовує після того, як гравець вже посунувся.
    // Це робить рух камери плавнішим, без тремтіння.
    void LateUpdate()
    {
        if (player != null)
        {
            transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
        }
    }
}