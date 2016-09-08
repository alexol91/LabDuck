using Caliburn.Micro;
using LabDuck.LexicalEditor.Services.Contracts;
using Newtonsoft.Json;
using System;
using System.Windows;

namespace LabDuck.LexicalEditor.ViewModels
{
    public class StartUpDialogViewModel : Screen
    {
        private readonly IWindowManager windowsManager;
        private readonly IFileManagementService fileManagementService;
        private readonly MainViewModel mainViewModel;

        public StartUpDialogViewModel(IWindowManager windowsManager, IFileManagementService fileManagementService,
            MainViewModel mainViewModel)
        {
            this.windowsManager = windowsManager;
            this.fileManagementService = fileManagementService;
            this.mainViewModel = mainViewModel;
        }

        public void OpenFile()
        {
            try
            {
                var fileName = fileManagementService.ShowOpenFileDialog();
                mainViewModel.TokenDefinitions = fileManagementService.LoadTokenDefinitions(fileName);
                mainViewModel.FileName = fileName;
                NavigateToMainWindows();
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Error when deserializate file: {ex?.Message}", "Json Error");
            }
            catch (OperationCanceledException)
            {
                // Do nothing.
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error ocurred: {ex?.Message}", "Error");
            }
        }

        public void CreateNew()
        {
            NavigateToMainWindows();
        }

        private void NavigateToMainWindows()
        {
            windowsManager.ShowWindow(mainViewModel);
            TryClose();
        }
    }
}
