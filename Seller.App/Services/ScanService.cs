using AForge.Video.DirectShow;
using System.Collections.Generic;

namespace Seller.App.Services;

public class ScanService
{
    FilterInfoCollection? infoCollection;

    public FilterInfoCollection GetCameraDevices()
    {
        infoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
        return infoCollection;
    }
}