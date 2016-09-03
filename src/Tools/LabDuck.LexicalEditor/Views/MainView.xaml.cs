using LabDuck.Domain.Lexical;
using LabDuck.LexicalEditor.ViewModels;
using MahApps.Metro.Controls;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace LabDuck.LexicalEditor.Views
{
    public partial class MainView : MetroWindow
    {
        private const string MainWindowTitle = "Lab Duck - Lexical Editor";

        public MainView()
        {
            InitializeComponent();
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            Title = MainWindowTitle;
        }

        private void OnTestTokenDefinitionClick(object sender, System.Windows.RoutedEventArgs e)
        {
            MatchRegularExpression();
        }

        private void MatchRegularExpression()
        {
            try
            {
                var selectedTokenDefinition = TokenDefinitions.SelectedItem as TokenDefinition;
                if (selectedTokenDefinition != null)
                {
                    string pattern = selectedTokenDefinition.RegularExpression;
                    HighlightRegex(pattern);
                }
                RegexErrorMessage.Text = string.Empty;
            }
            catch (Exception)
            {
                RegexErrorMessage.Text = "Error in Regular Expression";
            }
        }

        private void HighlightRegex(string pattern)
        {
            RegexTest.TextChanged -= OnRegexTestTextChanged;
            ClearHighlight();
            if (AllowMultiline.IsChecked.HasValue && AllowMultiline.IsChecked.Value)
            {
                SimplifyBlocks();
            }
            TextRange range = new TextRange(RegexTest.Document.ContentStart, RegexTest.Document.ContentEnd);
            Regex reg = new Regex(pattern, RegexOptions.Multiline);

            var start = RegexTest.Document.ContentStart;
            while (start != null && start.CompareTo(RegexTest.Document.ContentEnd) < 0)
            {
                if (start.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    var match = reg.Match(start.GetTextInRun(LogicalDirection.Forward));

                    var textrange = new TextRange(start.GetPositionAtOffset(match.Index, LogicalDirection.Forward), start.GetPositionAtOffset(match.Index + match.Length, LogicalDirection.Backward));
                    textrange.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(Colors.LightSkyBlue));
                    start = textrange.End;
                }
                start = start.GetNextContextPosition(LogicalDirection.Forward);
            }
            RegexTest.TextChanged += OnRegexTestTextChanged;
        }

        private void ClearHighlight()
        {
            TextRange range = new TextRange(RegexTest.Document.ContentStart, RegexTest.Document.ContentEnd);
            range.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(Colors.Transparent));
        }

        private void SimplifyBlocks()
        {
            var run = new Run();
            var paragraph = new Paragraph();
            paragraph.Inlines.Add(run);
            run.Text = ReadRegexTestText();
            RegexTest.Document.Blocks.Clear();
            RegexTest.Document.Blocks.Add(paragraph);
        }

        private void OnRegexTestTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!AllowMultiline.IsChecked.HasValue || !AllowMultiline.IsChecked.Value)
            {
                MatchRegularExpression();
            }
        }

        private void OnRegularExpressionTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            MatchRegularExpression();
        }

        private void OnTokenDefinitionsSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            TokenDefinitions.ScrollIntoView(TokenDefinitions.SelectedItem);
        }

        private void OnRegexTestLostFocus(object sender, RoutedEventArgs e)
        {
            MatchRegularExpression();
        }

        private void OnAllowMultilineChecked(object sender, RoutedEventArgs e)
        {
            MatchRegularExpression();
        }

        private string ReadRegexTestText()
        {
            var stringBuilder = new StringBuilder();
            foreach (var item in RegexTest.Document.Blocks)
            {
                var text = new TextRange(item.ContentStart, item.ContentEnd).Text.Replace("\r", string.Empty);
                if (!string.IsNullOrEmpty(text))
                {
                    stringBuilder.AppendLine(text);
                }
            }
            return stringBuilder.ToString();
        }
    }
}
