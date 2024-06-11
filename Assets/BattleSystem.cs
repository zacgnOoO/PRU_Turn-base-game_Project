using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    Unit playerUnit;
    Unit enemyUnit;

    public Text dialogueText;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public BattleState state;

    public GameObject enemyAttackPrefab; // Prefab cho hoạt ảnh tấn công của kẻ thù
    public GameObject playerHealPrefab;  // Prefab cho hoạt ảnh hồi máu của người chơi
    public GameObject playerAttackPrefab; // Prefab cho hoạt ảnh tấn công của người chơi

    private GameObject enemyGO; // Biến toàn cục để lưu trữ kẻ thù

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<Unit>();

        enemyGO = Instantiate(enemyPrefab, enemyBattleStation); // Lưu trữ kẻ thù vào biến toàn cục
        enemyUnit = enemyGO.GetComponent<Unit>();

        dialogueText.text = "A wild " + enemyUnit.unitName + " approaches...";

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {
        // Hiển thị hoạt ảnh tấn công của người chơi
        GameObject attackAnimation = Instantiate(playerAttackPrefab, playerBattleStation.position, Quaternion.identity);

        // Optionally, you can set the animation to destroy itself after playing
        Destroy(attackAnimation, 1f); // Adjust the time based on your animation length

        // Wait for the animation to finish
        yield return new WaitForSeconds(1f); // Adjust the time to match the length of your animation

        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);

        enemyHUD.SetHP(enemyUnit.currentHP);
        dialogueText.text = "The attack is successful!";

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {
        dialogueText.text = enemyUnit.unitName + " attacks!";

        // Ẩn kẻ thù bằng cách tắt GameObject của nó
        enemyGO.SetActive(false);

        // Instantiate the enemy attack animation at the enemy's position
        GameObject attackAnimation = Instantiate(enemyAttackPrefab, enemyBattleStation.position, Quaternion.identity);

        // Optionally, you can set the animation to destroy itself after playing
        Destroy(attackAnimation, 4f); // Adjust the time based on your animation length

        // Wait for the animation to finish
        yield return new WaitForSeconds(4f); // Adjust the time to match the length of your animation

        // Perform the attack logic
        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);

        playerHUD.SetHP(playerUnit.currentHP);

        // Show the enemy again after the attack animation finishes
        enemyGO.SetActive(true);

        // Wait for a bit more before continuing
        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            dialogueText.text = "You won the battle!";
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "You were defeated.";
        }
    }

    void PlayerTurn()
    {
        dialogueText.text = "Choose an action:";
    }

    IEnumerator PlayerHeal()
    {
        // Hiển thị hoạt ảnh hồi máu
        GameObject healAnimation = Instantiate(playerHealPrefab, playerBattleStation.position, Quaternion.identity);

        // Optionally, you can set the animation to destroy itself after playing
        Destroy(healAnimation, 2f); // Adjust the time based on your animation length

        // Thực hiện hồi máu
        playerUnit.Heal(20);
        playerHUD.SetHP(playerUnit.currentHP);
        dialogueText.text = "You feel renewed strength!";

        yield return new WaitForSeconds(2f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerAttack());
    }

    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(PlayerHeal());
    }
}
