using System;
using System.IO;

namespace Seller.App.Services;

public class TempService : IDisposable
{
    private readonly string RECEIPT = Path.Combine(Path.GetTempPath(), "40c76fb3-3e93-4ad1-b592-f53c83c0bd40.txt");

    public void SaveReceipt()
    {

    }

    public void Dispose()
            => GC.SuppressFinalize(this);
}