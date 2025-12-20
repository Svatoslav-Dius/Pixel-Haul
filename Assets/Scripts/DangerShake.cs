using UnityEngine;
using Cinemachine; // Обов'язково додаємо бібліотеку

public class DangerShake : MonoBehaviour
{
    [Header("Налаштування")]
    public string enemyTag = "Enemy";      // Тег ворогів
    public float maxDistance = 10f;        // Відстань, на якій починає трясти
    public float maxShakeIntensity = 2f;   // Максимальна сила тряски (коли павук впритул)

    [Header("Посилання")]
    public CinemachineVirtualCamera virtualCamera; // Сюди перетягнемо камеру

    private CinemachineBasicMultiChannelPerlin noiseControl;

    void Start()
    {
        // Отримуємо доступ до управління шумом (Noise) на камері
        if (virtualCamera != null)
        {
            noiseControl = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
    }

    void Update()
    {
        if (noiseControl == null) return;

        // 1. Знаходимо всіх павуків
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        float closestDistance = maxDistance; // Початкове значення - максимум

        // 2. Шукаємо найближчого
        foreach (GameObject enemy in enemies)
        {
            if (enemy == null) continue;

            float distance = Vector2.Distance(transform.position, enemy.transform.position);

            // Якщо знайшли когось ближче, запам'ятовуємо цю відстань
            if (distance < closestDistance)
            {
                closestDistance = distance;
            }
        }

        // 3. Обчислюємо силу тряски
        // Якщо павук далеко (10м) -> shake = 0
        // Якщо павук близько (0м) -> shake = 1
        float shakeFactor = 1 - (closestDistance / maxDistance);

        // Обрізаємо значення, щоб воно не було менше 0
        shakeFactor = Mathf.Clamp01(shakeFactor);

        // 4. Застосовуємо до камери
        noiseControl.m_AmplitudeGain = shakeFactor * maxShakeIntensity;

        // Додатково: чим ближче, тим швидше трусить (Frequency)
        noiseControl.m_FrequencyGain = shakeFactor * 10f;
    }
}