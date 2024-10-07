using System.Windows;

namespace Lab;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void MenuItemClick(object sender, RoutedEventArgs e)
    {
        if (ChildWindow.count == 0)
        {
            closeWindow.Visibility = Visibility.Hidden;
        }
        else
        {
            closeWindow.Visibility = Visibility.Visible;
        }
    }
 
    // Open a new child window
    private void NewWindow_Click(object sender, RoutedEventArgs e)
    {
        ChildWindow child = new ChildWindow();
        ChildWindow.count++;
        child.Owner = this;
        child.Show();
    }

    // Close the active child window
    private void CloseWindow_Click(object sender, RoutedEventArgs e)
    {
        if (ChildWindow.count > 0)
        {
            Window child = this.OwnedWindows[0];
            child.Close();
        }
    }

    // Exit the application
    private void Exit_Click(object sender, RoutedEventArgs e)
    {
        foreach (Window child in this.OwnedWindows)
        {
            child.Close();
        }

        this.Close();
    }

    // Arrange windows in cascade
    private void CascadeWindows_Click(object sender, RoutedEventArgs e)
    {
        double offsetX = 30;
        double offsetY = 30;
        double positionX = 0;
        double positionY = 0;

        foreach (Window child in this.OwnedWindows)
        {
            child.Left = positionX;
            child.Top = positionY;
            positionX += offsetX;
            positionY += offsetY;
        }

        foreach (Window child in this.OwnedWindows)
        {
            child.WindowState = WindowState.Normal;
        }
    }

    // Arrange windows in tile
    private void TileWindows_Click(object sender, RoutedEventArgs e)
    {
        int count = this.OwnedWindows.Count;
        if (count == 0) return;

        double width = this.Width / count;
        double height = this.Height / count;
        double positionX = 0;
        double positionY = 0;

        foreach (Window child in this.OwnedWindows)
        {
            child.Width = width;
            child.Height = height;
            child.Left = positionX;
            child.Top = positionY;
            positionX += width;
            positionY += height;
        }

        foreach (Window child in this.OwnedWindows)
        {
            child.WindowState = WindowState.Normal;
        }
    }

    // Minimize all child windows
    private void MinimizeWindows_Click(object sender, RoutedEventArgs e)
    {
        foreach (Window child in this.OwnedWindows)
        {
            child.WindowState = WindowState.Minimized;
        }
    }

    // Show about dialog
    private void About_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show($"Информация о программе\n{ChildWindow.count}", "О программе", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    // Confirmation before closing the parent window
    private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите закрыть приложение?", "Закрыть", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.No)
        {
            e.Cancel = true;
        }

        foreach (Window child in this.OwnedWindows)
        {
            child.Close();
        }
    }
}