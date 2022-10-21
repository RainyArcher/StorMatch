﻿using System.ComponentModel;
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
            new WeatherConditions { Name = "Yandex", Image = "yandex.png", TemperatureContent = "n"},
            new WeatherConditions { Name = "OpenWeather", Image = "openweather.png", TemperatureContent = "n" },
            new WeatherConditions { Name = "WeatherBit", Image = "weatherbit.png", TemperatureContent = "n" }
        };
        averageTemperature = 0;
    }

    [ObservableProperty]
    ObservableCollection<WeatherConditions> conditionsList;

    [ObservableProperty]
    string image;

    [ObservableProperty]
    string name;

    [ObservableProperty]
    string temperatureContent;

    [ObservableProperty]
    int averageTemperature;

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
                    wc.TemperatureContent = await GetYTemperature();
                }
                else if (wc.Name == "OpenWeather")
                {
                    wc.TemperatureContent = await GetOWTemperature();
                }
                else if (wc.Name == "WeatherBit")
                {
                    wc.TemperatureContent = await GetWBTemperature();
                }
                AverageTemperature = changeAverageTemperature();
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

    public static async Task<string> GetYTemperature()
    {
        string temperature = "n";
        try
        { 
            HttpServer server = new HttpServer();
            Dictionary<string, object> response = await server.Get(yandexAPIUri, yandexAPIKey);
            var day = JsonConvert.DeserializeObject<Dictionary<string, object>>(response["fact"].ToString());
            temperature = day["temp"].ToString();
        }
        catch (Exception e)
        {
            System.Diagnostics.Debug.WriteLine($"Something went wrong, caught exception {e}");
        }
        return temperature;
    }
    public static async Task<string> GetOWTemperature()
    {
        string temperature = "n";
        try
        {
            HttpServer server = new HttpServer();
            Dictionary<string, object> response = await server.Get(openWeatherAPIUri);
            var day = JsonConvert.DeserializeObject<Dictionary<string, float>>(response["main"].ToString());
            temperature = (Math.Round(day["temp"], 0)).ToString();
        }
        catch (Exception e)
        {
            System.Diagnostics.Debug.WriteLine($"Something went wrong, caught exception {e}");
        }
        return temperature;
    }
    public static async Task<string> GetWBTemperature()
    {
        string temperature = "n";
        try
        {
            HttpServer server = new HttpServer();
            Dictionary<string, object> response = await server.Get(weatherBitUri);
            var day = JsonConvert.DeserializeObject<Dictionary<string, object>>(response["data"].ToString()[1..^1]);
            temperature = (Math.Round(float.Parse(day["temp"].ToString()), 0)).ToString();
        }
        catch (Exception e)
        {
            System.Diagnostics.Debug.WriteLine($"Something went wrong, caught exception {e}");
        }
        return temperature;
    }
    public int changeAverageTemperature()
    {
        int summOfTemperateitems = 0;
        int validTemperatureItemsAmount = 0;
        foreach (WeatherConditions item in ConditionsList)
        {
            if (item.TemperatureContent != "n")
            {
                summOfTemperateitems += Convert.ToInt32(item.TemperatureContent);
                validTemperatureItemsAmount += 1;
            }
        }
        if (validTemperatureItemsAmount > 0)
        {
            return (int)Math.Round((double)summOfTemperateitems / validTemperatureItemsAmount);
        }
        else
        {
            return 0;
        }
    }
}
