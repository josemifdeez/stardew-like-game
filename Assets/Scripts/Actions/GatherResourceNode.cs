using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceNodeType
{
    Undefined,
    Tree,
    Ore
}

[CreateAssetMenu(menuName = "Data/ToolAction/Gather Resource Node")]
public class GatherResourceNode : ToolAction
{
    [SerializeField] float sizeOfInteractableArea = 1f;
    [SerializeField] List<ResourceNodeType> canHitNodesOfType;

    [SerializeField] string axeAnimationTrigger = "axe"; // Configura esto en el inspector
    [SerializeField] string pickaxeAnimationTrigger = "pick"; // Configura esto en el inspector

    public override bool OnApply(Vector2 worldPoint)
    {
        // Set the default animation trigger based on the node type
        SetAnimationTrigger();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(worldPoint, sizeOfInteractableArea);

        foreach (Collider2D c in colliders)
        {
            ToolHit hit = c.GetComponent<ToolHit>();
            if (hit != null)
            {
                if (hit.CanBeHit(canHitNodesOfType))
                {
                    
                    hit.Hit();
                    return true;
                }
            }
        }

        return false;
    }

    private void SetAnimationTrigger()
    {
        if (canHitNodesOfType.Contains(ResourceNodeType.Tree))
        {
            animationTrigger = axeAnimationTrigger;
            
        }
        else if (canHitNodesOfType.Contains(ResourceNodeType.Ore))
        {
            animationTrigger = pickaxeAnimationTrigger;
            
        }
        else
        {
            animationTrigger = string.Empty; // O cualquier otro valor por defecto
        }
    }
}