using UnityEngine;

public class ShellExplosion : MonoBehaviour
{
    public LayerMask tankMask;
    public ParticleSystem explosionPrefab;
    public AudioSource source;
    public float maxDamage = 100f;
    public float EplosionForce = 1000f;
    public float maxTimeLife = 2f;
    public float ExplosionRadius = 5f;
    


    private void Start()
    {
        Destroy(gameObject, maxTimeLife);
    }


    private void OnTriggerEnter(Collider other)
    {
        // Find all the tanks in an area around the shell and damage them.
        Collider[] cols = Physics.OverlapSphere(transform.position, ExplosionRadius, tankMask);

        for(int i =0; i < cols.Length;i++)
        {
            Rigidbody targetRigidbody = cols[i].GetComponent<Rigidbody>();

            if (!targetRigidbody) continue;
            targetRigidbody.AddExplosionForce(EplosionForce, transform.position, ExplosionRadius);

                TankHealth tankHealth = targetRigidbody.GetComponent<TankHealth>();

            if (!tankHealth) continue;

            float damage = CalculateDamage(targetRigidbody.position);

            tankHealth.TakeDamage(damage);
        }

        explosionPrefab.transform.parent = null;

        explosionPrefab.Play();

        source.Play();

        Destroy(explosionPrefab.gameObject, explosionPrefab.duration);

        Destroy(gameObject);
    }


    private float CalculateDamage(Vector3 targetPosition)
    {
        // Calculate the amount of damage a target should take based on it's position.
        Vector3 explosion2Target = targetPosition - transform.position;

        float explosionDistance = explosion2Target.magnitude;

        float relativeDistance = (ExplosionRadius - explosionDistance) / ExplosionRadius;


        float damage = maxDamage * relativeDistance;

        damage = Mathf.Max(0, damage);
        return damage;
    }
}