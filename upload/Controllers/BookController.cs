using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using upload.Models;


namespace upload.Controllers
{
    public class BookController : Controller
    {
        // Danh sách sách giả lập (lưu trữ tạm thời)
        private static List<Book> _books = new List<Book>();

        // Index: Hiển thị danh sách sách
        public IActionResult Index()
        {
            return View(_books); // Truyền danh sách sách vào View
        }

        // Create: Hiển thị form thêm sách
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //// Create: Xử lý thêm sách mới
        //[HttpPost]
        //public IActionResult Create(Book book)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Gán Id tự động
        //        book.Id = _books.Count > 0 ? _books.Max(b => b.Id) + 1 : 1;
        //        _books.Add(book); // Thêm sách vào danh sách
        //        return RedirectToAction("Index");
        //    }
        //    return View(book); // Trả lại view nếu dữ liệu không hợp lệ
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book, IFormFile image)
        {
            if (image != null && image.Length > 0)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", image.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
                book.Image = $"/images/{image.FileName}"; // Lưu đường dẫn vào database
            }

            // Lưu book vào database hoặc danh sách
            _books.Add(book);
            return RedirectToAction(nameof(Index));
        }


        // Edit: Hiển thị form chỉnh sửa sách
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book == null) return NotFound();
            return View(book);
        }

        // Edit: Xử lý cập nhật thông tin sách
        [HttpPost]
        public IActionResult Edit(int id, Book updatedBook)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book == null) return NotFound();

            if (ModelState.IsValid)
            {
                book.Title = updatedBook.Title;
                book.Author = updatedBook.Author;
                book.Description = updatedBook.Description;
                book.Price = updatedBook.Price;
                book.Image = updatedBook.Image;
                return RedirectToAction("Index");
            }
            return View(updatedBook);
        }

        // Delete: Hiển thị thông tin sách cần xóa
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book == null) return NotFound();
            return View(book);
        }

        // Delete: Xử lý xóa sách
        //[HttpPost, ActionName("Delete")]
        //public IActionResult DeleteConfirmed(int id)
        //{
        //    var book = _books.FirstOrDefault(b => b.Id == id);
        //    if (book != null) _books.Remove(book);
        //    return RedirectToAction("Index");
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                _books.Remove(book);
            }

            return RedirectToAction("Index");
        }

        // Details: Hiển thị thông tin chi tiết sách
        //    public IActionResult Details(int id)
        //    {
        //        var book = _books.FirstOrDefault(b => b.Id == id);
        //        if (book == null) return NotFound();
        //        return View(book);
        //    }
        public IActionResult Details(int id)
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

    }


}

