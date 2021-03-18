using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarRandomizer : MonoBehaviour
{
    [SerializeField] private AvatarController avatar1;
    [SerializeField] private AvatarController avatar2;
    [SerializeField] private AvatarController avatar3;
    [SerializeField] private AvatarController avatar4;
    [SerializeField] private AvatarController avatar5;
    [SerializeField] private AvatarController avatar6;

    private List<AvatarController> avatarList;

    void Start()
    {
        avatarList = new List<AvatarController>();
        avatarList.Add(avatar1);avatarList.Add(avatar2);avatarList.Add(avatar3);avatarList.Add(avatar4);avatarList.Add(avatar5);avatarList.Add(avatar6);
        avatarList.Shuffle();

        for (int i = 0; i < avatarList.Count; i++)
        {
            avatarList[i].playerIndex = i;
        }
    }
}

public static class IListExtensions
{
    /// <summary>
    /// Shuffles the element order of the specified list.
    /// </summary>
    public static void Shuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
}
