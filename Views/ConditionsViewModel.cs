using System.ComponentModel;
using StorMatch.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace StorMatch.Views;
public partial class ConditionsViewModel : ObservableObject
{
    public ConditionsViewModel()
    {
        ConditionsList = new ObservableCollection<WeatherConditions>();
        ConditionsList.Add(new WeatherConditions{ Name = "Microsoft", Image = "msft.png", TemperatureValue = 12});
        ConditionsList.Add(new WeatherConditions { Name = "Yandex", Image = "yandex.png", TemperatureValue = 11 });
        ConditionsList.Add(new WeatherConditions { Name = "Weather.com", Image = "weathercom.png", TemperatureValue = 13 });
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
