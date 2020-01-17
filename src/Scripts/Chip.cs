using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chip : MonoBehaviour
{
    public int owner, row, column;
    public Color color;

    void Start()
    {
        GetComponent<SpriteRenderer>().color = color;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject.GetComponent<Rigidbody2D>());
    }
}
