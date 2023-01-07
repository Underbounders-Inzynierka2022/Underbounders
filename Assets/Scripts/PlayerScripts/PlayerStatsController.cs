using BarsElements;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


/// <summary>
/// Controlls player stats
/// </summary>
public class PlayerStatsController : MonoBehaviour
{
    /// <summary>
    /// Instance of player stats controller
    /// </summary>
    public static PlayerStatsController Instance;

    /// <summary>
    /// Player statistics container
    /// </summary>
    [SerializeField] private PlayerSO playerStats;
    /// <summary>
    /// Status bars ui
    /// </summary>
    [SerializeField] private UIDocument ui;
    /// <summary>
    /// Grid for translating postion from grid based to global
    /// </summary>
    [SerializeField] private Grid layout;

    /// <summary>
    /// Health status bar element
    /// </summary>
    private Bar _healthBar;
    /// <summary>
    /// Ammunition status bar element
    /// </summary>
    private Bar _secondaryBar;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        playerStats.CurrentHealth = playerStats.MaxHealth;
        playerStats.secondaryAmmo = playerStats.maxSecondaryAmmo;
        var root = ui.rootVisualElement;
        _healthBar = root.Q<Bar>("HealthBar");
        _healthBar.value = (int)Mathf.Ceil(playerStats.CurrentHealth);
        _secondaryBar = root.Q<Bar>("SecondaryBar");
        _secondaryBar.value = playerStats.secondaryAmmo;
    }
    /// <summary>
    /// Returns current player health
    /// </summary>
    /// <returns>
    /// Current player health
    /// </returns>
    public float GetHealth()
    {
        return playerStats.CurrentHealth;
    }
    /// <summary>
    /// Returns number of bombs available
    /// </summary>
    /// <returns>
    /// Current ammo level
    /// </returns>
    public int GetAmmo()
    {
        return playerStats.secondaryAmmo;
    }

    /// <summary>
    /// Heals player for one healthpoint
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if the healing try was successful, <see langword="false"/> if player already has full health
    /// </returns>
    public bool Heal()
    {
        if (playerStats.CurrentHealth < playerStats.MaxHealth)
        {
            playerStats.CurrentHealth += 1;
            _healthBar.value = (int)Mathf.Ceil(playerStats.CurrentHealth);
            return true;
        }
        return false;
    }
    /// <summary>
    /// Adds one bomb to the amount
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if piucking up the bomb was succesful, <see langword="false"/> if player has maximum number of bombs
    /// </returns>
    public bool AmmoPickUp()
    {
        if (playerStats.secondaryAmmo < playerStats.maxSecondaryAmmo)
        {
            playerStats.secondaryAmmo += 1;
            _secondaryBar.value = playerStats.secondaryAmmo;
            return true;
        }
        return false;
    }
    /// <summary>
    /// Uses one of the bombs
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if player can use bomb, <see langword="false"/> if player has no bombs available
    /// </returns>
    public bool UseSecondary()
    {
        if (playerStats.secondaryAmmo > 0)
        {
            playerStats.secondaryAmmo -= 1;
            _secondaryBar.value = playerStats.secondaryAmmo;
            return true;
        }
        return false;
    }
    /// <summary>
    /// Teleports player to desired position according to grid
    /// </summary>
    /// <param name="x">
    /// X coordinate on the grid
    /// </param>
    /// <param name="y">
    /// Y coordinate on the grid
    /// </param>
    public void SetPlayerCords(int x, int y)
    {
       var pos =  layout.CellToWorld(new Vector3Int(x + 1, -y + 1, 0));
        transform.position = pos;
    }
    /// <summary>
    /// Sets player statistics on the begining of the game with the default values
    /// </summary>
    public void SetPlayerBegginningStats()
    {
        playerStats.CurrentHealth = playerStats.MaxHealth;
        playerStats.secondaryAmmo = playerStats.maxSecondaryAmmo;
        playerStats.speed = playerStats.baseSpeed;
        playerStats.attackSpeed = playerStats.baseAttackSpeed;
        playerStats.attack = playerStats.baseAttack;
        playerStats.knocbackMultiplier = playerStats.baseKnocbackMultiplier;
        playerStats.equipment = new List<ItemSO>();
        playerStats.inventory = new List<ItemSO>();
        _healthBar.value = (int)Mathf.Ceil(playerStats.CurrentHealth);
        _secondaryBar.value = playerStats.secondaryAmmo;
    }
    /// <summary>
    /// Sets up player statistics to default beside health and bombs which comes from save file
    /// </summary>
    /// <param name="health">
    /// Number of hearths to set up
    /// </param>
    /// <param name="ammo">
    /// Number of bombs available to player
    /// </param>
    public void SetPlayerLoadedStats(float health, int ammo)
    {
        playerStats.CurrentHealth = health;
        playerStats.secondaryAmmo = ammo;
        playerStats.speed = playerStats.baseSpeed;
        playerStats.attackSpeed = playerStats.baseAttackSpeed;
        playerStats.attack = playerStats.baseAttack;
        playerStats.knocbackMultiplier = playerStats.baseKnocbackMultiplier;
        playerStats.equipment = new List<ItemSO>();
        playerStats.inventory = new List<ItemSO>();
        _healthBar.value = (int)Mathf.Ceil(playerStats.CurrentHealth);
        _secondaryBar.value = playerStats.secondaryAmmo;
    }
}
