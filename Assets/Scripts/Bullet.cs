using UnityEngine;

public class Bullet : MonoBehaviour {
  public float life = 1.0f;
  public float damage = 1.0f;
  public GameObject owner;
  public GameObject BulletDiePrefab;

  // Update is called once per frame
  void Update() {
    life -= Time.deltaTime;
    if(life < 0.0f) {
      Die();
    }
  }

  public void Die() {
    Instantiate(
      BulletDiePrefab,
      transform.position,
      Quaternion.identity
    );
    Destroy(gameObject);
  }

  void OnCollisionEnter2D(Collision2D collision) { 
    if (collision.gameObject.CompareTag("PlayerBody") && owner != collision.gameObject) { 
      collision.gameObject.GetComponent<PlayerControl>().Hit(damage, owner);
      Die();
    } 
  } 
}
