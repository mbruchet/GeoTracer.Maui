using Microsoft.AspNetCore.Components;
using Microsoft.Maui.Devices.Sensors;

namespace GeoTracer.Client.Components.Pages;

public partial class ListenLocation
{
    Location CurrentLocation { get; set; }
    Location InitialLocation { get; set; }
    double Distance { get; set; }
    bool IsRunning { get; set; }
    string Status { get; set; }
    bool Success { get; set; }

    string InitialLocationDisplay => $"Latitude: {InitialLocation.Latitude}, Longitude: {InitialLocation.Longitude}, Altitude: {InitialLocation.Altitude}";
    string CurrentLocationDisplay => $"Latitude: {CurrentLocation.Latitude}, Longitude: {CurrentLocation.Longitude}, Altitude: {CurrentLocation.Altitude}";
    string SpeedDisplay => $"{CurrentLocation.Speed}";

    protected override async Task OnInitializedAsync()
    {
        InitialLocation = await Geolocation.Default.GetLastKnownLocationAsync();
    }

    async void OnStartListening()
    {
        try
        {
            Geolocation.LocationChanged += Geolocation_LocationChanged;
            var request = new GeolocationListeningRequest(GeolocationAccuracy.Medium);
            Success = await Geolocation.StartListeningForegroundAsync(request);
            
            IsRunning = Success;            

            Status = Success
                ? "Started listening for foreground location updates"
                : "Couldn't start listening";
        }
        catch (Exception ex)
        {
            // Unable to start listening for location changes
        }
    }

    void OnStopListening()
    {
        try
        {
            Geolocation.LocationChanged -= Geolocation_LocationChanged;
            Geolocation.StopListeningForeground();
            Status = "Stopped listening for foreground location updates";
            IsRunning = false;
            Success = true;
        }
        catch (Exception ex)
        {
            // Unable to stop listening for location changes
        }
    }

    void Geolocation_LocationChanged(object sender, GeolocationLocationChangedEventArgs e)
    {
        CurrentLocation = e.Location;
        Distance = Location.CalculateDistance(InitialLocation.Latitude, InitialLocation.Longitude, CurrentLocation, DistanceUnits.Kilometers);
        InvokeAsync(StateHasChanged);
    }
}
