// testingfingerprint/Services/AttendanceService.cs
using System;
using System.Collections.Generic;
using zkemkeeper;

namespace testingfingerprint.Services
{
    public interface IAttendanceService
    {
        List<AttendanceRecord> FetchAttendance();
    }

    public class AttendanceService : IAttendanceService
    {
        private readonly string ipAddress = "10.84.172.201";
        private readonly int port = 4370;
           
        public List<AttendanceRecord> FetchAttendance()
        {
            List<AttendanceRecord> attendanceList = new();
            //CZKEM axCZKEM = new();
              CZKEM axCZKEM = new CZKEM();



            var conn = axCZKEM.Connect_Net(ipAddress, port);

            if (conn)
            {
                axCZKEM.EnableDevice(1, false);

                if (axCZKEM.ReadGeneralLogData(1))
                {
                    string enrollNumber;
                    int verifyMode, inOutMode, year, month, day, hour, minute, second, workCode = 0;

                    while (axCZKEM.SSR_GetGeneralLogData(1, out enrollNumber, out verifyMode, out inOutMode,
                                                                out year, out month, out day, out hour, out minute, out second, ref workCode))
                    {
                        attendanceList.Add(new AttendanceRecord
                        {
                            UserId = enrollNumber,
                            DateTime = new DateTime(year, month, day, hour, minute, second),
                            VerifyMode = verifyMode,
                            InOutMode = inOutMode
                        });
                    }
                }

                axCZKEM.EnableDevice(1, true);
                axCZKEM.Disconnect();
            }

            return attendanceList;
        }
    }
      
    public class AttendanceRecord
    {
        public string? UserId { get; set; }
        public DateTime DateTime { get; set; }
        public int VerifyMode { get; set; }
        public int InOutMode { get; set; }
    }
}