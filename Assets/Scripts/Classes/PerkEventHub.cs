using System;
using UnityEngine;

public static class PerkEventHub
{
    public static event EventHandler OnSpeedColaPurchase;
    public static event EventHandler OnDoubleTapPurchase;

    public static void SpeedColaPurchased() => OnSpeedColaPurchase?.Invoke(null, EventArgs.Empty);
    public static void DoubleTapPurchased() => OnDoubleTapPurchase?.Invoke(null, EventArgs.Empty);
}

