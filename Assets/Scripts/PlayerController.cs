using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

		private bool dead = false;
		private Rigidbody2D rb2d;
		private bool faceRight;
		private bool restart;
		private bool jump;
		public GameObject playerExplosion;
		private bool gameover;
		[SerializeField]
		private float jumpForce = 100f;
		public float speed = 15f;
	    
		private Text endScreenText;
		private GameObject levelImage;
		private int health = 75;
		private int enemyDamage = 25;
		private int count = 0;
		public Text CountText;
		//public Text GameOverText;
		//public Text RestartText;
		public float gravity = 9.8f;

		[SerializeField]
		private Transform[] groundPoints;

		[SerializeField]
		private float groundRaduis;

		[SerializeField]
		private LayerMask Wground;
		private bool isGrounded;


		// Use this for initialization
		void Start ()
		{
			restart = false;
			faceRight = true;
			rb2d = GetComponent<Rigidbody2D> ();	
			count = 0;
			SetCountText ();
			//endScreenText = GameObject.Find ("EndScreenText").GetComponent<Text>();
			//levelImage = GameObject.Find ("LevelImage");
			//SetCountText ();
			//.SetActive(false);
			//GameOverText.text = "";
		}

		// Update is called once per frame
		void FixedUpdate ()
		{			
			float horizontal = Input.GetAxis ("Horizontal");
			float vertical = Input.GetAxis ("Vertical");
			if (dead == false) {
				rb2d.velocity = new Vector2 (horizontal * speed, rb2d.velocity.y);
			}	

			flip (horizontal);
			isGrounded = isGround();
			if(Input.GetMouseButtonDown(0))
			{
				jump = true;
			}
			if (jump && isGrounded) 
			{
				jump = false;
				isGrounded = false;
				rb2d.AddForce (new Vector2 (0f, jumpForce));

			}

    }
		void Update()
		{

			if (restart)
			{
				if (Input.GetKeyDown (KeyCode.R))
				{
					Application.LoadLevel (Application.loadedLevel);
				}
			}	
			if (gameover == true)
			{
				if (Input.GetKeyDown (KeyCode.R))
				{
					Application.LoadLevel (Application.loadedLevel);
				}
			}

        // 5 - Shooting
        bool shoot = Input.GetButtonDown("Fire1");
        shoot |= Input.GetButtonDown("Fire2");
        // Careful: For Mac users, ctrl + arrow is a bad idea

        if (shoot)
        {
            WeaponScript weapon = GetComponent<WeaponScript>();
            if (weapon != null)
            {
                // false because the player is not an enemy
                weapon.Attack(false);
            }
        }
    }

		void OnGUI() {
			if (GUI.Button (new Rect (20, 20, 100, 50), "Menu")) {
				Application.LoadLevel (0);
			}
		}
		private void flip(float horizontal)
		{
			if (horizontal > 0 && !faceRight || horizontal <0 && faceRight)
			{
				faceRight = !faceRight;
				Vector3 theScale = transform.localScale;
				theScale.x *= -1;
				transform.localScale = theScale;
			}
		}
		void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag ("feather")) {
			other.gameObject.SetActive (false);
			count = count + 1;
			SetCountText ();

		}
		if (other.gameObject.tag == "Enemy") {

			//Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
			//GameOverText.text = "You Died!";
			//RestartText.text = "Press R to Restart Game!";
			Destroy (other.gameObject);
			//Destroy(this.gameObject);
			health -= enemyDamage;
			GameOver ();

			//speed = 0;
			//dead = true;
			//restart = true;
			//Invoke ("Restart", 5f);
			//Application.LoadLevel ("MainMenu");
		}
	}

	void Restart()
	{
		GameOver ();
	}

		void SetCountText ()
		{
		CountText.text = "Feathers Collected: " + count.ToString ();
			if (count >= 10)
			{
			Application.LoadLevel (3);
			}
		}

		private bool isGround()
		{
			if (rb2d.velocity.y <= 0)
			{
				foreach (Transform point in groundPoints)
				{
					Collider2D[] colliders = Physics2D.OverlapCircleAll (point.position, groundRaduis, Wground);
					for (int i = 0; i < colliders.Length; i++)
					{
						if (colliders[i].gameObject != gameObject)
						{
							return true;

						}
					}
				}

			}
			return false;
		}

	void GameOver()
	{
		if (health <= 0 || gameover == true) 
		{

			//levelImage.SetActive(true);
			Application.LoadLevel("EndingScreen");
		}

	}
    void OnCollisionEnter2D(Collision2D collision)
    {
        bool damagePlayer = false;

        // Collision with enemy
        EnemyScript enemy = collision.gameObject.GetComponent<EnemyScript>();
        if (enemy != null)
        {
            // Kill the enemy
            HealthScript enemyHealth = enemy.GetComponent<HealthScript>();
            if (enemyHealth != null) enemyHealth.Damage(enemyHealth.hp);

            damagePlayer = true;
        }

        // Damage the player
        if (damagePlayer)
        {
            HealthScript playerHealth = this.GetComponent<HealthScript>();
            if (playerHealth != null) playerHealth.Damage(1);
        }
    }
}

