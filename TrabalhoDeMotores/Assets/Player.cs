using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int pontos = 0;

    public float velocidade = 10f;
    public float forcaPulo = 10f;
    public float forcaMola = 15f; // Força da mola

    public bool noChao = false;
    public bool andando = false;
    public bool impulso = false;

    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _animator = gameObject.GetComponent<Animator>();
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "chao")
        {
            noChao = true;
        }

        // Se o jogador pisar em um objeto com a tag "pulapula", ele será lançado para cima automaticamente
        if (collision.gameObject.tag == "pulapula")
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0); // Zera a velocidade vertical
            _rigidbody2D.AddForce(Vector2.up * forcaMola, ForceMode2D.Impulse);
            impulso = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "chao")
        {
            noChao = false;
        }

        if (collision.gameObject.tag == "pulapula")
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0); // Zera a velocidade vertical
            _rigidbody2D.AddForce(Vector2.up * forcaMola, ForceMode2D.Impulse);
            impulso = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        andando = false;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            gameObject.transform.position += new Vector3(-velocidade * Time.deltaTime, 0, 0);
            _spriteRenderer.flipX = true;
            Debug.Log("LeftArrow");

            if (noChao == true)
            {
                andando = true;
            }
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            gameObject.transform.position += new Vector3(velocidade * Time.deltaTime, 0, 0);
            _spriteRenderer.flipX = false;
            Debug.Log("RightArrow");

            if (noChao == true)
            {
                andando = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && noChao == true)
        {
            _rigidbody2D.AddForce(new Vector2(0, 1) * forcaPulo, ForceMode2D.Impulse);
            Debug.Log("Jump");
        }
        
        _animator.SetBool("Andando", andando);
        _animator.SetBool("Caindo", !noChao);
        _animator.SetBool("Impulso", impulso);
    }
}
