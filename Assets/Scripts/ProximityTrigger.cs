using UnityEngine;

public class ProximityTrigger : MonoBehaviour
{
    public GameObject Target;
    public float range;
    public GameObject explosion;

    private bool wasInRange = false;

    void Update()
    {
        Vector3 direction = Target.transform.position - transform.position;
        float distance = direction.magnitude;

        bool isInRange = distance <= range;

        // Trigger ONLY when entering range
        if (isInRange && !wasInRange)
        {

            explosion.SetActive(false);

        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}