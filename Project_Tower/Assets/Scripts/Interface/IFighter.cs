using System.Numerics;
using UnityEngine;

public interface IFighter
{
    int Speed {get; set;}

    Sprite PreviewImage {get; set;}

    public abstract float Attack();
}
