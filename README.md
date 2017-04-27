# GoRecycler
Object Pool Manager for unity 3d Game Engine. 

## How to use

1 - Add The GoRecyclerUtility by navigating on : GoRecycler -> Create Utility. Or adding it on an Empty GameObject from component menu.

2- in the GoRecyclerUtility add your object pools .. assign the label , the prefab and if you want you can pre-load your object pool to avoid instancing in Gameplay. 

3- To Get An GameObject from an Object Pool:

```c#

// first you will need to import the GoRecycler namespace in your script 

using GoRecycler;

// To Get The GameObject Simply use:

GameObject MyGO = GoRecyclerUtility.GetgameObject("Your Object Pool Label"  , Position , Rotation);

```

4- To Recycle a GameObject to his Pool

```c#

// first you will need to import the GoRecycler namespace in your script 

using GoRecycler;

// first you need to check if the GameObject it's on a Object Pool 

     bool isinpool = gameObject.IsInObjectPool();
	 
	 if (isinpool)
             gameObject.Recycle();  // To recycle to Object Pool Simply Use Recycle()
         else
             Destroy(gameObject); // If there is not Object Pool associated to the gameObject you can destroy it. 
```
That's the basic use .. there are more features:

-Interface with the events of the object pool that you can use it on a script associated on a Object Pool.

```c#

	public interface IPooled
    {
        void OnSpawn(GoRecycleBin RecycleBin);
        void OnRecycle(GoRecycleBin RecycleBin);
    }

```
- Get the Object Pool (RecycleBin) Asociated from a GameObject:

```c#

	// It will return null if the gameObject is not in a Object Pool.
	GoRecycleBin pool = gameObject.GetRecycleBin();

```
