using System.Threading.Channels;
using RFIDApi.Models;

public static class RfidSignalRQueue
{
    // เปลี่ยนชื่อตัวแปรเพื่อไม่ให้ชนกับ class Channel
    public static readonly Channel<RFIDTag> SignalChannel = 
        Channel.CreateUnbounded<RFIDTag>();
}