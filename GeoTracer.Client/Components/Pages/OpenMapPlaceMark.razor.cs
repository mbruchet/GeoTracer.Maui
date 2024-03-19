using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Devices.Sensors;

namespace GeoTracer.Client.Components.Pages;

public partial class OpenMapPlaceMark
{
    const string address = "Hitachi Solutions Europe, 34 Av. des Champs-Élysées, 75008 Paris, France";

    async Task VisitAddress()
    {
        var placemark = new Placemark
        {
            CountryName = "France",
            AdminArea = "IDF",
            Thoroughfare = "34 AV des Champs-Élysées",            
            Locality = "Paris",
            PostalCode = "75008"
        };

        var options = new MapLaunchOptions { Name = "Hitachi Solutions France",
            NavigationMode = NavigationMode.Driving
        };

        try
        {
            await Map.Default.OpenAsync(placemark, options);
        }
        catch (Exception ex)
        {
            // No map application available to open or placemark can not be located
        }
    }
}
