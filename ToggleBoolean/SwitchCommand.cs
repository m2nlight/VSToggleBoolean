using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace ToggleBoolean
{
    internal sealed class SwitchCommand : SwitchCommandBase
    {
        private SwitchCommand(AsyncPackage package, OleMenuCommandService commandService)
            : base(package, commandService, 0x100, false) { }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static SwitchCommand Instance { get; private set; }

        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in ToggleBooleanCommand's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService =
                await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new SwitchCommand(package, commandService);
        }
    }
}
