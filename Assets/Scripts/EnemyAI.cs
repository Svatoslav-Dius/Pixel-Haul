using UnityEngine;
using UnityEngine.AI; // ОБОВ'ЯЗКОВО: Підключаємо бібліотеку штучного інтелекту

public class EnemyAI : MonoBehaviour
{
    [Header("Налаштування Зору")]
    public float viewRadius = 5f;
    private Transform player;
    private NavMeshAgent agent; // Посилання на "мозок"

    void Start()
    {
        // Отримуємо компонент агента
        agent = GetComponent<NavMeshAgent>();

        // Відключаємо автоматичне оновлення повороту агентом,
        // бо ми будемо повертати спрайт вручну (для 2D)
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        GameObject playerObj = GameObject.Find("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player != null)
        {
            // 1. НАЙГОЛОВНІШЕ: Кажемо агенту "Йди до гравця!"
            // Він сам побудує шлях в обхід стін.
            if (CanSeePlayer())
            {
                agent.SetDestination(player.position);
            }
            // Якщо НЕ бачимо -> зупиняємось там, де дійшли (або можна додати патрулювання)
            else
            {
                // Якщо агент дійшов до останньої відомої точки - зупиняємо
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                {
                    agent.ResetPath();
                }
            }

            // Поворот спрайта (завжди дивиться туди, куди йде, або стоїть)
            if (agent.velocity.magnitude > 0.1f) // Крутимось тільки якщо рухаємось
            {
                Vector2 direction = agent.velocity.normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));
            }
        }
    }

    bool CanSeePlayer()
    {
        if (player == null) return false;

        // 1. Перевірка Дистанції
        float distToPlayer = Vector2.Distance(transform.position, player.position);
        if (distToPlayer > viewRadius) return false; // Занадто далеко

        // 2. Перевірка Кута (Чи гравець спереду?)
        // Оскільки наш спрайт дивиться "Вгору" (через -90), то його "перед" це transform.up
        //Vector2 dirToPlayer = (player.position - transform.position).normalized;
        //if (Vector2.Angle(transform.up, dirToPlayer) > viewAngle / 2) return false; // За спиною

        // 3. Перевірка Стін (Raycast)
        // Пускаємо промінь у бік гравця. Якщо він врізається в стіну (obstacleMask) - ми не бачимо.
        //if (Physics2D.Raycast(transform.position, dirToPlayer, distToPlayer, obstacleMask))
        //{
        //    return false; // Стіна заважає
        //}

        return true; // Ми бачимо гравця!
    }

    void OnDrawGizmos()
    {
        // Малюємо коло радіусу
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, viewRadius);
    }
}