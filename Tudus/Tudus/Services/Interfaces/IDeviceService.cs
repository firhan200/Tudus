using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tudus.Services.Interfaces
{
    public interface IDeviceService
    {
        void SetNotification(DateTime time, string title, string description, int todoId);

        string GetLocalFilePath(string filename);

        void ShowToast(string text);

        void CancelNotification(int todoId);
    }
}
