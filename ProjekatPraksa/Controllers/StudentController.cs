using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjekatPraksa.Data;
using ProjekatPraksa.Models;

using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Drawing;

namespace ProjekatPraksa.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly IWebHostEnvironment webHostEnviroment;
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            webHostEnviroment = webHost;
        }

        public IActionResult Index()
        {
            List<StudentEntity> students = _context.Students.ToList();
            return View(students);
        }

        [HttpGet]

        public IActionResult Create() {
            StudentEntity student = new StudentEntity();
            return View();
        }

        [HttpPost]

        public IActionResult Create(StudentEntity student)
        {
            string uniqueFileName = UploadedFile(student);
            student.ImageUrl = uniqueFileName;
            _context.Attach(student);
            _context.Entry(student).State = EntityState.Added;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        private string UploadedFile(StudentEntity student)
        {
            string uniqueFileName = null;

            if(student.Image != null)
            {
                string uploadsFolder = Path.Combine(webHostEnviroment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + student.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    student.Image.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var studentEntity = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentEntity == null)
            {
                return NotFound();
            }

            return View(studentEntity);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var studentEntity = await _context.Students.FindAsync(id);
            if (studentEntity == null)
            {
                return NotFound();
            }
            return View(studentEntity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Mobile,Email,ImageUrl")] StudentEntity studentEntity)
        {
            if (id != studentEntity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studentEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentEntityExists(studentEntity.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(studentEntity);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var studentEntity = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentEntity == null)
            {
                return NotFound();
            }

            return View(studentEntity);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Students == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Students'  is null.");
            }
            var studentEntity = await _context.Students.FindAsync(id);
            if (studentEntity != null)
            {
                _context.Students.Remove(studentEntity);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentEntityExists(int id)
        {
            return (_context.Students?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
