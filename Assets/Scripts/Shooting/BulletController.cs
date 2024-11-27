using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float damage = 25f;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private GameObject hitEffect;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<ITargetable>(out var target))
        {
            target.TakeDamage(damage);
            if (hitEffect != null)
            {
                Instantiate(hitEffect, collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal));
            }
        }
        Destroy(gameObject);
    }
}
