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
public class PartyTests : PageTest
{
    public override BrowserNewContextOptions ContextOptions()
    {
        return new BrowserNewContextOptions()
        {
            IgnoreHTTPSErrors = true
        };
    }

    [Test]
    public async Task CreateParty()
    {
        await Page.GotoAsync("http://127.0.0.1:5500/Front/Pages/Login/index.html");
        await Page.WaitForLoadStateAsync(LoadState.Load);
        //await Page.GetByLabel("Username").ClickAsync();
        await Page.GetByLabel("Username").FillAsync("pwCreatePartyTest");
        //await Page.GetByLabel("Password").ClickAsync();
        await Page.GetByLabel("Password").FillAsync("pwCreatePartyTest123@");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();
        await Expect(Page).ToHaveTitleAsync("Dashboard");
        await Page.WaitForLoadStateAsync(LoadState.Load);

        await Page.GetByRole(AriaRole.Button, new() { Name = "Create Party" }).ClickAsync();
        await Expect(Page).ToHaveTitleAsync("Create Party");
        await Page.WaitForLoadStateAsync(LoadState.Load);

        await Page.GetByLabel("Party Name").ClickAsync();
        await Page.GetByLabel("Party Name").FillAsync("zurka");
        await Page.GetByLabel("City").ClickAsync();
        await Page.GetByLabel("City").FillAsync("zurka");
        await Page.GetByLabel("Address").ClickAsync();
        await Page.GetByLabel("Address").FillAsync("zurka");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Image" }).ClickAsync();
        await Page.SetInputFilesAsync("input[type=file]", new[] { "avatar-thumb--1-.png" });
        void Page_Dialog_EventHandler(object sender, IDialog dialog)
        {
            string expected = "Zurka napravljena";
            string actual = dialog.Message;
            dialog.DismissAsync();
            Page.Dialog -= Page_Dialog_EventHandler;
            Assert.That(actual, Is.EqualTo(expected));
        }
        Page.Dialog += Page_Dialog_EventHandler;
        await Page.GetByRole(AriaRole.Button, new() { Name = "Create Party" }).ClickAsync();
    }

    [Test]
    public async Task CancelParty()
    {
        await Page.GotoAsync("http://127.0.0.1:5500/Front/Pages/Login/index.html");
        await Page.WaitForLoadStateAsync(LoadState.Load);
        await Page.GetByLabel("Username").ClickAsync();
        await Page.GetByLabel("Username").FillAsync("pwCancelPartyTest");
        await Page.GetByLabel("Password").ClickAsync();
        await Page.GetByLabel("Password").FillAsync("pwCancelPartyTest123@");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();
        await Expect(Page).ToHaveTitleAsync("Dashboard");
        await Page.WaitForLoadStateAsync(LoadState.Load);

        await Page.GetByRole(AriaRole.Button, new() { Name = "My Parties" }).ClickAsync();
        await Expect(Page).ToHaveTitleAsync("My Parties");
        await Page.WaitForLoadStateAsync(LoadState.Load);

        void Page_Dialog_EventHandler(object sender, IDialog dialog)
        {
            string expected = "Zurka je otkazana";
            string actual = dialog.Message;
            Assert.That(actual, Is.EqualTo(expected));
            dialog.DismissAsync();
            Page.Dialog -= Page_Dialog_EventHandler;
        }
        Page.Dialog += Page_Dialog_EventHandler;
        await Page.GetByRole(AriaRole.Button, new() { Name = "Cancel Party" }).ClickAsync();
    }

