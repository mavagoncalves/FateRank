using Microsoft.Maui.Controls;
using FateRank.ViewModels;

namespace FateRank.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
		BindingContext = new MainViewModel();
    }
}