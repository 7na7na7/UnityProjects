using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserIngameEvents : MonoBehaviour
{
    public delegate void updateItemCount();
    public static event updateItemCount UpdateItemCount;

    public void ItemShuffleButton()
    {
        if (UseItem.Instance.UseShuffleItem())
        {
            UseItem.Instance.ItemShuffle(true);
            UpdateItemCount();
        }
    }
    public void ItemHintButton()
    {
        if (UseItem.Instance.UseHintItem())
        {
            UseItem.Instance.ItemHint();
            UpdateItemCount();
        }
    }
}
