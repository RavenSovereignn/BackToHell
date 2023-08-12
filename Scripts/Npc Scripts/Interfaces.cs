using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDestroyable
{
    public void DestroyObj();
}

public interface IBoss{
    public void TakeDamage(int dmg);
}

public interface IThrowable
{
    public void Grabed();
    public void Thrown();

}

public interface IEdible
{
    public int Eat();
}