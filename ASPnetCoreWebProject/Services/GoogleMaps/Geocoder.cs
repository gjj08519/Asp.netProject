using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaWebsite.Services.GoogleMaps
{
    public interface IGeocoder
    {
        public string GetApiKey();
        public Location GetGeometryLocation(string address);
        public Location GetCompanyGeometryLocation();
        public string GetCompanyAddress();
    }

    public class Geocoder : IGeocoder
    {
        public Geocoder(IOptions<GoogleMapsOptions> options)
        {
            Options = options.Value;
        }

        public GoogleMapsOptions Options { get; set; }

        public string GetApiKey()
        {
            return Options.ApiKey;
        }

        public Location GetGeometryLocation(string address)
        {
            var client = new System.Net.WebClient();

            string apiKey = Options.ApiKey;

            var googleReply = client.DownloadString(string.Format("https://maps.googleapis.com/maps/api/geocode/json?address={0}&key={1}", address, apiKey));

            var reply = JsonConvert.DeserializeObject<GeocodingResponse>(googleReply);

            return reply.Results[0].Geometry.Location;
        }

        public string GetCompanyAddress()
        {
            return Options.CompanyAddress;
        }

        public Location GetCompanyGeometryLocation()
        {
            return GetGeometryLocation(Options.CompanyAddress);
        }
    }

    public class GeocodingResponse
    {
        [JsonProperty("results")]
        public Result[] Results { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public class Result
    {
        [JsonProperty("address_components")]
        public AddressComponents[] AddressComponents { get; set; }

        [JsonProperty("formatted_address")]
        public string FormattedAddress { get; set; }

        [JsonProperty("geometry")]
        public Geometry Geometry { get; set; }

        [JsonProperty("place_id")]
        public string PlaceId { get; set; }

        [JsonProperty("types")]
        public string[] Types { get; set; }
    }

    public class AddressComponents
    {
        [JsonProperty("long_name")]
        public string LongName { get; set; }

        [JsonProperty("short_name")]
        public string ShortName { get; set; }

        [JsonProperty("types")]
        public string[] Types { get; set; }
    }

    public class Geometry
    {
        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("location_type")]
        public string LocationType { get; set; }

        [JsonProperty("viewport")]
        public Viewport Viewport { get; set; }
    }

    public class Location
    {
        [JsonProperty("lat")]
        public float Latitude { get; set; }

        [JsonProperty("lng")]
        public float Longitude { get; set; }
    }

    public class Viewport
    {
        [JsonProperty("northeast")]
        public Northeast NorthEast { get; set; }

        [JsonProperty("southwest")]
        public Southwest SouthWest { get; set; }
    }

    public class Northeast
    {
        [JsonProperty("lat")]
        public float Latitude { get; set; }

        [JsonProperty("lng")]
        public float Longitude { get; set; }
    }

    public class Southwest
    {
        [JsonProperty("lat")]
        public float Latitude { get; set; }

        [JsonProperty("lng")]
        public float Longitude { get; set; }
    }
}