    [Test]
    public async Task EditParty()
    {
        await Page.GotoAsync("http://127.0.0.1:5500/Front/Pages/Login/index.html");
        await Page.WaitForLoadStateAsync(LoadState.Load);
        await Page.GetByLabel("Username").ClickAsync();
        await Page.GetByLabel("Username").FillAsync("pwEditPartyTest");
        await Page.GetByLabel("Password").ClickAsync();
        await Page.GetByLabel("Password").FillAsync("pwEditPartyTest123@");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();
        await Expect(Page).ToHaveTitleAsync("Dashboard");
        await Page.WaitForLoadStateAsync(LoadState.Load);

        await Page.GetByRole(AriaRole.Button, new() { Name = "My Parties" }).ClickAsync();
        await Expect(Page).ToHaveTitleAsync("My Parties");
        await Page.WaitForLoadStateAsync(LoadState.Load);

        async void Page_Dialog_EventHandler(object sender, IDialog dialog)
        {
            if (dialog.Type == DialogType.Alert)
            {
                string expected = "Zurka je izmenjena";
                string actual = dialog.Message;
                Assert.That(actual, Is.EqualTo(expected));
                await dialog.AcceptAsync();

            }
            else
            {
                await dialog.AcceptAsync("Poruka");
            }
            //await dialog.DismissAsync();
            Page.Dialog -= Page_Dialog_EventHandler;
        }
        Page.Dialog += Page_Dialog_EventHandler;
        await Page.GetByRole(AriaRole.Button, new() { Name = "Edit Party" }).ClickAsync();
    }

    [Test]
    public async Task AttendParty()
    {
        await Page.GotoAsync("http://127.0.0.1:5500/Front/Pages/Login/index.html");
        await Page.WaitForLoadStateAsync(LoadState.Load);
        await Page.GetByLabel("Username").ClickAsync();
        await Page.GetByLabel("Username").FillAsync("pwAttendPartyTest");
        await Page.GetByLabel("Password").ClickAsync();
        await Page.GetByLabel("Password").FillAsync("pwAttendPartyTest123@");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();
        //await Task.Delay(1000);
        await Expect(Page).ToHaveTitleAsync("Dashboard");
        await Page.WaitForLoadStateAsync(LoadState.Load);

        await Page.GetByRole(AriaRole.Button, new() { Name = "Parties", Exact = true }).ClickAsync();
        await Expect(Page).ToHaveTitleAsync("Parties");
        await Page.WaitForLoadStateAsync(LoadState.Load);

        void Page_Dialog_EventHandler(object sender, IDialog dialog)
        {
            string expected = "Karta za zurku uzeta";
            string actual = dialog.Message;
            Assert.That(actual, Is.EqualTo(expected));
            dialog.DismissAsync();
            Page.Dialog -= Page_Dialog_EventHandler;
        }
        Page.Dialog += Page_Dialog_EventHandler;
        await Page.Locator("sl-card").Filter(new() { HasText = "PartyToAttendCity: NisAddress" }).GetByRole(AriaRole.Button).ClickAsync();
    }

    [Test]
    public async Task UnattendParty()
    {
        await Page.GotoAsync("http://127.0.0.1:5500/Front/Pages/Login/index.html");
        await Page.WaitForLoadStateAsync(LoadState.Load);
        await Page.GetByLabel("Username").ClickAsync();
        await Page.GetByLabel("Username").FillAsync("pwUnattendPartyTest");
        await Page.GetByLabel("Password").ClickAsync();
        await Page.GetByLabel("Password").FillAsync("pwUnattendPartyTest123@");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();
        await Expect(Page).ToHaveTitleAsync("Dashboard");
        await Page.WaitForLoadStateAsync(LoadState.Load);

        await Page.GetByRole(AriaRole.Button, new() { Name = "Tickets" }).ClickAsync();
        await Expect(Page).ToHaveTitleAsync("Tickets");
        await Page.WaitForLoadStateAsync(LoadState.Load);

        await Page.GetByRole(AriaRole.Button, new() { Name = "Unattend" }).ClickAsync();

        void Page_Dialog_EventHandler(object sender, IDialog dialog)
        {
            string expected = "Odlazak otkazan";
            string actual = dialog.Message;
            Assert.That(actual, Is.EqualTo(expected));
            dialog.DismissAsync();
            Page.Dialog -= Page_Dialog_EventHandler;
        }
        Page.Dialog += Page_Dialog_EventHandler;

    }
}
