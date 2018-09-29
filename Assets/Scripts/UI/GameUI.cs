using Enums;
using Player;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField]
    private PlayerController _player;

    [SerializeField]
    private Image _playerAvatar;

    [SerializeField]
    private Sprite _walkSprite;

    [SerializeField]
    private Sprite _crouchSprite;

    [SerializeField]
    private Sprite _runSprite;

    [SerializeField]
    private Slider _healthSlider;

    [SerializeField]
    private Text _healthText;

    [SerializeField]
    private Image _activeItemSprite;

    [SerializeField]
    private Text _activeItemName;

    [SerializeField]
    private AttackBehavior _attackBehaviour;

    [SerializeField]
    private Image _primaryWeaponSprite;

    [SerializeField]
    private Image _secondaryWeaponSprite;

    [SerializeField]
    private Text _ammoCount;

    [SerializeField]
    private Text _scoreText;

    private Dictionary<MovementMode, Sprite> _playerSprites;

	// Use this for initialization
	void Start ()
    {
        _playerSprites = new Dictionary<MovementMode, Sprite>
        {
            { MovementMode.Sneak, _crouchSprite },
            { MovementMode.Walk, _walkSprite },
            { MovementMode.Run, _runSprite }
        };

        _playerAvatar.sprite = _walkSprite;
        _player.MovementModeChanged += OnMovementModeChange;

        _healthSlider.maxValue = _player.MaxHitPoints;
        _healthSlider.value = _player.HitPoints;
        _healthText.text = $"{_player.HitPoints}/{_player.MaxHitPoints}";
        _player.HealthChanged += OnPlayerHealthChanged;
        _player.Deadened += OnPlayerDeath;

        _ammoCount.text = _attackBehaviour.Ammo.ToString();
        if (!_attackBehaviour.IsUsingCrossbow) HideAmmo();
        _attackBehaviour.AmmoChanged += OnAmmoChanged;
        _attackBehaviour.WeaponSwapped += OnWeaponSwapped;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnWeaponSwapped(bool isCrossbow)
    {
        if (isCrossbow) ShowAmmo();
        else HideAmmo();
    }

    private void OnAmmoChanged(int count)
    {
        _ammoCount.text = count.ToString();
    }

    private void OnPlayerDeath()
    {
        // play sad trombone noise or something i dunno
    }

    private void OnPlayerHealthChanged(float currentHealth, float change)
    {
        _healthSlider.value = currentHealth;
        _healthText.text = $"{Mathf.RoundToInt(currentHealth)}/{_player.MaxHitPoints}";
    }

    private void OnMovementModeChange(MovementMode mode)
    {
        _playerAvatar.sprite = _playerSprites[mode];
    }

    private void ShowAmmo()
    {
        _ammoCount.CrossFadeAlpha(1, 0.1f, false);
    }

    private void HideAmmo()
    {
        _ammoCount.CrossFadeAlpha(0, 0.1f, false);
    }
}
