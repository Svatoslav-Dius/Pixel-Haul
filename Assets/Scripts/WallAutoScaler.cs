using UnityEngine;
using UnityEngine.AI;

[ExecuteInEditMode]
public class WallAutoScaler : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private NavMeshObstacle navObstacle;

    void Update()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        if (boxCollider == null) boxCollider = GetComponent<BoxCollider2D>();
        if (navObstacle == null) navObstacle = GetComponent<NavMeshObstacle>();

        if (spriteRenderer != null)
        {
            Vector2 spriteSize = spriteRenderer.size;

            // АВТО-КОЛАЙДЕР
            if (boxCollider != null)
            {
                // Вимикаємо Auto Tiling, щоб ми могли керувати розміром вручну через скрипт
                boxCollider.autoTiling = false;
                boxCollider.size = spriteSize;
            }

            // АВТО-НАВІГАЦІЯ
            if (navObstacle != null)
            {
                // Робимо розмір перешкоди таким самим, як спрайт
                navObstacle.size = new Vector3(spriteSize.x, spriteSize.y, 10f);
                navObstacle.carving = true;
            }
        }
    }
}