using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using System.Net;

namespace TestiranjeAPI.Tests.Backend;

[TestFixture]
public class UserTests : PlaywrightTest
{
    IAPIRequestContext Request;

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
    public async Task UserLogin_ShouldReturnOk()
    {
        await using var response = await Request.PostAsync("/User/login", new()
        {
            Headers = new Dictionary<string, string>() { { "Content-Type", "application/json" } },
            DataObject = new
            {
                username = "makisha",
                password = "makisha"
            }
        });

        Assert.That(response.Status, Is.EqualTo((int)HttpStatusCode.OK));
    }

    [Test]
    public async Task UserLogin_ShouldReturnBadRequest()
    {
        await using var response = await Request.PostAsync("User/login", new()
        {
            Headers = new Dictionary<string, string>() { { "Content-Type", "application/json" } },
            DataObject = new
            {
                username = "makisha",
                password = "wrongpass"
            }
        });

        Assert.That(response.Status, Is.EqualTo((int)HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task UserRegister_ShouldReturnOk()
    {
        await using var response = await Request.PostAsync("User/signup", new()
        {
            Headers = new Dictionary<string, string>() { { "Content-Type", "application/json" } },
            DataObject = new
            {
                username = "okej1",
                email = "okej",
                password = "nesto",
                avatar = "okej"
            }
        });

        Assert.That(response.Status, Is.EqualTo((int)HttpStatusCode.OK));
    }

    [Test]
    public async Task UserRegister_ShouldReturnBadRequest()
    {
        await using var response = await Request.PostAsync("User/signup", new()
        {
            Headers = new Dictionary<string, string>() { { "Content-Type", "application/json" } },
            DataObject = new
            {
                username = "okej",
                email = "okej",
                password = "nesto",
                avatar = "okej"
            }
        });

        Assert.That(response.Status, Is.EqualTo((int)HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task UserUpdate_ShouldReturnOk()
    {

        await using var response = await Request.PutAsync("/User/update/2", new()
        {
            DataObject = new
            {
                username = "makisha",
                password = "novasifra",
                email = "emaaaail",
                avatar = "Ok"
            }
        });
        Assert.That(response.Status, Is.EqualTo((int)HttpStatusCode.OK));
    }

    [Test]
    public async Task UserUpdate_ShouldReturnBadRequest()
    {
        int userId = int.MaxValue;
        await using var response = await Request.PutAsync($"/User/update/{userId}", new()
        {
            DataObject = new
            {
                username = "makisha",
                password = "novasifra",
                email = "emaaaail",
                avatar = "Ok"
            }
        });
        Assert.That(response.Status, Is.EqualTo((int)HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task UserInfo_ShouldReturnOk()
    {
        int userId = 2;
        await using var response = await Request.GetAsync($"/User/info/{userId}");

        Assert.That(response.Status, Is.EqualTo(200));
    }

    [Test]
    public async Task UserInfo_ShouldReturnBadRequest()
    {
        int userId = int.MaxValue;
        await using var response = await Request.GetAsync($"/User/info/{userId}");

        Assert.That(response.Status, Is.EqualTo(400));
    }

    [TearDown]
    public async Task TearDownAPITesting()
    {
        await Request.DisposeAsync();
    }
}
