using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ToDoListManager
{
    
    public partial class MainWindow : Window
    {
        public string PlaceholderText = "Enter a task...";
        private const string FilePath = "tasks.txt";

        public MainWindow()
        {
            InitializeComponent();
            TaskInput.Text = PlaceholderText;
            TaskInput.Foreground = Brushes.Gray;
            LoadTasksFromFile();
        }
        private void TaskInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TaskInput.Text == PlaceholderText)
            {
                TaskInput.Text = string.Empty;
                TaskInput.Foreground = Brushes.Black;
            }
        }

        private void TaskInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TaskInput.Text))
            {
                TaskInput.Text = PlaceholderText;
                TaskInput.Foreground = Brushes.Gray;
            }
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            string task = TaskInput.Text.Trim();
            if (!string.IsNullOrEmpty(task) && task != PlaceholderText)
            {
                var checkBox = new CheckBox
                {
                    Content = task,
                    Margin = new Thickness(5),
                    Foreground = Brushes.Black
                };
                checkBox.Checked += TaskStatusChanged;
                checkBox.Unchecked += TaskStatusChanged;
                TaskList.Items.Add(checkBox);
                SaveTasksToFile();
                TaskInput.Text = PlaceholderText;
                TaskInput.Foreground = Brushes.Gray;
            }
            else
            {
                MessageBox.Show("Please enter a valid task!");
            }
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (TaskList.SelectedItem is CheckBox selectedTask)
            {
                TaskList.Items.Remove(selectedTask);
                SaveTasksToFile();
            }
            else
            {
                MessageBox.Show("Please select a task to delete!");
            }
        }

        private void TaskStatusChanged(object sender, RoutedEventArgs e)
        {
            SaveTasksToFile();
        }

        private void SaveTasksToFile()
        {
            try
            {
                var tasks = TaskList.Items
                    .OfType<CheckBox>()
                    .Select(cb => $"{cb.IsChecked};{cb.Content}")
                    .ToList();
                File.WriteAllLines(FilePath, tasks);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving tasks: {ex.Message}");
            }
        }

        private void LoadTasksFromFile()
        {
            try
            {
                if (!File.Exists(FilePath)) return;
                var lines = File.ReadAllLines(FilePath);
                foreach (var line in lines)
                {
                    var parts = line.Split(';');
                    if (parts.Length == 2)
                    {
                        var checkBox = new CheckBox
                        {
                            Content = parts[1],
                            IsChecked = bool.Parse(parts[0]),
                            Margin = new Thickness(5)
                        };
                        checkBox.Checked += TaskStatusChanged;
                        checkBox.Unchecked += TaskStatusChanged;
                        TaskList.Items.Add(checkBox);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tasks: {ex.Message}");
            }
        }

        private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TaskList == null) return;

            string filter = (FilterComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "All";
            TaskList.Items.Clear();

            if (!File.Exists(FilePath)) return;

            var tasks = File.ReadAllLines(FilePath);

            foreach (var task in tasks)
            {
                var parts = task.Split(';');
                if (parts.Length != 2) continue;

                var checkBox = new CheckBox
                {
                    Content = parts[1],
                    IsChecked = bool.Parse(parts[0]),
                    Margin = new Thickness(5)
                };
                checkBox.Checked += TaskStatusChanged;
                checkBox.Unchecked += TaskStatusChanged;

                if (filter == "Completed" && checkBox.IsChecked == true ||
                    filter == "Pending" && checkBox.IsChecked == false ||
                    filter == "All")
                {
                    TaskList.Items.Add(checkBox);
                }
            }
        }
    }
}
