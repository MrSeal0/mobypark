using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class RdwAuto
{
    public string kenteken { get; set; }
    public string merk { get; set; }
    public string handelsbenaming { get; set; }
    public string eerste_kleur { get; set; }
    public string datum_eerste_toelating { get; set; }
}

public static class RdwService
{
    private static readonly HttpClient _httpClient = new HttpClient();

    public static async Task<RdwAuto?> GetAutoByKentekenAsync(string kenteken)
    {
        string url = $"https://opendata.rdw.nl/resource/m9d7-ebf2.json" +
                     $"?$select=kenteken,merk,handelsbenaming,eerste_kleur,datum_eerste_toelating" +
                     $"&kenteken={kenteken}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        var autos = JsonSerializer.Deserialize<RdwAuto[]>(json);

        return autos?.Length > 0 ? autos[0] : null;
    }
}
