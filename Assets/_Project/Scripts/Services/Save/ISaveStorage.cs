namespace Services.Save
{
    public interface ISaveStorage<T>
    {
        public void Save(T data);

        public T Load();
    }
}