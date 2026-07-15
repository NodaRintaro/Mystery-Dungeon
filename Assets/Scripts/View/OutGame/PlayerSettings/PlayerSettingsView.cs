using UnityEngine;

namespace View.OutGame.PlayerSettings
{
    public class PlayerSettingsView : MonoBehaviour
    {
        [SerializeField] private NameInputView _nameInputView;
        [SerializeField] private GenderSelectView _genderSelectView;
        [SerializeField] private JobSelectView _jobSelectView;
    }
}
