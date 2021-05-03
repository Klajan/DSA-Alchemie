using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Alchemie
{
    internal class UpdateChecker
    {
        private static readonly HttpClient _client = new();
        private static readonly Version _version = Assembly.GetExecutingAssembly().GetName().Version;
        public static string CurrentVersion { get => _version.ToString(); }
        private const string _owner = "Klajan";
        private const string _repo = "DSA-Alchemie";
        private static readonly Uri _gitAPIuri = new($"https://api.github.com/repos/{_owner}/{_repo}/releases", UriKind.Absolute);

        public static async Task<Release> CheckUpdateAvailable()
        {
            var releases = await GetReleasesAsync().ConfigureAwait(false);
            foreach (Release release in releases)
            {
                if (!release.Prerelease | Alchemie.Properties.Settings.Default.CheckForPrerelease)
                {
                    var version = Release.ParseVersion(release.Tag);
                    if (_version.CompareTo(version) < 0)
                    {
                        return release;
                    }
                }
            }
            return null;
        }

        private static async Task<List<Release>> GetReleasesAsync()
        {
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            _client.DefaultRequestHeaders.Add("User-Agent", ".NET DSA-Alchemie Update Checker");

            var streamTask = _client.GetStreamAsync(_gitAPIuri);
            var releases = await JsonSerializer.DeserializeAsync<List<Release>>(await streamTask.ConfigureAwait(false)).ConfigureAwait(false);
            return releases;
        }
    }

    public class Release
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }

        [JsonPropertyName("html_url")]
        public Uri Url { get; set; }

        [JsonPropertyName("tag_name")]
        public string Tag { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("body")]
        public string Body { get; set; }

        [JsonPropertyName("prerelease")]
        public bool Prerelease { get; set; }

        [JsonPropertyName("published_at")]
        public DateTime PublishedAt { get; set; }

        public static Version ParseVersion(string version)
        {
            Regex regex = new("([0-9]+.){2,3}([0-9]+)?");
            var match = regex.Match(version);
            if (match.Success)
            {
                return Version.Parse(match.Value);
            }
            return null;
        }
    }
}