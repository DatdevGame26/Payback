using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  Điều chỉnh ánh sáng theo hiệu ứng (giảm cường độ sáng theo thời gian tồn tại của VFX)
public class LightIntensityDecrease : MonoBehaviour
{
    [SerializeField] Light particleLight;
    private ParticleSystem particle; 
    private float startIntensity;

    void Start()
    {
        particle = GetComponent<ParticleSystem>();
        if (particleLight != null)
        {
            startIntensity = particleLight.intensity;
        }
    }

    void Update()
    {
        if (particleLight != null && particle != null)
        {
            float duration = particle.main.duration;
            float t = Mathf.Clamp01(particle.time / duration);
            particleLight.intensity = Mathf.Lerp(startIntensity, 0, t);
        }
    }
}
