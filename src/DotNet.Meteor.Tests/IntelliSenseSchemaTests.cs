using Xunit;
using DotNet.Meteor.Xaml;

namespace DotNet.Meteor.Tests;

public class IntelliSenseSchemaTests: TestFixture {

    [Fact]
    public void AutoSelectActualPathTest() {
        var projectPath = CreateMockProject(@"
        <Project Sdk=""Microsoft.NET.Sdk"">
            <PropertyGroup>
                <OutputType>Exe</OutputType>
                <UseMaui>true</UseMaui>
            </PropertyGroup>
        </Project>
        ");
        CreateOutputAssembly("Debug", "net7.0-droid", null, "Microsoft.Maui.Controls.dll", false);
        CreateOutputAssembly("Debug", "net7.0-ios", DeviceService.AppleArm64, "Microsoft.Maui.Controls.dll", false);
        var expectedPath = CreateOutputAssembly("Debug", "net7.0-ios", DeviceService.AppleSimulatorX64, "Microsoft.Maui.Controls.dll", false);
        var typeLoader = new MauiTypeLoader(projectPath, s => {
            if (!s.Contains("Bad IL format"))
                Assert.Fail(s);
        });
        typeLoader.LoadComparedTypes();

        Assert.Equal(Path.GetDirectoryName(expectedPath), typeLoader.AssembliesDirectory);
        DeleteMockData();
    }
}