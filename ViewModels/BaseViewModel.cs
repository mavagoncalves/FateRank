using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FateRank.ViewModels;

/// <summary>
/// A base class for ViewModels that implements INotifyPropertyChanged.
/// This allows the UI to automatically update when data changes.
/// </summary>
public class BaseViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Updates a value and notifies the UI that it has changed.
    /// </summary>
    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}