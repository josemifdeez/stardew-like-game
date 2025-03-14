using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class ChaseEnemy : MonoBehaviour
{
    Transform player;
    [SerializeField] float speed;
    [SerializeField] Vector2 attackSize = Vector2.one;
    [SerializeField] int damage = 1;
    [SerializeField] float timeToAttack = 2f;
    float attackTimer;
    Vector3 originalScale;
    Animator animator;
    public bool animable;

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.player.transform;
        attackTimer = Random.Range(0, timeToAttack);
        originalScale = transform.localScale; // Guardar la escala original
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Calcular la direcci�n hacia el jugador
        Vector3 direction = (player.position - transform.position).normalized;

        // Mover al enemigo hacia el jugador
        transform.position = Vector3.MoveTowards(
            transform.position,
            player.position,
            speed * Time.deltaTime
        );

        if (animable)
        {
            animator.SetFloat("horizontal", direction.x);
            animator.SetFloat("vertical", direction.y);
        }
        else
        {
            if (direction.x > 0) // Si el jugador est� a la derecha
            {
                transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
            }
            else if (direction.x < 0) // Si el jugador est� a la izquierda
            {
                transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
            }
        }

        Attack();
    }

    private void Attack()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer > 0f) { return; }

        attackTimer = timeToAttack;

        Collider2D[] targets = Physics2D.OverlapBoxAll(transform.position, attackSize, 0f);

        for (int i = 0; i < targets.Length; i++)
        {
            // Verificar si el objetivo es el jugador
            if (targets[i].gameObject.CompareTag("Player"))
            {
                Damageable character = targets[i].GetComponent<Damageable>();
                if (character != null)
                {
                    character.TakeDamage(damage);
                }
            }
        }
    }

    // Para depurar la caja de ataque en la escena de Unity
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, attackSize);
    }
}
