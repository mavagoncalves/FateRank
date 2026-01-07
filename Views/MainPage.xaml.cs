using Microsoft.Maui.Controls;
using FateRank.ViewModels;

namespace FateRank.Views;

public partial class MainPage : ContentPage
{
    /// <summary>
    /// Initializes the UI components and assigns a new <see cref="MainViewModel"/> as the page's BindingContext.
    /// </summary>
    public MainPage()
    {
        InitializeComponent();
		BindingContext = new MainViewModel();
    }
}