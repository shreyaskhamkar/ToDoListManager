# ToDoList Manager Application

## Overview
The **ToDoList Manager** is a simple task management desktop application built using **C# and WPF (Windows Presentation Foundation)**. This tool allows users to manage their daily tasks efficiently by providing features such as task creation, deletion, task status tracking, filtering, and persistence.

---

## Features
- **Add Tasks:** Enter and add tasks to the list.
- **Delete Tasks:** Remove selected tasks from the list.
- **Mark Tasks as Complete:** Use checkboxes to track completed tasks.
- **Filter Tasks:** Filter tasks by "All," "Completed," or "Pending" status.
- **Task Persistence:** Tasks are saved in a text file (`tasks.txt`) and loaded automatically when the application starts.
- **Themed UI:** Styled buttons, input boxes, and layout for a better user experience.

---

## Requirements
- **Operating System:** Windows 10 or higher
- **Development Environment:** Visual Studio 2022 or later
- **.NET Framework:** Version 4.8 or higher

---

## Installation Instructions
1. Clone or download this repository.
2. Open the project in **Visual Studio**.
3. Build the project to restore dependencies.
4. Run the application by pressing `F5` or selecting **Start**.

---

## Usage Instructions
1. **Adding a Task:**
    - Enter the task description in the text box.
    - Click the `Add` button to add the task.

2. **Deleting a Task:**
    - Select a task from the list.
    - Click the `Delete Selected` button to remove it.

3. **Marking a Task as Complete:**
    - Check the checkbox next to the task to mark it as completed.

4. **Filtering Tasks:**
    - Use the dropdown to filter by "All," "Completed," or "Pending" tasks.

5. **Task Persistence:**
    - Tasks are automatically saved in the `tasks.txt` file.
    - On the next application start, tasks will be reloaded.

---

## File Structure
- **MainWindow.xaml:** Defines the user interface.
- **MainWindow.xaml.cs:** Contains the event handlers and business logic.
- **tasks.txt:** Stores task information persistently.

---

## Code Highlights
### Adding Tasks
```csharp
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
```

### Filtering Tasks
```csharp
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
```
![image](https://github.com/user-attachments/assets/15d8579f-0721-44cd-b152-d4d3e06e3391)

## Contributors
- **Shreyas Khamkar** - Developer and Maintainer

