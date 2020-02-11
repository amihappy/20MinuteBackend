namespace _20MinuteBackend.Domain.Randomizers
{
    public interface IDataRandomizerFactory
    {
        IDataTypeRandomizer Create(string value);
    }
}
