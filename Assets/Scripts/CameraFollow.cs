using UnityEngine;

public class CameraFollow : MonoBehaviour {
  public GameObject target;

  // Start is called before the first frame update
  void Start() {
      
  }

  // Update is called once per frame
  void FixedUpdate() {
    gameObject.transform.position = new Vector3(
      gameObject.transform.position.x + (target.transform.position.x - gameObject.transform.position.x) * 0.25f,
      gameObject.transform.position.y + (target.transform.position.y - gameObject.transform.position.y) * 0.25f,
      gameObject.transform.position.z
    );
  }
}
