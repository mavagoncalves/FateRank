using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FateRank.ViewModels;

public class BaseViewModel : INotifyPropertyChanged
{
    // The '?' makes it nullable, fixing CS8618
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}