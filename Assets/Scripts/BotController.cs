using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(SphereCollider))]
public class BotController : MonoBehaviour
{
    public Rigidbody rigidbody { get; private set; }
    private static PlayerController player;
    [SerializeField]
    GameObject particleEffect;
    private bool beenHit;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        player = FindObjectOfType<PlayerController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == player.CurrentItem.gameObject && !beenHit)
        {
            beenHit = true;
            // Remove rigidbody limits
            rigidbody.constraints = RigidbodyConstraints.None;

            // Start death timer
            StartCoroutine(Explode());
        }
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(2.0f);

        // Spawn Effect
        Instantiate(particleEffect, transform.position, transform.rotation);
        
        // Send message to player
        player.OnBotDestroyed();

        // Destroy Object
        Destroy(gameObject);
    }
}
