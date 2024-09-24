namespace Characters
{
    public interface IMainConfiguration
    {
        IProperties InitialPlayerProperties { get; }
        PlayerPropertiesProgressionModelSO PlayerPropertiesProgressionModelSo { get; }
    }
}