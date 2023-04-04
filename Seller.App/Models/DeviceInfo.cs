using System.Management;
using System.Threading.Tasks;

namespace Seller.App.Models;

public static class DeviceInfo
{
    public static async Task<string> ReturnHardWareID()
    {
        string s = "";
        Task task = Task.Run(() =>
        {
            ManagementObjectSearcher bios = new ManagementObjectSearcher("SELECT * FROM Win32_BIOS");
            ManagementObjectCollection bios_Collection = bios.Get();
            foreach (ManagementObject obj in bios_Collection)
            {
                s = obj["SerialNumber"].ToString();
                break; //break just to get the first found object data only
            }
        });
        Task.WaitAll(task);

        return await Task.FromResult(s);
    }
}