using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    private float popDuration = 0.8f;

    [SerializeField] TextMeshPro textComponent;

    private float upwardSpeed = 5f;
    private float fallSpeed = 0f;
    private float gravity = 20f;

    public void PopDamage(int dmg)
    {
        fallSpeed = upwardSpeed;
        if (dmg > 0)
        {
            textComponent.color = Color.red;
            textComponent.text = "-" + dmg;
        }
        else if (dmg < 0)
        {
            dmg *= -1;
            textComponent.color = Color.green;
            textComponent.text = "+" + dmg;
        }
        else
        {
            textComponent.color = Color.yellow;
            textComponent.text = "Blocked";
        }
    }

    void Update()
    {
        // move upward then simulate fall
        fallSpeed -= gravity * Time.deltaTime;

        transform.position += new Vector3(0, fallSpeed * Time.deltaTime, 0);

        popDuration -= Time.deltaTime;
        float t = popDuration / 1f;

        Color c = textComponent.color;
        c.a = Mathf.Clamp01(t);
        textComponent.color = c;
        if (popDuration <= 0)
        {
            Destroy(gameObject);
        }
    }
}