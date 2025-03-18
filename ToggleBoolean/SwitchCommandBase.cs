using System;
using System.ComponentModel.Design;
using EnvDTE;
using Microsoft.VisualStudio.Shell;

namespace ToggleBoolean
{
    internal class SwitchCommandBase
    {
        private bool _reverse;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("1809e4b7-8455-46dd-8743-03e071875aa9");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;

        /// <summary>
        /// Initializes a new instance of the <see cref="SwitchCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        internal protected SwitchCommandBase(
            AsyncPackage package,
            OleMenuCommandService commandService,
            int commandId,
            bool reverse
        )
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService =
                commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, commandId);
            var menuItem = new MenuCommand(this.Execute, menuCommandID);
            commandService.AddCommand(menuItem);

            _reverse = reverse;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
        {
            get { return this.package; }
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
            var replaceText = (string)null;

            if (!string.IsNullOrEmpty(text))
            {
                replaceText = Switcher.Switch(text, _reverse);
            }

            if (replaceText == null)
            {
                textSelection.WordRight();
                textSelection.WordLeft(true);
                text = textSelection.Text.Trim();
                replaceText = Switcher.Switch(text, _reverse);
            }

            if (replaceText == null)
            {
                textSelection.MoveToPoint(start);
                textSelection.MoveToPoint(end, true);
                textSelection.WordLeft();
                textSelection.WordRight(true);
                text = textSelection.Text.Trim();
                replaceText = Switcher.Switch(text, _reverse);
            }

            if (replaceText != null)
            {
                textSelection.ReplaceText(text, replaceText);
            }

            textSelection.MoveToPoint(start);
            textSelection.MoveToPoint(end, true);
        }
    }
}
