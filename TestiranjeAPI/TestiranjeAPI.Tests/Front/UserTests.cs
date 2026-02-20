using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestiranjeAPI.Tests.Front;

//[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class UserTests : PageTest
{
    public override BrowserNewContextOptions ContextOptions()
    {
        return new BrowserNewContextOptions()
        {
            IgnoreHTTPSErrors = true
        };
    }

    [Test]
    public async Task Login()
    {
        await Page.GotoAsync("http://127.0.0.1:5500/Front/Pages/Login/index.html");
        await Page.WaitForLoadStateAsync(LoadState.Load);
        await Page.GetByLabel("Username").ClickAsync();
        await Page.GetByLabel("Username").FillAsync("pwLoginTest");
        await Page.GetByLabel("Password").ClickAsync();
        await Page.GetByLabel("Password").FillAsync("pwLoginTest123@");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();
        await Expect(Page).ToHaveTitleAsync("Dashboard");
        await Page.WaitForLoadStateAsync(LoadState.Load);

        string expectedTitle = "Dashboard";
        string actualTitle = await Page.TitleAsync();


        Assert.That(actualTitle, Is.EqualTo(expectedTitle));
    }

    [Test]
    public async Task UpdateProfile()
    {
        await Page.GotoAsync("http://127.0.0.1:5500/Front/Pages/Login/index.html");
        await Page.WaitForLoadStateAsync(LoadState.Load);
        await Page.GetByLabel("Username").ClickAsync();
        await Page.GetByLabel("Username").FillAsync("pwUpdateTest");
        await Page.GetByLabel("Password").ClickAsync();
        await Page.GetByLabel("Password").FillAsync("pwUpdateTest123@");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();
        await Expect(Page).ToHaveTitleAsync("Dashboard");
        await Page.WaitForLoadStateAsync(LoadState.Load);

        await Page.GetByRole(AriaRole.Button, new() { Name = "Profile" }).ClickAsync();
        await Expect(Page).ToHaveTitleAsync("My Profile");
        await Page.WaitForLoadStateAsync(LoadState.Load);

        await Page.GetByLabel("Email").ClickAsync();
        await Page.GetByLabel("Email").FillAsync("pwUpdateTest123@gmail.com");
        void Page_Dialog1_EventHandler(object sender, IDialog dialog)
        {
            string expected = "Profil je azuriran";
            string actual = dialog.Message;
            dialog.DismissAsync();
            Page.Dialog -= Page_Dialog1_EventHandler;
            Assert.That(actual, Is.EqualTo(expected));
        }
        Page.Dialog += Page_Dialog1_EventHandler;
        await Page.GetByRole(AriaRole.Button, new() { Name = "Update" }).ClickAsync();
    }

    [Test]
    public async Task Register()
    {
        await Page.GotoAsync("http://127.0.0.1:5500/Front/Pages/Register/index.html");
        await Page.WaitForLoadStateAsync(LoadState.Load);
        await Page.GetByLabel("Username").ClickAsync();
        await Page.GetByLabel("Username").FillAsync("pwRegisterTest");
        await Page.GetByLabel("Email").ClickAsync();
        await Page.GetByLabel("Email").FillAsync("pwRegisterTest@gmail.com");
        await Page.GetByLabel("Password").ClickAsync();
        await Page.GetByLabel("Password").FillAsync("pwRegisterTest123@");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Avatar" }).ClickAsync();
        await Page.SetInputFilesAsync("input[type=file]", new[] { "avatar-thumb--1-.png" });
        await Page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();
        await Expect(Page).ToHaveTitleAsync("Login");
        await Page.WaitForLoadStateAsync(LoadState.Load);


        string actual = await Page.TitleAsync();
        string expected = "Login";

        Assert.That(actual, Is.EqualTo(expected));
    }
}
