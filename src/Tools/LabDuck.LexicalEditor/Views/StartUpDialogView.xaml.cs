using System;
using MahApps.Metro.Controls;

namespace LabDuck.LexicalEditor.Views
{
    public partial class StartUpDialogView : MetroWindow
    {
        private const string MainWindowTitle = "Lab Duck - Lexical Editor";

        public StartUpDialogView()
        {
            InitializeComponent();
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            Title = MainWindowTitle;
        }
    }
}
