using Lab.Servises;
using System.Windows;

namespace Lab;

/// <summary>
/// Interaction logic for ChildWindow.xaml
/// </summary>
public partial class ChildWindow : Window
{
    private object[] _plugins;
    public static int count = 0;

    public ChildWindow()
    {
        InitializeComponent();
        LoadPlugins();
    }

    // Load the plugins and populate the ComboBox
    private void LoadPlugins()
    {
        _plugins = PluginLoader.LoadPlugins("Plugins");

        foreach (var plugin in _plugins)
        {
            var descriptionMethod = plugin.GetType().GetMethod("GetDescription");
            if (descriptionMethod != null)
            {
                string description;
                string? result = descriptionMethod.Invoke(plugin, null) as string;

                if (result != null)
                {
                    description = result;
                }
                else
                {
                    description = "Чёт пошло не так";
                }
                pluginComboBox.Items.Add(description);
            }
        }

        if (_plugins.Length > 0)
        {
            pluginComboBox.SelectedIndex = 0;
        }
    }

    // Check if the input is binary
    private bool IsBinary(string binaryString)
    {
        return binaryString.All(c => c == '0' || c == '1');
    }

    // Convert binary string to decimal
    private int BinaryToDecimal(string binaryString)
    {
        return Convert.ToInt32(binaryString, 2);
    }

    // Convert decimal to binary string
    private string DecimalToBinary(int number)
    {
        return Convert.ToString(number, 2);
    }

    // Execute the selected plugin's operation
    private void ExecutePlugin_Click(object sender, RoutedEventArgs e)
    {
        string binaryA = inputA.Text;
        string binaryB = inputB.Text;

        if (IsBinary(binaryA) && IsBinary(binaryB))
        {
            int a = BinaryToDecimal(binaryA);
            int b = BinaryToDecimal(binaryB);

            if (pluginComboBox.SelectedIndex >= 0)
            {
                var selectedPlugin = _plugins[pluginComboBox.SelectedIndex];
                string selectedMethod = GetMethodNameFromDescription(selectedPlugin.GetType().GetMethod("GetDescription").Invoke(selectedPlugin, null).ToString());

                int? result = PluginLoader.ExecuteMethod(selectedPlugin, selectedMethod, a, b);

                if (result != null)
                {
                    resultTextBlock.Text = $"Результат: {DecimalToBinary(result.Value)}";
                }
            }
        }
        else
        {
            MessageBox.Show("Введите корректные двоичные числа (состоящие только из 0 и 1).");
        }
    }

    // Map description to method name
    private string GetMethodNameFromDescription(string description)
    {
        switch (description)
        {
            case "Бинарное сложение двух чисел":
                return "Add";
            case "Бинарное вычитание двух чисел":
                return "Sub";
            case "Бинарное умножение двух чисел":
                return "Mul";
            case "Остаток от деления двух чисел в бинарной форме":
                return "Div";
            default:
                return null;
        }
    }

    private void ChildWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите закрыть приложение?", "Закрыть", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.No)
        {
            e.Cancel = true;
        }
        --count;
    }
}
