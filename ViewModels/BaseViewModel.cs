using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FateRank.ViewModels;

/// <summary>
/// Base view model implementing <see cref="INotifyPropertyChanged"/> and providing a helper
/// to raise property change notifications for bindings.
/// </summary>
public class BaseViewModel : INotifyPropertyChanged
{
    /// <summary>
    /// Event raised when a property value has changed.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Raises the <see cref="PropertyChanged"/> event for the specified property name.
    /// If no <paramref name="name"/> is supplied, the caller member name is used.
    /// </summary>
    /// <param name="name">The name of the property that changed.</param>
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}