using AutoMapper;
using Demo.BLl.Interfacies;
using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IunitOfWork _unitOfWork;

 
        public EmployeeController(IMapper mapper,IunitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            
        }
        public IActionResult Index()
        {

          

                
                var Emp = _unitOfWork.EmployeeRepository.GetAll();
                var mappedEmp = _mapper.Map<IEnumerable<Employee>,IEnumerable<EmpViewModel>>(Emp);
                return View(mappedEmp);
            
            
            
        }
        public IActionResult Search(string SearchInpId)
        {
            if (string.IsNullOrEmpty(SearchInpId))
            {

                var Emp = _unitOfWork.EmployeeRepository.GetAll();
                var mappedEmp = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmpViewModel>>(Emp);
                return PartialView("EmpTablePartialView", mappedEmp);

            }
            else
            {
                var emp = _unitOfWork.EmployeeRepository.GetEmpByName(SearchInpId.ToLower());
                var mappedEmp = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmpViewModel>>(emp);
                return PartialView("EmpTablePartialView", mappedEmp);
            }
            
        }
        public IActionResult Create()
        {
            
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EmpViewModel EmployeeVm)
        {
            if (ModelState.IsValid) //server side validation
            {

               if(EmployeeVm.Image !=null)
                {
                    string fileExtension = Path.GetExtension(EmployeeVm.Image.FileName);
                    if (fileExtension != ".jpg")
                    {
                        ViewData["messege"] = "Please upload file with .jpg extension only.";
                        return View(EmployeeVm);
                    }
                    else
                    {
                        ViewData["messege"] = string.Empty;
                        EmployeeVm.ImageName = DocumentSetting.UploadFile(EmployeeVm.Image, "Images");
                    }
                }
               else
                {
                    ViewData["messege"] = "please choose your photo";
                    return View(EmployeeVm);
                }

                var mappedEmp=_mapper.Map<EmpViewModel,Employee>(EmployeeVm);
                _unitOfWork.EmployeeRepository.Add(mappedEmp);
                _unitOfWork.Copmlete();
               
                return RedirectToAction(nameof(Index));
            }
            
            return View(EmployeeVm);
        }
        public IActionResult Details(int? id, string ViewName = "Details")
        {
            if (id == null)
            {
                return BadRequest();
            }
            var Emp = _unitOfWork.EmployeeRepository.GetById(id.Value);
            var mappedEmp = _mapper.Map<Employee, EmpViewModel>(Emp);
            if (Emp == null)
            {
                return NotFound();
            }
            
            TempData["imageName"] = mappedEmp.ImageName;
            return View(ViewName, mappedEmp);
        }
        public IActionResult Edit(int? id)
        {
          
            return Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, EmpViewModel EmployeeVm)
        {
            if(TempData["imageName"]!=null)
            {
                DocumentSetting.DeleteFile(TempData["imageName"] as string, "Images");
            }
            
            if (id != EmployeeVm.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                if (EmployeeVm.Image != null)
                {
                    string fileExtension = Path.GetExtension(EmployeeVm.Image.FileName);
                    if (fileExtension != ".jpg")
                    {
                        ViewData["messege"] = "Please upload file with .jpg extension only.";
                        return View(EmployeeVm);
                    }
                    else
                    {
                        ViewData["messege"] = string.Empty;
                        EmployeeVm.ImageName = DocumentSetting.UploadFile(EmployeeVm.Image, "Images");
                    }
                }
                else
                {
                    ViewData["messege"] = "please choose your photo";
                    return View(EmployeeVm);
                }
                try
                {
                    var mappedEmp = _mapper.Map<EmpViewModel, Employee>(EmployeeVm);
                     _unitOfWork.EmployeeRepository.Update(mappedEmp);
                    _unitOfWork.Copmlete();
                    
                    return RedirectToAction(nameof(Index));

                }
                catch (Exception ex)
                {
                    
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(EmployeeVm);

        }

        public IActionResult Delete(int? id)
        {
           
            return Details(id, "Delete");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, EmpViewModel EmployeeVm)
        {
            
            if (id != EmployeeVm.Id)
                return BadRequest();

            try
            {
                var mappedEmp = _mapper.Map<EmpViewModel, Employee>(EmployeeVm);
                if (EmployeeVm.ImageName != null)
                {
                    DocumentSetting.DeleteFile(EmployeeVm.ImageName, "Images");
                }
                _unitOfWork.EmployeeRepository.Delate( mappedEmp);
                _unitOfWork.Copmlete();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            
            return View(EmployeeVm);

        }
    }
}
 