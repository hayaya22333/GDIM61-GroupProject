using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    public float popDuration = 1.5f;

    [SerializeField] TextMeshPro textComponent;

    public float upwardSpeed = 2f;
    public float fallSpeed = 0f;
    public float gravity = 5f;

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

        // lifetime
        popDuration -= Time.deltaTime;

        float t = popDuration / 1f; // 1f = lifetime duration

        Color c = textComponent.color;
        c.a = Mathf.Clamp01(t);
        textComponent.color = c;
        if (popDuration <= 0)
        {
            Destroy(gameObject);
        }
    }
}