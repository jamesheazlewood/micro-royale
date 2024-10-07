using TMPro;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
  // Public settings
  public bool playerControlled = false;

  // Drag and drop
  public Rigidbody2D phys;
  public DNA dna;
  public TMP_Text dnaText;
  public GameObject bulletPrefab;
  public GameObject dnaBubblePrefab;
  public GameObject playerDeathPrefab;
  public GameObject jetObj;
  public GameObject bodyCircle;
  public GameObject bodyOval;
  public GameObject bodyDiamond;
  public GameObject bodyPill;
  public ParticleSystem jetParticles;
  public GameObject[] guns; // 0 = middle, 1 = left, 2 = right
  public ParticleSystem[] muzzleParticles; // 0 = middle, 1 = left, 2 = right
  public GameObject[] spikes; // = 0 top left first and bottom right last

  // Stats
  public float ammo = 0;
  public float food = 1.0f;
  public float health = 100.0f;
  public bool isAlive = true;
  public float gunCooldown = 0.10f;
  public float bulletSpread = 1.00f;
  public float bulletSpeed = 1000.00f;
  public float bulletDamage = 1.00f;
  public int bulletsPerShot = 1;
  public float gunCurrentCooldown = 1.00f;

  // AI
  public float aiThinkDelay = 3.0f;
  public float aiTimer = 0;
  public float aiAngle = 0;
  public bool aimoving = false;

  // Internal
  public GameObject lastHitBy;
  public GameObject currentBody;
  
  ParticleSystem.EmissionModule jetParticlesEmission;
  Vector2 movement;
  Vector2 facingDirection; // Facing direction 2D vector
  Vector3 originalScale;
  public float acceleration;
  public float jetAcceleration;
  public bool isBoostDown = false;
  public bool isBoosting = false;
  public bool isShootDown = false;
  public bool isShooting = false;

  // Start is called before the first frame update
  void Start() {
    originalScale = transform.localScale;
    dna = new DNA();
    dna.Shuffle();
    LoadDNA();

    if(playerControlled) {
      dnaText.text = dna.ToColoredString();
    } else {
      aiThinkDelay = Random.Range(2.0f, 4.0f);
    }

  }

  void LoadDNA() {
    acceleration = dna.GetAcceleration();
    jetAcceleration = dna.GetJetAcceleration();
    jetParticlesEmission = jetParticles.emission;
    jetParticlesEmission.enabled = false;

    switch(dna.gunBullets) {
      case Dit.C: bulletsPerShot = 2; break;
      case Dit.A: bulletsPerShot = 3; break;
      case Dit.T: bulletsPerShot = 4; break;
      default: bulletsPerShot = 1; break;
    }

    bulletSpeed = 100.0f + ((float)dna.gunBullets * 100f);

    bodyCircle.SetActive(false);
    bodyOval.SetActive(false);
    bodyDiamond.SetActive(false);
    bodyPill.SetActive(false);

    switch(dna.bodyShape) {
      case Dit.C: currentBody = bodyCircle; break;
      case Dit.A: currentBody = bodyDiamond; break;
      case Dit.T: currentBody = bodyPill; break;
      default: currentBody = bodyOval; break;
    }

    currentBody.SetActive(true);

    for(var i = 0; i < 3; i++) {
      guns[i].SetActive(false);
    }

    switch(dna.gunCount) {
      case Dit.A:
        guns[1].SetActive(true);
        guns[2].SetActive(true);
        break;
      case Dit.T:
        guns[0].SetActive(true);
        guns[1].SetActive(true);
        guns[2].SetActive(true);
        break;
      default: // C
        guns[0].SetActive(true);
        break;
    }

    Color c = dna.GetColor();
    foreach(Transform g in currentBody.transform) {
      if(g.CompareTag("Spike") || g.CompareTag("PlayerBody")) {
        g.GetComponent<SpriteRenderer>().color = c;
      }
    }
    foreach(Transform g in transform) {
      if(g.CompareTag("PlayerBody")) {
        g.GetComponent<SpriteRenderer>().color = c;
      }
    }

    float s = ((float)dna.size + 1.0f) * 0.333f;
    transform.localScale = originalScale + new Vector3(s, s, s);
  }

  // Update is called once per frame
  void Update() {
    if(playerControlled) {
      // Capture player input
      movement.x = Input.GetAxisRaw("Horizontal");  // A and D for left and right
      movement.y = Input.GetAxisRaw("Vertical");    // W and S for up and down

      // Get the mouse position in screen space and convert it to world space
      Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      mousePos.z = 0;

      // Calculate the direction from the player to the mouse position
      // c.x + (t.x - c.x) * 0.25f,
      Vector3 t = (mousePos - new Vector3(transform.position.x, transform.position.y, 0)).normalized;
      facingDirection = new Vector2(
        facingDirection.x + (t.x - facingDirection.x) * 0.125f,
        facingDirection.y + (t.y - facingDirection.y) * 0.125f
      );

      if(Input.GetMouseButton(1)) {
        isBoostDown = true; 
      } else {
        isBoostDown = false; 
      }

      if (Input.GetMouseButton(0)) {
        isShootDown = true; 
      } else {
        isShootDown = false; 
      }

      if (Input.GetKeyDown(KeyCode.Space)) {
        dna.Shuffle();
        LoadDNA();
        dnaText.text = dna.ToColoredString();
      }

    } else {
      // Run AI ticker
      aiTimer += Time.deltaTime;
      if(aiTimer > 3.0f) {
        aiTimer = Random.Range(0.0f, 2.0f);
        aiAngle = Random.Range(0.0f, Mathf.PI * 2);
        aimoving = Random.Range(0.0f, 10.0f) < 8.0f;
        isBoostDown = Random.Range(0.0f, 10.0f) > 8.0f;
        isShootDown = Random.Range(0.0f, 10.0f) > 8.0f;
      }

      Vector3 t = new Vector2(Mathf.Sin(aiAngle), Mathf.Cos(aiAngle));
      facingDirection = new Vector2(
        facingDirection.x + (t.x - facingDirection.x) * 0.025f,
        facingDirection.y + (t.y - facingDirection.y) * 0.025f
      );

      movement.x = t.x;
      movement.y = t.y;
    }

    phys.AddForce(movement * acceleration * Time.deltaTime);

    if(isBoostDown) {
      phys.AddForce(facingDirection * jetAcceleration * Time.deltaTime);
      if(!isBoosting) {
        jetParticlesEmission.enabled = true;
        isBoosting = true;
      }
    } else {
      if(isBoosting) {
        jetParticlesEmission.enabled = false;
        isBoosting = false;
      }
    }

    // Calculate the angle in radians and convert to degrees
    float angle = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg;

    // Apply the rotation (we rotate around the Z axis, -90 is to adjust for the sprite's orientation)
    transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));

    gunCurrentCooldown -= Time.deltaTime;

    if(isShootDown && (int)dna.gunCount > 0) {
      if(gunCurrentCooldown < 0.0f) {
        gunCurrentCooldown = gunCooldown;

        for(var i = 0; i < (int)dna.gunCount; i++) {
          muzzleParticles[i].Clear();
          muzzleParticles[i].Play();
          for(var j = 0; j < bulletsPerShot; j++) {
            GameObject b = Instantiate(
              bulletPrefab,
              transform.position + new Vector3(
                Random.Range(-0.1f, 0.1f),
                Random.Range(-0.1f, 0.1f),
                0
              ) + new Vector3(facingDirection.x, facingDirection.y, 0) * 1.5f,
              transform.rotation
            );
            b.GetComponent<Bullet>().damage = bulletDamage;
            b.GetComponent<Bullet>().owner = gameObject;
            b.GetComponent<Rigidbody2D>().AddForce(b.transform.up * bulletSpeed);
          }
        }
        phys.AddForce(
          new Vector2(-facingDirection.x, -facingDirection.y) * jetAcceleration * bulletsPerShot * Time.deltaTime
        );
      }
    }
  }

  public void Hit(float damage, GameObject owner) {
    lastHitBy = owner;
    health -= damage;
    if(health < 0) {
      isAlive = false;
      GameObject orb = Instantiate(
        dnaBubblePrefab,
        transform.position,
        Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360f)))
      );
      Instantiate(
        playerDeathPrefab,
        transform.position,
        Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360f)))
      );
    }
  }
}
