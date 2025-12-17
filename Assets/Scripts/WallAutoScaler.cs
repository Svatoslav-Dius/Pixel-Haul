using UnityEngine;
using UnityEngine.AI; // Потрібно для NavMesh

[ExecuteInEditMode] // Ця магія змушує скрипт працювати прямо в редакторі!
public class WallAutoScaler : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private NavMeshObstacle navObstacle;

    void Update()
    {
        // 1. Знаходимо компоненти (якщо їх ще не знайшли)
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        if (boxCollider == null) boxCollider = GetComponent<BoxCollider2D>();
        if (navObstacle == null) navObstacle = GetComponent<NavMeshObstacle>();

        // 2. Якщо є картинка, починаємо магію
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

            // АВТО-НАВІГАЦІЯ (Найважливіше!)
            if (navObstacle != null)
            {
                // Робимо розмір перешкоди таким самим, як спрайт
                // Z ставимо товстим (10), щоб пробити підлогу
                navObstacle.size = new Vector3(spriteSize.x, spriteSize.y, 10f);
                navObstacle.carving = true; // Автоматично вмикаємо "Вирізання"
            }
        }
    }
}