using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralCombatCard : DragObject
{
    [Header("Game Status")]
    public int turnCountDown;
    public bool onTurn = false;

    [Header("Components")]
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [Header("Cards Stats")]
    public int spd;
    public int hp;
    public int id;
    public int atk;

    private CombatController combatController;
    private Vector3 startPos;

    void Awake()
    {
        startPos = transform.position;
        locked = true;
    }

    void Start()
    {
        combatController = CombatLocator.Instance.Controller;

        combatController.NextTurn += HandleNextTurn;
    }

    void Update()
    {
        if (hp <= 0) KillSelf();
    }

    public void SetCountDown(int x)
    {
        turnCountDown = x;
    }

    private void KillSelf()
    {
        Debug.Log(gameObject.name + " hp fell under 0.");
        gameObject.SetActive(false);
    }

    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        Debug.Log("Card " + id + " took " + dmg + "damage!!!");
    }

    public void Scoot(int insertedCountDown)
    {
        if (turnCountDown >= insertedCountDown)
        {
            turnCountDown += 1;
        }
    }

    public void HandleNextTurn()
    {
        turnCountDown -= 1;

        if (turnCountDown == 0)
        {
            MyTurn();
        }
        else if (turnCountDown < 0)
        {
            Debug.Log("ERROR: Negative count down [" + turnCountDown + "] on card " + id);
        }
    }

    private void MyTurn()
    {
        Debug.Log("It's card " + id + "'s turn.");
        combatController.inTurn = true;

        _spriteRenderer.color = Color.green;
        onTurn = true;
        locked = false;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (isDragging || !onTurn) return;

        if (other.TryGetComponent<GeneralCombatCard>(out GeneralCombatCard target))
        {
            combatController.Attack(id, target.id, atk);
            turnCountDown += spd;
            combatController.ScootCards(id, turnCountDown);

            transform.position = startPos;
            onTurn = false;
            locked = true;
            _spriteRenderer.color = Color.white;
        }
    }
}
