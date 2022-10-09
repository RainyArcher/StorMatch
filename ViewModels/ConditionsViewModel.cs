using System.ComponentModel;
using StorMatch.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace StorMatch.Views;
public partial class ConditionsViewModel : ObservableObject
{
    private const string yandexAPIUri = "https://api.weather.yandex.ru/v2/forecast/?lat=55.721560&lon=84.929553&lang=ru_RU";
    private const string yandexAPIKey = "X-Yandex-API-Key";
    private const string yandexAPIValue = "e98d1b89-867f-4e6d-9589-26dc06ea9aee";
    public ConditionsViewModel()
    {
        conditionsList = new ObservableCollection<WeatherConditions>
        {
            new WeatherConditions { Name = "Microsoft", Image = "msft.png", TemperatureValue = 12 },
            new WeatherConditions { Name = "Yandex", Image = "yandex.png", TemperatureValue = 11},
            new WeatherConditions { Name = "Weather.com", Image = "weathercom.png", TemperatureValue = 13 }
        };
    }

    [ObservableProperty]
    ObservableCollection<WeatherConditions> conditionsList;

    [ObservableProperty]
    string image;

    [ObservableProperty]
    string name;

    [ObservableProperty]
    int temperatureValue;

    [RelayCommand]
    public async Task OnWeatherUpdateButtonClicked()
    {
        var item = conditionsList.FirstOrDefault(i => i.Name == "Yandex");
        if (item != null)
        {
            item.TemperatureValue = await GetYTemperature();
        }
    }

    public static async Task<int> GetYTemperature()
    {
        HttpServer server = new HttpServer();
        Dictionary<string, object> response = await server.Get(yandexAPIUri, yandexAPIKey, yandexAPIValue);
        var day = JsonConvert.DeserializeObject<Dictionary<string, object>>(response["fact"].ToString());
        string temperature = day["temp"].ToString();
        return Convert.ToInt32(temperature);
    }
}
