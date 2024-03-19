using Microsoft.Maui.Devices.Sensors;

namespace GeoTracer.Client.Components.Pages;

public partial class CurrentLocation
{
    private CancellationTokenSource _cancelTokenSource;
    private Location? _location;
    private bool _isCheckingLocation;
    string LocationLabel => $"Latitude: {_location.Latitude}, Longitude: {_location.Longitude}, Altitude: {_location.Altitude}";

    protected override async Task OnInitializedAsync()
    {
        _location = await GetCurrentLocation();
    }

    public async Task<Location> GetCurrentLocation()
    {
        try
        {
            _isCheckingLocation = true;

            GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

            _cancelTokenSource = new CancellationTokenSource();

            return await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);
        }
        // Catch one of the following exceptions:
        //   FeatureNotSupportedException
        //   FeatureNotEnabledException
        //   PermissionException
        catch (Exception ex)
        {
            // Unable to get location
        }
        finally
        {
            _isCheckingLocation = false;
        }

        return default;
    }

    public void CancelRequest()
    {
        if (_isCheckingLocation && _cancelTokenSource != null && _cancelTokenSource.IsCancellationRequested == false)
            _cancelTokenSource.Cancel();
    }
}
