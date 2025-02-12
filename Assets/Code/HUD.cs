using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
    public enum InfoType {
        Exp,
        Level,
        Kill,
        Time,
        Health
    }

    public InfoType type;

    private Text m_myText;
    private Slider m_mySlider;

    private void Awake() {
        m_myText = GetComponent<Text>();
        m_mySlider = GetComponent<Slider>();
    }

    private void LateUpdate() {
        switch (type) {
            case InfoType.Exp:
                float curExp = GameManager.instance.exp;
                float maxExp = GameManager.instance.nextExp[Mathf.Min(GameManager.instance.level, GameManager.instance.nextExp.Length - 1)];
                m_mySlider.value = curExp / maxExp;
                break;
            case InfoType.Level:
                m_myText.text = string.Format("Lv.{0:F0}", GameManager.instance.level);
                break;
            case InfoType.Kill:
                m_myText.text = string.Format("{0:F0}", GameManager.instance.kill);
                break;
            case InfoType.Time:
                float remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;
                int min = Mathf.FloorToInt(remainTime / 60);
                int sec = Mathf.FloorToInt(remainTime % 60);
                m_myText.text = string.Format("{0:D2}:{1:D2}", min, sec);
                break;
            case InfoType.Health:
                float curHealth = GameManager.instance.health;
                float maxHealth = GameManager.instance.maxHealth;
                m_mySlider.value = curHealth / maxHealth;
                break;
        }
    }
}