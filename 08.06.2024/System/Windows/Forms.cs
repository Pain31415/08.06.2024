
namespace System.Windows
{
    internal class Forms
    {
        internal class FolderBrowserDialog
        {
            internal string SelectedPath;

            public FolderBrowserDialog()
            {
            }

            internal Forms.DialogResult ShowDialog()
            {
                throw new NotImplementedException();
            }
        }

        internal class DialogResult
        {
            public static DialogResult OK { get; internal set; }
        }
    }
}