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
    private static Dictionary<string, string> yandexAPIKey = new Dictionary<string, string>() { { "X-Yandex-API-Key", "" } };
    private const string openWeatherAPIUri = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&units=metric&appid={openWeatherAPIKey}";
    private const string openWeatherAPIKey = "";
    private const string weatherBitUri = $"https://api.weatherbit.io/v2.0/current?lat={lat}&lon={lon}&key={weatherBitAPIKey}";
    private const string weatherBitAPIKey = "";

    public ConditionsViewModel()
    {
        conditionsList = new ObservableCollection<WeatherConditions>
        {
            new WeatherConditions { Name = "Yandex", Image = "yandex.png", TemperatureValue = 0},
            new WeatherConditions { Name = "OpenWeather", Image = "openweather.png", TemperatureValue = 0 },
            new WeatherConditions { Name = "WeatherBit", Image = "weatherbit.png", TemperatureValue = 0 },
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
    public async Task OnWeatherUpdateButtonClicked(string name)
    {
        var wc = conditionsList.FirstOrDefault(i => i.Name == name);
        if (wc != null){
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                if (wc.Name == "Yandex")
                {
                    wc.TemperatureValue = await GetYTemperature();
                }
                else if (wc.Name == "OpenWeather")
                {
                    wc.TemperatureValue = await GetOWTemperature();
                }
                else if (wc.Name == "WeatherBit")
                {
                    wc.TemperatureValue = await GetWBTemperature();
                }
                else
                {
                    bool answer = await Shell.Current.DisplayAlert("Uh-oh, no internet", "Would you like to retry?", "Yes", "Cancel", FlowDirection.LeftToRight);
                    if (answer)
                    {
                        await OnWeatherUpdateButtonClicked(name);
                    }
                }
            }
        }
    }

    // TODO 
    // Combine all the GetTemperature() methods into one class with a universal functionality for each service
    // make every method double
    public static async Task<int> GetYTemperature()
    {
        int temperature = 0;
        try
        { 
            HttpServer server = new HttpServer();
            Dictionary<string, object> response = await server.Get(yandexAPIUri, yandexAPIKey);
            var day = JsonConvert.DeserializeObject<Dictionary<string, object>>(response["fact"].ToString());
            temperature = Convert.ToInt32(day["temp"].ToString());
        }
        catch (Exception e)
        {
            System.Diagnostics.Debug.WriteLine($"Something went wrong, caught exception {e}");
        }
        return temperature;
    }
    public static async Task<int> GetOWTemperature()
    {
        int temperature = 0;
        try
        {
            HttpServer server = new HttpServer();
            Dictionary<string, object> response = await server.Get(openWeatherAPIUri);
            var day = JsonConvert.DeserializeObject<Dictionary<string, float>>(response["main"].ToString());
            temperature = (int)Math.Round(day["temp"], 0);
        }
        catch (Exception e)
        {
            System.Diagnostics.Debug.WriteLine($"Something went wrong, caught exception {e}");
        }
        return temperature;
    }
    public static async Task<int> GetWBTemperature()
    {
        int temperature = 0;
        try
        {
            HttpServer server = new HttpServer();
            Dictionary<string, object> response = await server.Get(weatherBitUri);
            var day = JsonConvert.DeserializeObject<Dictionary<string, object>>(response["data"].ToString()[1..^1]);
            temperature = (int)Math.Round(float.Parse(day["temp"].ToString()), 0);
        }
        catch (Exception e)
        {
            System.Diagnostics.Debug.WriteLine($"Something went wrong, caught exception {e}");
        }
        return temperature;
    }
}
