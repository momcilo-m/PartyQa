using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using System.Text.Json.Nodes;
using System.Text;

namespace TestiranjeAPI.Tests.Backend;

[TestFixture]
public class TaskTests : PlaywrightTest
{
    IAPIRequestContext Request;
    private readonly string MY_TASKS = "my-tasks";
    private readonly string CREATE = "create";
    private readonly string EDIT = "edit";
    private readonly string REMOVE = "remove";

    [SetUp]
    public async Task SetupAPITesting()
    {
        var headers = new Dictionary<string, string>
        {
            {"Accept", "*/*" }
        };

        Request = await Playwright.APIRequest.NewContextAsync(new APIRequestNewContextOptions
        {
            BaseURL = "http://localhost:5062",
            ExtraHTTPHeaders = headers,
            IgnoreHTTPSErrors = true
        });
    }

    [Test]
    public async Task MyTasks_ShouldReturnOk_AndNonEmptyTasks()
    {
        int userId = 8;
        await using var response = await Request.GetAsync($"/Task/{MY_TASKS}/{userId}");

        var body = await response.BodyAsync();
        var jsonString = Encoding.UTF8.GetString(body);
        var resultArr = JsonNode.Parse(jsonString)!.AsArray();

        Assert.Multiple(() =>
        {
            Assert.That(response.Status, Is.EqualTo(200));
            Assert.That(resultArr.Count, Is.Not.EqualTo(0));
        });
    }

    [Test]
    public async Task My_Tasks_ShouldReturnOk_AndEmptyTasks()
    {
        int userId = 9;
        await using var response = await Request.GetAsync($"/Task/{MY_TASKS}/{userId}");

        var body = await response.BodyAsync();
        var jsonString = Encoding.UTF8.GetString(body);
        var resultArr = JsonNode.Parse(jsonString)!.AsArray();

        Assert.Multiple(() =>
        {
            Assert.That(response.Status, Is.EqualTo(200));
            Assert.That(resultArr.Count, Is.EqualTo(0));
        });
    }

    [Test]
    public async Task My_Tasks_ShouldReturnBadRequests()
    {
        int userId = int.MaxValue;
        await using var response = await Request.GetAsync($"/Task/{MY_TASKS}/{userId}");

        Assert.That(response.Status, Is.EqualTo(400));
    }

    [Test]
    public async Task CreateTask_ShouldReturnOk()
    {
        int userId = 10;
        int partyId = 9;
        await using var response = await Request.PostAsync($"/Task/{CREATE}/{userId}/{partyId}", new APIRequestContextOptions
        {
            Headers = new Dictionary<string, string>() { { "Content-Type", "application/json" } },
            DataObject = new
            {
                Name = "Test",
                Description = "Test"
            }
        });

        Assert.That(response.Status, Is.EqualTo(200));
    }

    [Test]
    public async Task CreateTask_ShouldReturnBadRequest()
    {
        int userId = 1000;
        int partyId = 1000;
        await using var response = await Request.PostAsync($"/Task/{CREATE}/{userId}/{partyId}", new APIRequestContextOptions
        {
            Headers = new Dictionary<string, string>() { { "Content-Type", "application/json" } },
            DataObject = new
            {
                Name = "Test",
                Description = "Test"
            }
        });

        Assert.That(response.Status, Is.EqualTo(400));
    }

    [Test]
    public async Task EditTask_ShouldReturnOk()
    {
        int taskId = 2;
        await using var response = await Request.PutAsync($"/Task/{EDIT}/{taskId}", new APIRequestContextOptions
        {
            Headers = new Dictionary<string, string>() { { "Content-Type", "application/json" } },
            DataObject = new
            {
                Name = "Test",
                Description = "Test"
            }
        });

        Assert.That(response.Status, Is.EqualTo(200));
    }

    [Test]
    public async Task EditTask_ShouldReturnBadRequest()
    {
        int taskId = 1000;
        await using var response = await Request.PutAsync($"/Task/{EDIT}/{taskId}", new APIRequestContextOptions
        {
            Headers = new Dictionary<string, string>() { { "Content-Type", "application/json" } },
            DataObject = new
            {
                Name = "Test",
                Description = "Test"
            }
        });

        Assert.That(response.Status, Is.EqualTo(400));
    }

    [Test]
    public async Task RemoveTask_ShouldReturnOk()
    {
        int taskId = 3;
        await using var resposne = await Request.DeleteAsync($"/Task/{REMOVE}/{taskId}");

        Assert.That(resposne.Status, Is.EqualTo(200));
    }

    [Test]
    public async Task RemoveTask_ShouldReturnBadRequest()
    {
        int taskId = 1000;
        await using var resposne = await Request.DeleteAsync($"/Task/{REMOVE}/{taskId}");

        Assert.That(resposne.Status, Is.EqualTo(400));
    }

    [TearDown]
    public async Task TearDownAPITesting()
    {
        await Request.DisposeAsync();
    }
}
