using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(MeshCollider)), RequireComponent(typeof(Outline))]
public class ItemController : MonoBehaviour
{
    public Rigidbody rigidbody { get; private set; }
    public MeshCollider meshCollider { get; private set; }
    public Outline outline { get; private set; }
    private Vector3 spawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        meshCollider = GetComponent<MeshCollider>();
        outline = GetComponent<Outline>();
        spawnPoint = transform.position;
        DisableOutline();
    }


    public void EnableOutline()
    {
        outline.OutlineWidth = 3;
    }

    public void DisableOutline()
    {
        outline.OutlineWidth = 0;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Death"))
        {
            transform.position = spawnPoint;
        }
    }
}
