using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using System.Text;
using System.Text.Json.Nodes;

namespace TestiranjeAPI.Tests.Backend;

[TestFixture]
public class PartyTests : PlaywrightTest
{
    IAPIRequestContext Request;
    private readonly string MY_PARTIES = "my-parties";
    private readonly string AVAILABLE_PARTIES = "available-parties";
    private readonly string MY_ATTENDING_PARTIES = "my-attending-parties";
    private readonly string PARTIES_NAMES = "parties-names";
    private readonly string CREATE = "create";
    private readonly string ATTEND = "attend";
    private readonly string CANCEL = "cancel";
    private readonly string UNATTEND = "unattend";
    private readonly string EDIT = "edit";

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
    public async Task CreateParty_ShouldReturnOk()
    {
        int userId = 1;
        await using var response = await Request.PostAsync($"/Party/{CREATE}/{userId}", new APIRequestContextOptions

        {
            Headers = new Dictionary<string, string>() { { "Content-Type", "application/json" } },
            DataObject = new
            {
                Name = "Zurka",
                City = "Nis",
                Address = "Bozidarceva",
                Image = "Slika"
            }
        });

        Assert.That(response.Status, Is.EqualTo(200));
    }

    [Test]
    public async Task CreateParty_ShouldReturnBadRequest()
    {
        int userId = 1000;
        await using var response = await Request.PostAsync($"/Party/{CREATE}/{userId}", new APIRequestContextOptions
        {
            Headers = new Dictionary<string, string>() { { "Content-Type", "application/json" } },
            DataObject = new
            {
                Name = "Zurka",
                City = "Nis",
                Address = "Bozidarceva",
                Image = "Slika"
            }
        });

        Assert.That(response.Status, Is.EqualTo(400));
    }

    [Test]
    public async Task CancelParty_ShouldReturnOk()
    {
        int partyId = 1;
        await using var response = await Request.DeleteAsync($"/Party/{CANCEL}/{partyId}");

        Assert.That(response.Status, Is.EqualTo(200));
    }
    [Test]
    public async Task CancelParty_ShouldReturnBadRequest()
    {
        int partyId = 1000;
        await using var response = await Request.DeleteAsync($"/Party/{CANCEL}/{partyId}");

        Assert.That(response.Status, Is.EqualTo(400));
    }

    [Test]
    public async Task EditParty_ShouldReturnOk()
    {
        int partyId = 2;
        await using var response = await Request.PutAsync($"/Party/{EDIT}/{partyId}", new APIRequestContextOptions
        {
            Headers = new Dictionary<string, string>() { { "Content-Type", "application/json" } },
            DataObject = new
            {
                Name = "Zurka",
                City = "Nis",
                Address = "Bozidarceva",
                Image = "Slika"
            }
        });

        Assert.That(response.Status, Is.EqualTo(200));
    }

    [Test]
    public async Task EditParty_ShouldReturnBadRequest()
    {
        int partyId = 1000;
        await using var response = await Request.PutAsync($"/Party/{EDIT}/{partyId}", new APIRequestContextOptions
        {
            Headers = new Dictionary<string, string>() { { "Content-Type", "application/json" } },
            DataObject = new
            {
                Name = "Zurka",
                City = "Nis",
                Address = "Bozidarceva",
                Image = "Slika"
            }
        });

        Assert.That(response.Status, Is.EqualTo(400));
    }

    [Test]
    public async Task MyParties_ShouldReturnOk_AndNonEmptyParties()
    {
        int userId = 1;
        await using var response = await Request.GetAsync($"/Party/{MY_PARTIES}/{userId}");

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
    public async Task MyParties_ShouldReturnOk_AndEmptyParties()
    {
        int userId = 3;
        await using var response = await Request.GetAsync($"/Party/{MY_PARTIES}/{userId}");

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
    public async Task MyParties_ShouldReturnBadRequest()
    {
        int userId = int.MaxValue;
        await using var response = await Request.GetAsync($"/Party/{MY_PARTIES}/{userId}");

        Assert.That(response.Status, Is.EqualTo(400));
    }
    
    [Test]
    public async Task AvailableParties_ShouldReturnOk_AndNonEmptyParties()
    {
        int userId = 1;
        await using var response = await Request.GetAsync($"/Party/{AVAILABLE_PARTIES}");

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
    public async Task AvailablePartiesNamesAndIds_ShouldReturnOk_AndNonEmptyParties()
    {
        int userId = 7;
        await using var response = await Request.GetAsync($"/Party/{PARTIES_NAMES}/{userId}");

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
    public async Task AvailablePartiesNamesAndIds_ShouldReturnOk_AndEmptyParties()
    {
        int userId = 6;
        await using var response = await Request.GetAsync($"/Party/{PARTIES_NAMES}/{userId}");

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
    public async Task AttendParty_ShouldReturnOk()
    {
        int partyId = 6;
        int userId = 3;

        await using var response = await Request.PostAsync($"Party/{ATTEND}/{partyId}/{userId}");

        Assert.That(response.Status, Is.EqualTo(200));
    }

    [Test]
    public async Task AttendParty_ShouldReturnBadRequest()
    {
        int partyId = 1000;
        int userId = 3;

        await using var response = await Request.PostAsync($"Party/{ATTEND}/{partyId}/{userId}");

        Assert.That(response.Status, Is.EqualTo(400));
    }

    [Test]
    public async Task MyAttendingParties_ShouldReturnOk_AndNonEmptyParties()
    {
        int userId = 3;

        await using var response = await Request.GetAsync($"/Party/{MY_ATTENDING_PARTIES}/{userId}");

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
    public async Task MyAttendingParties_ShouldReturnOk_AndEmptyParties()
    {
        int userId = 4;

        await using var response = await Request.GetAsync($"/Party/{MY_ATTENDING_PARTIES}/{userId}");

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
    public async Task UnattendParty_ShouldReturnOk()
    {
        int partyId = 4;
        int userId = 5;

        await using var response = await Request.DeleteAsync($"/Party/{UNATTEND}/{partyId}/{userId}");

        Assert.That(response.Status, Is.EqualTo(200));
    }

    [Test]
    public async Task UnattendParty_ShouldReturnBadRequest()
    {
        int partyId = 1000;
        int userId = 1000;

        await using var response = await Request.DeleteAsync($"/Party/{UNATTEND}/{partyId}/{userId}");

        Assert.That(response.Status, Is.EqualTo(400));
    }

    [Test]
    public async Task PartiesNames_ShouldReturnOk()
    {
        int userId = 2;

        await using var response = await Request.GetAsync($"/Party/{PARTIES_NAMES}/{userId}");

        Assert.That(response.Status, Is.EqualTo(200));
    }

    [TearDown]
    public async Task TearDownAPITesting()
    {
        await Request.DisposeAsync();
    }
}
