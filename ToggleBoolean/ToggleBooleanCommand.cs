using System;
using System.ComponentModel.Design;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace ToggleBoolean
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class ToggleBooleanCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("1809e4b7-8455-46dd-8743-03e071875aa9");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;

        private static readonly string[] WordA = { "true", "yes", "on", "0", "True", "Yes", "On", "TRUE", "YES", "ON" };
        private static readonly string[] WordB = { "false", "no", "off", "1", "False", "No", "Off", "FALSE", "NO", "OFF" };

        /// <summary>
        /// Initializes a new instance of the <see cref="ToggleBooleanCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private ToggleBooleanCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService =
                commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static ToggleBooleanCommand Instance { get; private set; }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
        {
            get { return this.package; }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in ToggleBooleanCommand's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService =
                await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new ToggleBooleanCommand(package, commandService);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private async void Execute(object sender, EventArgs e)
        {
            // ThreadHelper.ThrowIfNotOnUIThread();
            // string message = string.Format(CultureInfo.CurrentCulture, "Inside {0}.MenuItemCallback()", this.GetType().FullName);
            // string title = "ToggleBooleanCommand";
            //
            // // Show a message box to prove we were here
            // VsShellUtilities.ShowMessageBox(
            //     this.package,
            //     message,
            //     title,
            //     OLEMSGICON.OLEMSGICON_INFO,
            //     OLEMSGBUTTON.OLEMSGBUTTON_OK,
            //     OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);

            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            var dte = await ServiceProvider.GetServiceAsync(typeof(DTE)) as DTE;
            if (dte == null)
            {
                return;
            }

            var textSelection = dte.ActiveDocument.Selection as TextSelection;
            if (textSelection == null)
            {
                return;
            }

            var start = textSelection.ActivePoint.CreateEditPoint();
            var end = textSelection.AnchorPoint.CreateEditPoint();

            var text = textSelection.Text.Trim();
            if (string.IsNullOrEmpty(text))
            {
                textSelection.WordRight();
                textSelection.WordLeft(true);
                text = textSelection.Text.Trim();
            }

            var idx = Array.IndexOf(WordA, text);
            if (idx >= 0)
            {
                textSelection.ReplaceText(text, WordB[idx]);
            }
            else
            {
                idx = Array.IndexOf(WordB, text);
                if (idx >= 0)
                {
                    textSelection.ReplaceText(text, WordA[idx]);
                }
                else
                {
                    textSelection.MoveToPoint(start);
                    textSelection.MoveToPoint(end, true);
                    textSelection.WordLeft();
                    textSelection.WordRight(true);
                    text = textSelection.Text.Trim();

                    idx = Array.IndexOf(WordA, text);
                    if (idx >= 0)
                    {
                        textSelection.ReplaceText(text, WordB[idx]);
                    }
                    else
                    {
                        idx = Array.IndexOf(WordB, text);
                        if (idx >= 0)
                        {
                            textSelection.ReplaceText(text, WordA[idx]);
                        }
                    }
                }
            }

            textSelection.MoveToPoint(start);
            textSelection.MoveToPoint(end, true);
        }
    }
}
