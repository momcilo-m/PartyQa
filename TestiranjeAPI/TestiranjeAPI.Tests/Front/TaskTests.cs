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
public class TaskTests : PageTest
{
    public override BrowserNewContextOptions ContextOptions()
    {
        return new BrowserNewContextOptions()
        {
            IgnoreHTTPSErrors = true
            
        };
    }
    [Test]
    public async Task CreateTask()
    {
        await Page.GotoAsync("http://127.0.0.1:5500/Front/Pages/Login/index.html");
        await Page.WaitForLoadStateAsync(LoadState.Load);
        await Page.GetByLabel("Username").ClickAsync();
        await Page.GetByLabel("Username").FillAsync("pwTaskCreateTest");
        await Page.GetByLabel("Password").ClickAsync();
        await Page.GetByLabel("Password").FillAsync("pwTaskCreateTest123@");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();
        await Expect(Page).ToHaveTitleAsync("Dashboard");
        await Page.WaitForLoadStateAsync(LoadState.Load);

        await Page.GetByRole(AriaRole.Button, new() { Name = "Create Task" }).ClickAsync();
        await Expect(Page).ToHaveTitleAsync("Create Task");
        await Page.WaitForLoadStateAsync(LoadState.Load);

        await Page.GetByRole(AriaRole.Combobox, new() { Name = "Choose Party" }).ClickAsync();
        await Page.GetByRole(AriaRole.Option, new() { Name = "TaskToCreate" }).Locator("slot").Nth(1).ClickAsync();
        await Page.GetByLabel("Task Name").ClickAsync();
        await Page.GetByLabel("Task Name").FillAsync("Task");
        await Page.GetByLabel("Task Description").ClickAsync();
        await Page.GetByLabel("Task Description").FillAsync("Task");
        void Page_Dialog_EventHandler(object sender, IDialog dialog)
        {
            string expected = "Task je uspesno napravljen";
            string actual = dialog.Message;
            Assert.That(actual, Is.EqualTo(expected));
            dialog.DismissAsync();
            Page.Dialog -= Page_Dialog_EventHandler;
        }
        Page.Dialog += Page_Dialog_EventHandler;
        await Page.GetByRole(AriaRole.Button, new() { Name = "Create Task" }).ClickAsync();
    }

    [Test]
    public async Task EditTask()
    {
        await Page.GotoAsync("http://127.0.0.1:5500/Front/Pages/Login/index.html");
        await Page.WaitForLoadStateAsync(LoadState.Load);
        await Page.GetByLabel("Username").ClickAsync();
        await Page.GetByLabel("Username").FillAsync("pwTaskEditTest");
        await Page.GetByLabel("Password").ClickAsync();
        await Page.GetByLabel("Password").FillAsync("pwTaskEditTest123@");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();
        await Expect(Page).ToHaveTitleAsync("Dashboard");
        await Page.WaitForLoadStateAsync(LoadState.Load);

        await Page.GetByRole(AriaRole.Button, new() { Name = "Tasks" }).ClickAsync();
        await Expect(Page).ToHaveTitleAsync("Tasks");
        await Page.WaitForLoadStateAsync(LoadState.Load);

        async void Page_Dialog_EventHandler(object sender, IDialog dialog)
        {
            if (dialog.Type == DialogType.Alert)
            {
                string expected = "Task uspesno editovan";
                string actual = dialog.Message;
                Assert.That(actual, Is.EqualTo(expected));
                await dialog.DismissAsync();
            }
            else
            {
                await dialog.AcceptAsync("Task");
            }
            //dialog.DismissAsync();
            Page.Dialog -= Page_Dialog_EventHandler;
        }
        Page.Dialog += Page_Dialog_EventHandler;
        await Page.GetByRole(AriaRole.Button, new() { Name = "Edit" }).ClickAsync();
    }

    [Test]
    public async Task RemoveTask()
    {
        await Page.GotoAsync("http://127.0.0.1:5500/Front/Pages/Login/index.html");
        await Page.WaitForLoadStateAsync(LoadState.Load);
        await Page.GetByLabel("Username").ClickAsync();
        await Page.GetByLabel("Username").FillAsync("pwTaskRemoveTest");
        await Page.GetByLabel("Password").ClickAsync();
        await Page.GetByLabel("Password").FillAsync("pwTaskRemoveTest123@");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();
        await Expect(Page).ToHaveTitleAsync("Dashboard");
        await Page.WaitForLoadStateAsync(LoadState.Load);

        await Page.GetByRole(AriaRole.Button, new() { Name = "Tasks" }).ClickAsync();
        await Expect(Page).ToHaveTitleAsync("Tasks");
        await Page.WaitForLoadStateAsync(LoadState.Load);

        void Page_Dialog_EventHandler(object sender, IDialog dialog)
        {
            string expected = "Task je uspesno obrisan";
            string actual = dialog.Message;
            Assert.That(actual, Is.EqualTo(expected));
            dialog.DismissAsync();
            Page.Dialog -= Page_Dialog_EventHandler;
        }
        Page.Dialog += Page_Dialog_EventHandler;
        await Page.GetByRole(AriaRole.Button, new() { Name = "Remove" }).ClickAsync();
    }
}
