// testingfingerprint/Controllers/AttendanceController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using testingfingerprint.Services; // Using the new namespace

namespace testingfingerprint.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly ILogger<AttendanceController> _logger;
        private readonly IAttendanceService _attendanceService; // Injecting the service

        public AttendanceController(ILogger<AttendanceController> logger, IAttendanceService attendanceService)
        {
            _logger = logger;
            _attendanceService = attendanceService;
        }

        public IActionResult Index()
        {
            try
            {
                List<AttendanceRecord> attendanceRecords = _attendanceService.FetchAttendance();
                return View(attendanceRecords);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching attendance data: {Message}", ex.Message);
                return View(new List<AttendanceRecord>());
            }
        }
    }
}