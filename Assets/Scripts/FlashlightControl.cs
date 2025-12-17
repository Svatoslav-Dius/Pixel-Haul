using UnityEngine;

public class FlashlightControl : MonoBehaviour
{
    // Змінна, щоб пам'ятати, куди ми дивилися останній раз
    private Vector3 lastDirection = Vector3.right;

    void Update()
    {
        // 1. Читаємо натискання WASD
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // Створюємо вектор руху
        Vector3 moveDirection = new Vector3(moveX, moveY, 0).normalized;

        // 2. Якщо гравець рухається (вектор не нульовий)
        if (moveDirection.magnitude > 0.1f)
        {
            // Запам'ятовуємо цей напрямок як "останній"
            lastDirection = moveDirection;
        }

        // 3. Обчислюємо кут повороту на основі останнього напрямку
        // Math magic: Atan2 перетворює координати (X, Y) у кут в градусах
        float angle = Mathf.Atan2(lastDirection.y, lastDirection.x) * Mathf.Rad2Deg;

        // 4. Повертаємо ліхтарик
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }
}