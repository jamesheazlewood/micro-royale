using UnityEngine;

public class Bob : MonoBehaviour {
	public float scaleSpeed = 2.0f;
	public float scaleAmount = 1.0f;
	public float rotSpeed = 2.0f;
	public float rotAmount = 1.0f;
	public float ySpeed = 0.0f;
	public float yAmount = 0.0f;
	public float xSpeed = 0.0f;
	public float xAmount = 0.0f;
	public bool rotContinue = false;
	public float startTime = 0;
	public Vector3 initialScale;
	public Vector3 initialPos;
	public float time;

	// Use this for initialization
	void Awake() {
		initialScale = transform.localScale;
		initialPos = transform.localPosition;
	}

	// Use this for initialization
	void Start() {
		time = startTime;
	}

	// Update is called once per frame
	void Update() {
    time += Time.deltaTime;

    // Scale
    if(scaleAmount != 0) {
      float scale = Mathf.Sin(time * scaleSpeed) * scaleAmount;
      transform.localScale = initialScale + new Vector3(scale, scale, scale);
    }

    // Rotate
    if(rotAmount != 0) {
      float angle = transform.eulerAngles.z;
      if(rotContinue) {
        angle += Time.deltaTime * rotSpeed * rotAmount;
      } else {
        angle = Mathf.Sin(time * rotSpeed) * rotAmount;
      }
      transform.eulerAngles = new Vector3(0, 0, angle);
    }

    // Position
    if(xAmount != 0 && xSpeed != 0) {
      transform.localPosition = new Vector3(
        initialPos.x + Mathf.Sin(time * xSpeed) * xAmount,
        transform.localPosition.y,
        transform.localPosition.z
      );
    }
    if(yAmount != 0 && ySpeed != 0) {
      transform.localPosition = new Vector3(
        transform.localPosition.x,
        initialPos.y + Mathf.Sin(time * ySpeed) * yAmount,
        transform.localPosition.z
      );
    }
	}
}
