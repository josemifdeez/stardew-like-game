using UnityEngine;

[CreateAssetMenu(menuName = "Data/ToolAction/Use Food")]
public class UseFoodAction : ToolAction
{
    public override bool OnApply(Vector2 worldPoint)
    {
        Character character = GameObject.FindObjectOfType<Character>();
        if (character != null)
        {
            // Obtén el item seleccionado del ToolbarController
            ToolbarController toolbarController = GameObject.FindObjectOfType<ToolbarController>();
            if (toolbarController == null) return false;

            Item currentItem = toolbarController.GetItem;

            if (currentItem != null && currentItem.healAmount > 0)
            {
                character.Heal(currentItem.healAmount);
                return true;
            }
        }
        return false;
    }

    public override void OnItemUsed(Item usedItem, ItemContainer inventory)
    {
        // Elimina el ítem del inventario después de usarlo
        inventory.Remove(usedItem);
    }
}
