using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWP391_CareSkin_BE.Data;
using SWP391_CareSkin_BE.Models;

namespace SWP391_CareSkin_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly MyDbContext _context;

        public AdminController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet("User")]
        public IActionResult GetAllUser()
        {
            var userList = _context.Customers.ToList();
            return Ok(userList);
        }


        [HttpGet("UserId")]
        public IActionResult GetUserById(int id)
        {
            var user = _context.Customers.SingleOrDefault(uId => uId.CustomerId == id);
            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete("{userId}")]
        public IActionResult DeleteUserById(int id)
        {
            var user = _context.Customers.SingleOrDefault(uId => uId.CustomerId == id);
            if (user != null)
            {
                _context.Remove(user);
                _context.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, user);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("Staff")]
        public IActionResult GetAllStaff()
        {
            var staffList = _context.Staffs.ToList();
            return Ok(staffList);
        }

        [HttpGet("StaffId")]
        public IActionResult GetStaffById(int id)
        {
            var staff = _context.Staffs.SingleOrDefault(sId => sId.StaffId == id);
            if (staff != null)
            {
                return Ok(staff);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("Staff")]
        public IActionResult AddNewStaff(Staff staff)
        {
            try
            {
                var _staff = new Staff
                {
                    UserName = staff.UserName,
                    Password = staff.Password,
                    FullName = staff.FullName,
                    Email = staff.Email,
                    Phone = staff.Phone,
                    DoB = staff.DoB,
                    ProfilePicture = staff.ProfilePicture,
                };
                _context.Add(staff);
                _context.SaveChanges();
                return StatusCode(StatusCodes.Status201Created, staff);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("Staff")]
        public IActionResult UpdateStaff(int id, Staff staff)
        {
            var _staff = _context.Customers.SingleOrDefault(uId => uId.CustomerId == id);
            if (_staff != null)
            {
                _staff.UserName = _staff.UserName;
                _staff.Password = _staff.Password;
                _staff.FullName = _staff.FullName;
                _staff.Phone = _staff.Phone;
                _staff.Dob = _staff.Dob;
                _staff.ProfilePicture = _staff.ProfilePicture;
                _context.SaveChanges();
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete("Staff{staffId}")]
        public IActionResult DeleteStaffById(int id)
        {
            var staff = _context.Staffs.SingleOrDefault(sId => sId.StaffId == id);
            if (staff != null)
            {
                _context.Remove(staff);
                _context.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, staff);
            }
            else
            {
                return NotFound();
            }
        }

        




    }
}
