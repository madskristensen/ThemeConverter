global using System;
global using Community.VisualStudio.Toolkit;
global using Microsoft.VisualStudio.Shell;
global using Task = System.Threading.Tasks.Task;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.VisualStudio.TextTemplating.VSHost;

namespace ThemeConverter
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration(Vsix.Name, Vsix.Description, Vsix.Version)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(PackageGuids.ThemeConverterString)]
    [ProvideCodeGenerator(typeof(ThemeConverter), ThemeConverter.Name, ThemeConverter.Description, true, RegisterCodeBase = true)]
    //[ProvideCodeGeneratorExtension(ThemeConverter.Name, ".jsonc", ProjectSystem = ProjectTypes.EXTENSIBILITY)]
    [ProvideUIContextRule(PackageGuids.VisibilityString,
        name: "JSONC",
        expression: "JSONC",
        termNames: new[] { "JSONC" },
        termValues: new[] { "HierSingleSelectionName:.jsonc?$" })]
    public sealed class ThemeConverterPackage : ToolkitPackage
    {
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await this.RegisterCommandsAsync();
        }
    }
}