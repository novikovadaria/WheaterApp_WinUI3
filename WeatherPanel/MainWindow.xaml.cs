using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WeatherPanel
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            _ = Main();
        }

        async Task Main()
        {
            string apiKey = "0e70393539d5449d92983846242803";
            string city = "Krasnodar";
            string apiUrl = $"http://api.weatherapi.com/v1/current.json?key={apiKey}&q={city}&aqi=no";

            try
            {
                txtCity.Text = city;
                string weatherData = await GetWeatherData(apiUrl);
                // Parse and extract relevant information from the JSON response
                // For simplicity, let's assume you're interested in the temperature only
                string temperature = ParseValues(weatherData, "temp_c");
                txtTemp.Text = "Today : " + temperature.ToString() + " °C";
                string currentStatus = ParseValues(weatherData, "text");
                txtStatus.Text = currentStatus.ToString();
                string precipitation = ParseValues(weatherData, "precip_in");
                txtPrecip.Text = "Precipitation : " + precipitation.Substring(1,precipitation.Length-1).ToString() + " in";
                string wind = ParseValues(weatherData, "wind_mph");
                txtWind.Text = "Wind : " + wind.ToString() + " mph";
                string humidity = ParseValues(weatherData, "humidity");
                txtHumidity.Text = "Humidity : " + humidity.ToString();
                string feelsLike = ParseValues(weatherData, "feelslike_c");
                txtFeelsLike.Text = "Feels Like : " + feelsLike.ToString() + " °C";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        async Task<string> GetWeatherData(string apiUrl)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    
                    return data;
                }
                else
                {
                    throw new HttpRequestException($"Failed to fetch data. Status code: {response.StatusCode}");
                }
            }
        }

        string ParseValues(string jsonData, string field)
        {
            // Parse JSON data to extract the information
            // This depends on the structure of the response from the weather API
            // Here, I'm using a simple string search, which is not robust for all cases
            int startIndex = jsonData.IndexOf("\""+ field + "\":") + ("\"" + field + "\":").Length;
            int endIndex = jsonData.IndexOf(",", startIndex);
            string Str = jsonData.Substring(startIndex, endIndex - startIndex).Trim();
            return Str.Trim('"');
        }

    }
}
