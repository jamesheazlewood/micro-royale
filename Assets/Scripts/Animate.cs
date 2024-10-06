using UnityEngine;

public class Animate : MonoBehaviour {
	public float timer;
	public float delay = 0.08f;
	public Sprite[] frames;
	public int frame;
  public SpriteRenderer spriteRenderer;

  void Awake() {
    spriteRenderer = GetComponent<SpriteRenderer>();
  }

	// Update is called once per frame
	void Update() {
		timer += Time.deltaTime;

    if(timer > delay) {
      if(frame != frames.Length - 1) {
        SetFrame(frame + 1);
      } else {
        SetFrame(0);
      }
    }
	}

	// 
	public void SetFrame(int newFrame) {
		timer = 0;
		frame = newFrame;
		spriteRenderer.sprite = frames[newFrame];
	}
}
