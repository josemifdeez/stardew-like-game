using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemConversionConfig
{
    public Item inputItem;
    public Item outputItem;
    public int outputCount = 1;
    public int processTime = 5;
}

[Serializable]
public class ItemConvertorData
{
    public ItemSlot itemSlot;
    public int timer;
    public ItemConversionConfig currentConfig;

    public ItemConvertorData()
    {
        itemSlot = new ItemSlot();
    }
}

[RequireComponent(typeof(TimeAgent))]
public class ItemConvertorInteract : Interactable, IPersistant
{
    [SerializeField] List<ItemConversionConfig> conversionConfigs;

    ItemConvertorData data;
    Animator animator;

    private void Start()
    {
        TimeAgent timeAgent = GetComponent<TimeAgent>();
        timeAgent.onTimeTick += ItemConvertProcess;

        if (data == null)
        {
            data = new ItemConvertorData();
        }
        animator = GetComponent<Animator>();
        Animate();
    }

    private void ItemConvertProcess(DayTimeController dayTimeController)
    {
        if (data.itemSlot == null || data.currentConfig == null) { return; }
        if (data.timer > 0)
        {
            data.timer -= 1;
            if (data.timer <= 0)
            {
                CompleteItemConversion();
            }
        }
    }

    public override void Interact(Character character)
    {
        if (data.itemSlot.item == null)
        {
            ItemSlot itemSlot = GetItemSlot(character);

            if (itemSlot == null) { return; }

            foreach (var config in conversionConfigs)
            {
                if (itemSlot.item == config.inputItem)
                {
                    StartItemProcessing(itemSlot, config);
                    return;
                }
            }
        }

        if (data.itemSlot.item != null && data.timer <= 0)
        {
            GameManager.instance.inventoryContainer.Add(data.itemSlot.item, data.itemSlot.count);
            data.itemSlot.Clear();
            data.currentConfig = null;
        }
    }

    private ItemSlot GetItemSlot(Character character)
    {
        foreach (var config in conversionConfigs)
        {
            if (GameManager.instance.dragAndDropController.Check(config.inputItem))
            {
                return GameManager.instance.dragAndDropController.itemSlot;
            }

            ToolbarController toolbarController = character.GetComponent<ToolbarController>();
            if (toolbarController == null) { return null; }

            ItemSlot itemSlot = toolbarController.GetItemSlot;
            if (itemSlot.item == config.inputItem)
            {
                return itemSlot;
            }
        }

        return null;
    }

    private void StartItemProcessing(ItemSlot toProcess, ItemConversionConfig config)
    {
        data.itemSlot.Copy(toProcess);
        data.itemSlot.count = 1;

        if (toProcess.item.stackable)
        {
            toProcess.count -= 1;
            if (toProcess.count <= 0)
            {
                toProcess.Clear();
            }
        }
        else
        {
            toProcess.Clear();
        }

        // Actualizar el itemSlot del dragAndDropController o ToolbarController
        if (GameManager.instance.dragAndDropController.itemSlot == toProcess)
        {
            GameManager.instance.dragAndDropController.itemSlot = toProcess;
        }
        else
        {
            ToolbarController toolbarController = GameManager.instance.player.GetComponent<ToolbarController>();
            if (toolbarController != null)
            {
                int selectedTool = Array.FindIndex(
                    GameManager.instance.inventoryContainer.slots.ToArray(),
                    slot => slot == toProcess
                );
                if (selectedTool >= 0)
                {
                    toolbarController.Set(selectedTool);
                }
            }
        }

        data.timer = config.processTime;
        data.currentConfig = config;
        Animate();
    }

    private void Animate()
    {
        animator.SetBool("Working", data.timer > 0f);
    }

    private void CompleteItemConversion()
    {
        Animate();
        data.itemSlot.Clear();
        data.itemSlot.Set(data.currentConfig.outputItem, data.currentConfig.outputCount);
        data.currentConfig = null;
    }

    public string Read()
    {
        return JsonUtility.ToJson(data);
    }

    public void Load(string jsonString)
    {
        data = JsonUtility.FromJson<ItemConvertorData>(jsonString);
    }
}
