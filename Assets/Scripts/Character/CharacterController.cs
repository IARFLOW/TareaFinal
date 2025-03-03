using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterController : MonoBehaviour
{

    private Animator heroAnimator;
    private Rigidbody2D rb;
    //private GameObject weaponSprite;
    public float speed = 3f;
    public float jumpForce = 5f;
    private Vector2 direction;

    private int jumpCount = 0;
    private enum direcciones { IZQDA, DCHA, ARRIBA, ABAJO }
    private direcciones direccion;


    // Start is called before the first frame update
    void Start()
    {
        heroAnimator = this.GetComponent<Animator>();
        heroAnimator.Play("walk");
        direccion = direcciones.DCHA;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Reiniciamos el vector de dirección
        direction = new Vector2();

        // Movimiento y animación según la tecla de dirección pulsada
        if (Input.GetKey(KeyCode.A))
        {
            direccion = direcciones.IZQDA;
            heroAnimator.Play("walk");
            direction = Vector2.left;
            // Volteamos el personaje hacia la izquierda
            transform.localScale = new Vector2(-1, 1);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            direccion = direcciones.DCHA;
            heroAnimator.Play("walk");
            direction = Vector2.right;
            // Volteamos el personaje hacia la derecha
            transform.localScale = new Vector2(1, 1);
        }

        //else if (Input.GetKey(KeyCode.W))
        //{
        //    direccion = direcciones.ARRIBA;
        //    heroAnimator.Play("walk");
        //    direction = Vector2.up;
        //}
        //else if (Input.GetKey(KeyCode.S))
        //{
        //    direccion = direcciones.ABAJO;
        //    heroAnimator.Play("walk");
        //    direction = Vector2.down;
        //}
        //else if (Input.GetKey(KeyCode.S))
        //{
        //    direccion = direcciones.ABAJO;
        //    heroAnimator.Play("walk");
        //    direction = Vector2.down;
        //}
        else
        {
            heroAnimator.Play("Idle");
        }

        transform.Translate(direction.normalized * speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.W) && jumpCount <2)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpCount++;
            heroAnimator.Play("jump");  
        }

        this.transform.rotation = Quaternion.identity;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag ("Ground"))
        {
            jumpCount = 0;
        }
    }
    //public void KillPlayer()
    //{
    //    // Reinicia la escena actual
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    //}

}

// Método para instanciar (disparar) el arma
//    void fire()
//    {
//        gameobject throwableweapon = instantiate(
//            weaponsprite,
//            transform.position,
//            quaternion.identity
//        ) as gameobject;

//        throwableweapon.getcomponent<rigidbody2d>().velocity =
//            direction.normalized * weaponspeed;
//    }


