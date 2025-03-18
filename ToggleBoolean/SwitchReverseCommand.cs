using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace ToggleBoolean
{
    internal sealed class SwitchReverseCommand : SwitchCommandBase
    {
        private SwitchReverseCommand(AsyncPackage package, OleMenuCommandService commandService)
            : base(package, commandService, 0x101, true) { }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static SwitchReverseCommand Instance { get; private set; }

        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in ToggleBooleanCommand's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService =
                await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new SwitchReverseCommand(package, commandService);
        }
    }
}
