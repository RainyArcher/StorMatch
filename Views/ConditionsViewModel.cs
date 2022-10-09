using System.ComponentModel;
using StorMatch.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using StorMatch;

namespace StorMatch.Views;
public partial class ConditionsViewModel : ObservableObject
{
    private const string yandexAPIKey = "e98d1b89-867f-4e6d-9589-26dc06ea9aee";
    public ConditionsViewModel()
    {
        ConditionsList = new ObservableCollection<WeatherConditions>
        {
            new WeatherConditions { Name = "Microsoft", Image = "msft.png", TemperatureValue = 12 },
            new WeatherConditions { Name = "Yandex", Image = "yandex.png", TemperatureValue = 11 },
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
}
