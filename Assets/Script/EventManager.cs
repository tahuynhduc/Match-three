using System;

public static class EventManager
{
    public static Action<Item> SelectedItem = delegate{};
    public static Action RevertItem = delegate { };
    public static Action<bool> SwapItem = delegate { };
    public static void OnRevertItem()
    {
        RevertItem.Invoke();
    }
    public static void OnSelectedItem(Item selectedItem)
    {
        SelectedItem.Invoke(selectedItem);
    }
    public static void OnSwapItem(bool state)
    {
        SwapItem.Invoke(state);
    }
}