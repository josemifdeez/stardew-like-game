//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ChaseEnemy : MonoBehaviour
//{
//    Transform player;
//    [SerializeField] float speed;
//    [SerializeField] Vector2 attackSize = Vector2.one;
//    [SerializeField] int damage = 1;
//    [SerializeField] float timeToAttack = 2f;
//    float attackTimer;
//    Vector3 originalScale;

//    // Start is called before the first frame update
//    void Start()
//    {
//        player = GameManager.instance.player.transform;
//        attackTimer = Random.Range(0, timeToAttack);
//        originalScale = transform.localScale; // Guardar la escala original
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        // Calcular la dirección hacia el jugador
//        Vector3 direction = (player.position - transform.position).normalized;

//        // Mover al enemigo hacia el jugador
//        transform.position = Vector3.MoveTowards(
//            transform.position,
//            player.position,
//            speed * Time.deltaTime
//        );

//        // Cambiar la rotación en el eje Y para mirar al jugador
//        if (direction.x > 0) // Si el jugador está a la derecha
//        {
//            transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
//        }
//        else if (direction.x < 0) // Si el jugador está a la izquierda
//        {
//            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
//        }

//        Attack();
//    }

//    private void Attack()
//    {
//        attackTimer -= Time.deltaTime;

//        if (attackTimer > 0f) { return; }

//        attackTimer = timeToAttack;

//        Collider2D[] targets = Physics2D.OverlapBoxAll(transform.position, attackSize, 0f);

//        for (int i = 0; i < targets.Length; i++)
//        {
//            // Verificar si el objetivo no es el propio enemigo
//            if (targets[i].gameObject != gameObject)
//            {
//                Damageable character = targets[i].GetComponent<Damageable>();
//                if (character != null)
//                {
//                    character.TakeDamage(damage);
//                }
//            }
//        }
//    }

//    // Para depurar la caja de ataque en la escena de Unity
//    private void OnDrawGizmosSelected()
//    {
//        Gizmos.color = Color.red;
//        Gizmos.DrawWireCube(transform.position, attackSize);
//    }
//}
