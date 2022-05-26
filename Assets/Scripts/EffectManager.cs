using System.Collections;
using UnityEngine;

public class EffectManager : GameComponent
{
    private IEnumerator PlayCoroutine(ParticleSystem particle, Vector2 position)
    {
        var time = particle.main.duration + particle.main.startLifetime.constant;
        var particleClone = Instantiate(particle, position, new Quaternion());
        particleClone.Play();
        yield return new WaitForSeconds(time);
        Destroy(particleClone.gameObject);
    }

    public void Play(ParticleSystem particle, Vector2 position)
    {
        StartCoroutine(PlayCoroutine(particle, position));
    }
}