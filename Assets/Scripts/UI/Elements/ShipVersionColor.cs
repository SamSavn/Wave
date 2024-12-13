using UnityEngine;
using UnityEngine.UI;
using Wave.Settings;

namespace Wave.UI
{
    public class ShipVersionColor : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _primaryColor;
        [SerializeField] private Image _secondaryColor;

        public void SetUp(ShipVersion ship)
        {
            _primaryColor.color = ship.GetPrimaryColor();
            _secondaryColor.color = ship.GetSecondaryColor();
        }
    } 
}
