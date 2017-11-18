namespace Framework
{
    public delegate void PropertyChangedHandlerEx(object sender, PropertyChangedEventArgs args);
    public interface INotifyPropertyChangedEx
    {
        event PropertyChangedHandlerEx PropertyChanged;
    }
    public interface INotifyPropertyChangingEx
    {
        event PropertyChangedHandlerEx PropertyChanging;
    }
}
