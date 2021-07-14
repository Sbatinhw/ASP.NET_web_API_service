using System;

namespace WeatherApi
{
    public class WeatherForecast
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }

        public WeatherForecast()
        {
            var rng = new Random();
            Date = DateTime.Now.AddDays(rng.Next(1, 10));
            TemperatureC = rng.Next(-20, 55);
            Summary = Summaries[rng.Next(Summaries.Length)];

        }

        public WeatherForecast(DateTime dateTime)
        {
            Date = dateTime;
            var rng = new Random();
            TemperatureC = rng.Next(-20, 55);
            Summary = Summaries[rng.Next(Summaries.Length)];
        }


    }
}
