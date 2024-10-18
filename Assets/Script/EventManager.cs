using System;

public static class EventManager
{
    public static Action<Item> SelectedItem = delegate{};
    public static Action<Item> SwapItem = delegate { };

    public static void OnSwapItem(Item swapItem)
    {
        SwapItem.Invoke(swapItem);
    }
    public static void OnSelectedItem(Item selectedItem)
    {
        SelectedItem.Invoke(selectedItem);
    }
}