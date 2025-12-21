using UnityEngine;
using Cinemachine;

public class DangerShake : MonoBehaviour
{
    [Header("Налаштування")]
    public string enemyTag = "Enemy";
    public float maxDistance = 10f;
    public float maxShakeIntensity = 2f;

    [Header("Посилання")]
    public CinemachineVirtualCamera virtualCamera;

    private CinemachineBasicMultiChannelPerlin noiseControl;

    void Start()
    {
        if (virtualCamera != null)
        {
            noiseControl = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
    }

    void Update()
    {
        if (noiseControl == null) return;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        float closestDistance = maxDistance;

        foreach (GameObject enemy in enemies)
        {
            if (enemy == null) continue;

            float distance = Vector2.Distance(transform.position, enemy.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
            }
        }

        float shakeFactor = 1 - (closestDistance / maxDistance);

        shakeFactor = Mathf.Clamp01(shakeFactor);

        noiseControl.m_AmplitudeGain = shakeFactor * maxShakeIntensity;

        noiseControl.m_FrequencyGain = shakeFactor * 10f;
    }
}