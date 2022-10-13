using System.ComponentModel;
using StorMatch.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace StorMatch.Views;
public partial class ConditionsViewModel : ObservableObject
{
    private const string lat = ""; // Latitude
    private const string lon = ""; // Longitude
    private const string yandexAPIUri = $"https://api.weather.yandex.ru/v2/forecast/?lat={lat}&lon={lon}&lang=ru_RU";
    private const string yandexAPIKey = "X-Yandex-API-Key";
    private const string yandexAPIValue = "";
    private const string openWeatherAPIUri = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&units=metric&appid={openWeatherAPIKey}";
    private const string openWeatherAPIKey = "";

    public ConditionsViewModel()
    {
        conditionsList = new ObservableCollection<WeatherConditions>
        {
            new WeatherConditions { Name = "Yandex", Image = "yandex.png", TemperatureValue = 0},
            new WeatherConditions { Name = "OpenWeather", Image = "openweather.png", TemperatureValue = 0 }
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
        foreach (var item in ConditionsList)
        {
            if (item != null)
            {
                NetworkAccess accessType = Connectivity.Current.NetworkAccess;

                if (accessType == NetworkAccess.Internet)
                {
                    if (item.Name == "Yandex")
                    {
                        item.TemperatureValue = await GetYTemperature();
                    }
                    else if (item.Name == "OpenWeather")
                    {
                        item.TemperatureValue = await GetOWTemperature();
                    }
                }
                else
                {
                    bool answer = await Shell.Current.DisplayAlert("Uh-oh, no internet", "Would you like to retry?", "Yes", "Cancel", FlowDirection.LeftToRight);
                    if (answer)
                    {
                        await OnWeatherUpdateButtonClicked();
                    }
                }
            }
        }
    }

    // TODO 
    // Combine all the GetTemperature() methods into one class with a universal functionality for each service
    public static async Task<int> GetYTemperature()
    {
        HttpServer server = new HttpServer();
        Dictionary<string, object> response = await server.Get(yandexAPIUri, yandexAPIKey, yandexAPIValue);
        var day = JsonConvert.DeserializeObject<Dictionary<string, object>>(response["fact"].ToString());
        string temperature = day["temp"].ToString();
        return Convert.ToInt32(temperature);
    }
    public static async Task<int> GetOWTemperature()
    {
        HttpServer server = new HttpServer();
        Dictionary<string, object> response = await server.Get(openWeatherAPIUri);
        var day = JsonConvert.DeserializeObject<Dictionary<string, float>>(response["main"].ToString());
        string temperature = ((int)Math.Round(day["temp"], 0)).ToString();
        return Convert.ToInt32(temperature);
    }
}
