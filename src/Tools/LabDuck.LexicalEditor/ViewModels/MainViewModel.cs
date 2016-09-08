using Caliburn.Micro;
using LabDuck.Domain.Enums;
using LabDuck.Domain.Lexical;
using LabDuck.LexicalEditor.Command;
using LabDuck.LexicalEditor.Services.Contracts;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace LabDuck.LexicalEditor.ViewModels
{
    public class MainViewModel : Screen
    {
        private readonly IFileManagementService fileManagementService;
        private TokenDefinition _selectedTokenDefinition;
        private ObservableCollection<TokenDefinition> _tokenDefinitions;

        public MainViewModel(IFileManagementService fileManagementService)
        {
            this.fileManagementService = fileManagementService;
            InitializeTypes();
            TokenDefinitions = new ObservableCollection<TokenDefinition>();
        }

        public IEnumerable<TokenType> TokenTypes { get; set; }
        public string FileName { get; set; }
        public ICommand SaveCommand => new RelayCommand(() => Save());
        public ICommand NewCommand => new RelayCommand(() => New());
        public ICommand OpenCommand => new RelayCommand(() => Open());
        public ICommand SaveAsCommand => new RelayCommand(() => OpenDialogToSaveFile());
        public TokenDefinition SelectedTokenDefinition
        {
            get { return _selectedTokenDefinition; }
            set
            {
                _selectedTokenDefinition = value;
                NotifyOfPropertyChange();
            }
        }
        public ObservableCollection<TokenDefinition> TokenDefinitions
        {
            get { return _tokenDefinitions; }
            set
            {
                _tokenDefinitions = value;
                NotifyOfPropertyChange();
            }
        }


        protected override void OnActivate()
        {
            base.OnActivate();
            SelectFirstOrDefaultTokenDefinition();
        }

        private void InitializeTypes()
        {
            TokenTypes = Enum.GetValues(typeof(TokenType)).Cast<TokenType>();
        }

        public void Save()
        {
            if (string.IsNullOrEmpty(FileName))
            {
                OpenDialogToSaveFile();
            }
            else
            {
                SaveInAFile();
            }
        }

        public void New()
        {
            TokenDefinitions.Clear();
            FileName = string.Empty;
            SelectFirstOrDefaultTokenDefinition();
        }

        public void Open()
        {
            try
            {
                var fileName = fileManagementService.ShowOpenFileDialog();
                TokenDefinitions = fileManagementService.LoadTokenDefinitions(fileName);
                FileName = fileName;
                SelectFirstOrDefaultTokenDefinition();
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

        private void SaveInAFile()
        {
            try
            {
                fileManagementService.SaveTokenDefinitions(FileName, TokenDefinitions);
                MessageBox.Show($"Token definitions was successful saved", "Save Success");
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Error when serialize file: {ex?.Message}", "Json Error");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error ocurred: {ex?.Message}", "Error");
            }
        }

        private void OpenDialogToSaveFile()
        {
            try
            {
                FileName = fileManagementService.ShowSaveFileDialog();
                SaveInAFile();
            }
            catch(OperationCanceledException)
            {
                // Do Nothing.
            }
        }

        public void RemoveTokenDefinition()
        {
            TokenDefinitions.Remove(SelectedTokenDefinition);
            SelectedTokenDefinition = TokenDefinitions.FirstOrDefault();
        }

        private void SelectFirstOrDefaultTokenDefinition()
        {
            if(TokenDefinitions.Any())
            {
                SelectedTokenDefinition = TokenDefinitions.FirstOrDefault();
            }
            else
            {
                CreateTokenDefinition();
            }
        }

        public void CreateTokenDefinition()
        {
            TokenDefinitions.Add(new TokenDefinition() { Name = "New Token Definition" });
            SelectedTokenDefinition = TokenDefinitions.LastOrDefault();
        }
    }
}
