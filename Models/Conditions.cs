using CommunityToolkit.Mvvm.ComponentModel;

namespace StorMatch.Models
{
    public class WeatherConditions : ObservableObject
    {
        private string name;
        private string image;
        private int temperatureValue;
        public string Name { get => name; set => SetProperty(ref name, value); }
        public string Image { get => image; set => SetProperty(ref image, value); }
        public int TemperatureValue { get => temperatureValue; set => SetProperty(ref temperatureValue, value); }
    }
}