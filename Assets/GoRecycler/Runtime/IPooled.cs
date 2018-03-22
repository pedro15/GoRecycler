namespace GoRecycler
{
    public interface IPooled
    {
        void OnSpawn();
        void OnRecycle();
    }
}