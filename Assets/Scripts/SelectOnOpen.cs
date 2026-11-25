using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Put on every menu panel. As the panel opens it selects its first button,
/// so the arrow keys and Enter work at once. (The EventSystem's First Selected
/// only covers a menu that is already there when the scene starts.)
/// </summary>
public class SelectOnOpen : MonoBehaviour
{
    [SerializeField] GameObject firstButton;

    void OnEnable()
    {
        if (EventSystem.current != null && firstButton != null)
            EventSystem.current.SetSelectedGameObject(firstButton);
    }
}
