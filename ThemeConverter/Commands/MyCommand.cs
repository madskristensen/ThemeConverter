﻿namespace ThemeConverter
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
            var jsonc = await VS.Solutions.GetActiveItemAsync() as PhysicalFile;

            await jsonc.TrySetAttributeAsync("Custom Tool", ThemeConverter.Name);
        }
    }
}
