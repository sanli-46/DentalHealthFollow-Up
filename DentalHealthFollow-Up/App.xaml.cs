﻿namespace DentalHealthFollow_Up.MAUI;

public partial class App : Application
{
    public App(AppShell appShell)
    {
        InitializeComponent();
        MainPage = appShell;
    }
}
