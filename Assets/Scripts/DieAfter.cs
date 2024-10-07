using UnityEngine;

public class DieAfter : MonoBehaviour {
    public float life = 1.0f;

  // Update is called once per frame
  void Update() {
    life -= Time.deltaTime;
    if(life < 0.0f) {
      Destroy(gameObject);
    }
  }
}
