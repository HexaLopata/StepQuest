using UnityEngine;

public class EffectPlayer : MonoBehaviour
{
    [SerializeField] private ParticleSystem _effectPrefab;

    public void Play()
    {
        Store.Instance.EffectManager.Play(_effectPrefab, transform.position);
    }
}