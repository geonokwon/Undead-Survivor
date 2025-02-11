using UnityEngine;

public class FollowHealth : MonoBehaviour {
    private RectTransform rect;

    void Awake() {
        rect = GetComponent<RectTransform>();
    }

    void FixedUpdate() {
        rect.position = Camera.main.WorldToScreenPoint(GameManager.instance.player.transform.position);
    }
}