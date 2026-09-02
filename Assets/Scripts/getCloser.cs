using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class EnemyAI2 : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;

    public int damage = 10;
    public float attackCooldown = 1f;
    private float lastAttackTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Ensure Rigidbody is kinematic so NavMeshAgent can move enemy
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        // Ensure collider is not trigger (we want real collision)
        Collider col = GetComponent<Collider>();
        col.isTrigger = false;
    }

    void Update()
    {
        if (player != null)
        {
            agent.SetDestination(player.position);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TryDamage(collision.gameObject);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TryDamage(collision.gameObject);
        }
    }

    private void TryDamage(GameObject playerObj)
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            PlayerHealth health = playerObj.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
                Debug.Log("Enemy hit player for " + damage);
            }
            lastAttackTime = Time.time;
        }
    }
}