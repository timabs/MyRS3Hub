using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

public class SkillValue
{
    [JsonPropertyName("level")]
    public int Level { get; set; }

    [JsonPropertyName("xp")]
    public long Xp { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }
    public string? Name { get; set; }
}

public class Activity
{
    [JsonPropertyName("date")]
    public string? Date { get; set; }

    [JsonPropertyName("details")]
    public string? Details { get; set; }

    [JsonPropertyName("text")]
    public string? Text { get; set; }
}

public class ProfileData
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("totalxp")]
    public int? TotalXp { get; set; }

    [JsonPropertyName("combatlevel")]
    public int? CombatLevel { get; set; }
    public string? AvatarUrl { get; set; }

    [JsonPropertyName("skillvalues")]
    public List<SkillValue>? SkillValues { get; set; }

    [JsonPropertyName("activities")]
    public List<Activity>? Activities { get; set; }
}

public class ProfileDataService
{
    public ProfileData? ProfileData { get; private set; }

    private readonly IHttpClientFactory _clientFactory;

    public ProfileDataService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public event Action? OnProfileDataUpdated;
    public static readonly Dictionary<int, string> SkillNames = new Dictionary<int, string>
    {
        { 0, "Attack" },
        { 1, "Defence" },
        { 2, "Strength" },
        { 3, "Constitution" },
        { 4, "Ranged" },
        { 5, "Prayer" },
        { 6, "Magic" },
        { 7, "Cooking" },
        { 8, "Woodcutting" },
        { 9, "Fletching" },
        { 10, "Fishing" },
        { 11, "Firemaking" },
        { 12, "Crafting" },
        { 13, "Smithing" },
        { 14, "Mining" },
        { 15, "Herblore" },
        { 16, "Agility" },
        { 17, "Thieving" },
        { 18, "Slayer" },
        { 19, "Farming" },
        { 20, "Runecrafting" },
        { 21, "Hunter" },
        { 22, "Construction" },
        { 23, "Summoning" },
        { 24, "Dungeoneering" },
        { 25, "Divination" },
        { 26, "Invention" },
        { 27, "Archaeology" },
        { 28, "Necromancy" }
    };

    public async Task GetProfile(string userName)
    {
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"https://apps.runescape.com/runemetrics/profile/profile?user={userName}&activities=20"
        );

        var client = _clientFactory.CreateClient();

        var response = await client.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            ProfileData = System.Text.Json.JsonSerializer.Deserialize<ProfileData>(jsonResponse);
            ProfileData.AvatarUrl = $"http://secure.runescape.com/m=avatar-rs/{userName}/chat.png";
            if (ProfileData?.SkillValues != null)
            {
                // Map the skill IDs to names
                foreach (var skill in ProfileData.SkillValues)
                {
                    skill.Name = SkillNames.ContainsKey(skill.Id)
                        ? SkillNames[skill.Id]
                        : "Unknown";
                }
            }
            OnProfileDataUpdated?.Invoke();
        }
        else
        {
            Console.Write("Error fetching profile");
        }
    }
}
