using LabDuck.Domain.Lexical;
using System.Collections.ObjectModel;

namespace LabDuck.LexicalEditor.Services.Contracts
{
    public interface IFileManagementService
    {
        string ShowOpenFileDialog();
        string ShowSaveFileDialog();
        ObservableCollection<TokenDefinition> LoadTokenDefinitions(string fileName);
        void SaveTokenDefinitions(string fileName, ObservableCollection<TokenDefinition> tokenDefinitions);
    }
}
