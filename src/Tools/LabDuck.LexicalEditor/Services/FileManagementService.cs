using System;
using System.Collections.ObjectModel;
using LabDuck.Domain.Lexical;
using LabDuck.LexicalEditor.Services.Contracts;
using Newtonsoft.Json;
using System.IO;
using Microsoft.Win32;
using System.Collections.Generic;

namespace LabDuck.LexicalEditor.Services
{
    public class FileManagementService : IFileManagementService
    {
        public string ShowOpenFileDialog()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.CheckFileExists = true;
            dialog.Multiselect = false;
            dialog.Filter = "JSON|*.json";
            var show = dialog.ShowDialog();
            if (show.HasValue && show.Value)
            {
                return dialog.FileName;
            }
            throw new OperationCanceledException();
        }

        public string ShowSaveFileDialog()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "JSON|*.json";
            var show = dialog.ShowDialog();
            if (show.HasValue && show.Value)
            {
                return dialog.FileName;
            }
            throw new OperationCanceledException();
        }

        public ObservableCollection<TokenDefinition> LoadTokenDefinitions(string fileName)
        {
            string json = File.ReadAllText(fileName);
            var tokenDefinitions = JsonConvert.DeserializeObject<List<TokenDefinition>>(json);
            return new ObservableCollection<TokenDefinition>(tokenDefinitions);
        }

        public void SaveTokenDefinitions(string fileName, ObservableCollection<TokenDefinition> tokenDefinitions)
        {
            var json = JsonConvert.SerializeObject(tokenDefinitions);
            File.WriteAllText(fileName, json);
        }
    }
}
