using UnityEngine;

public class FlashlightControl : MonoBehaviour
{
    private Vector3 lastDirection = Vector3.right;

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = new Vector3(moveX, moveY, 0).normalized;

        if (moveDirection.magnitude > 0.1f)
        {
            lastDirection = moveDirection;
        }

        float angle = Mathf.Atan2(lastDirection.y, lastDirection.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }
}