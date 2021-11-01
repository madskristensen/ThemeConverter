using EnvDTE;
using EnvDTE80;

namespace ThemeConverter
{
    [Command(PackageIds.MyCommand)]
    internal sealed class MyCommand : BaseCommand<MyCommand>
    {
        protected override Task InitializeCompletedAsync()
        {
            Command.Supported = false;
            return base.InitializeCompletedAsync();
        }

        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            DTE2 dte = await VS.GetRequiredServiceAsync<DTE, DTE2>();
            ProjectItem item = dte.SelectedItems.Item(1)?.ProjectItem;

            if (item != null)
            {
                item.Properties.Item("CustomTool").Value = ThemeConverter.Name;
            }
        }
    }
}
