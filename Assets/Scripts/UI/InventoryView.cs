using UnityEngine;
using UnityEngine.UI;

public class InventoryView : GameComponent
{
    [SerializeField] private Button _swapButton;
    [SerializeField] private Image _equipedWeaponImage;
    [SerializeField] private Image _nextWeaponImage;
    [SerializeField] Weapon _weaponToAdd;
    [SerializeField] Inventory _inventory;

    protected override void OnStart()
    {
        _inventory.onChangedWeapon.AddListener(OnWeaponChanged);
        _inventory.onChangedInventory.AddListener(OnInventoryChanged);
        _swapButton.onClick.AddListener(() => _inventory.ChangeWeapon());
    }

    public void OnWeaponChanged(Weapon weapon)
    {
        UpdateImages();
    }

    public void OnInventoryChanged()
    {
        if (_inventory.WeaponsCount == 0)
        {
            _swapButton.gameObject.SetActive(false);
            _nextWeaponImage.enabled = false;
        }
        else
        {
            _swapButton.gameObject.SetActive(true);
            _nextWeaponImage.enabled = true;
        }
        UpdateImages();
    }

    private void UpdateImages()
    {
        if(_inventory.EquipedWeapon is not null)
            _equipedWeaponImage.sprite = _inventory.EquipedWeapon.Image;
        if(_inventory.WeaponsCount > 0 && _inventory.TopWeapon is not null)
            _nextWeaponImage.sprite = _inventory.TopWeapon.Image;
    }

    protected override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _inventory.ChangeWeapon();
        }
    }
}