using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Models;
using SalesWebMvc.Services;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services.Exceptions;
using System.Diagnostics;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;

        public SellersController(SellerService sellerService, DepartmentService departmentService) {
            _sellerService = sellerService;
            _departmentService = departmentService;
        }
         
        // GET: Sellers
        public async Task<IActionResult> Index() {
            var list = await _sellerService.FindAll();
            return View(list);
        }

        // GET: Sellers/Create
        public async Task<IActionResult> Create() {
            var departments = await _departmentService.FindAll();
            var viewModel = new SellerViewModel { Departments = departments };
            return View(viewModel);
        }
        
        // POST: Sellers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Seller seller) {
            //if (!ModelState.IsValid) {
            //    var departments = await _departmentService.FindAll();
            //    SellerViewModel viewModel = new SellerViewModel { Seller = seller, Departments = departments };
            //    return View(viewModel);
            //}

            await _sellerService.Insert(seller);
            return RedirectToAction(nameof(Index));
        }

        // GET: Sellers/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            if (id == null) {
                return RedirectToAction(nameof(Error), new {message="Id not provided"});
            }

            var seller = await _sellerService.FindById(id.Value);
            if (seller == null) {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            return View(seller);
        }

        // POST: Sellers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            try {
                await _sellerService.Remove(id);
                return RedirectToAction(nameof(Index));
            } catch (IntegrityException e) {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        
        // GET: Sellers/Details/5
        public async Task<IActionResult> Details(int? id) {
            if (id == null) {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var seller = await _sellerService.FindById(id.Value);
            if (seller == null) {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            return View(seller);
        }
        
        // GET: Sellers/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            if (id == null) {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var seller =  await _sellerService.FindById(id.Value);
            if (seller == null) {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            var departments = await _departmentService.FindAll();
            SellerViewModel viewModel = new SellerViewModel { Seller = seller, Departments = departments };

            return View(viewModel);
        }

        // POST: Sellers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Seller seller) {
            //if (!ModelState.IsValid) {
            //    var departments = await _departmentService.FindAll();
            //    SellerViewModel viewModel = new SellerViewModel { Seller = seller, Departments = departments };
            //    return View(viewModel);
            //}

            if (id != seller.Id) {
                return RedirectToAction(nameof(Error), new { message = "Id mismatch" });
            }
            
            await _sellerService.Update(seller);
            try {
                return RedirectToAction(nameof(Index));
            } catch (ApplicationException e) {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
            
            return View(seller);
        }

        public IActionResult Error(string message) {
            var viewModel = new ErrorViewModel { Message = message, RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
            return View(viewModel);
        }
    }
}
