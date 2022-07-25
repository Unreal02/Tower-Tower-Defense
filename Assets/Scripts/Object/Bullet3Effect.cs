using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet3Effect : MonoBehaviour
{
    public float life;
    private MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        Destroy(gameObject, life);
    }

    // Update is called once per frame
    void Update()
    {
        Color color = meshRenderer.material.color;
        color.a -= Time.deltaTime / life;
        meshRenderer.material.color = color;
    }
}
