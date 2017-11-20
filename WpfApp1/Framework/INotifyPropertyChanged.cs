namespace Framework
{
    public delegate void PropertyChangedHandlerEx(object sender, PropertyChangedEventArgsEx args);
    public interface INotifyPropertyChangedEx
    {
        event PropertyChangedHandlerEx PropertyChangedEx;
    }
    public interface INotifyPropertyChangingEx
    {
        event PropertyChangedHandlerEx PropertyChangingEx;
    }
}
