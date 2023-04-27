using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    [SerializeField] int damage;
    
    [SerializeField] float moveSpeed; //remove later?

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Health>(out var damageTaker))
        {
            damageTaker.TakeDamage(damage);
        }
    }

    //movement will be done differently in another script later, this is temporary
    private void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        GetComponent<Rigidbody2D>().MovePosition((Vector2)transform.position + new Vector2(moveX, moveY).normalized * moveSpeed);
    }
}
