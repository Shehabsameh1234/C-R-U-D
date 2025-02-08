using AutoMapper;
using Demo.BLl.Interfacies;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

using System.Collections.Generic;

namespace Demo.PL.Controllers
{
   
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly IMapper _mapper;

        
        private readonly IunitOfWork _unitOfWork;

        public DepartmentController(IMapper mapper,IunitOfWork unitOfWork)
        {
            _mapper = mapper;
            
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var dep = _unitOfWork.DepartmentRepository.GetAll();
            var mappedDep = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(dep);
            return View(mappedDep);
        }
        
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DepartmentViewModel departmentVm)
        {
            if (ModelState.IsValid)
            {
                var MappedDep = _mapper.Map<DepartmentViewModel, Department>(departmentVm);
                 _unitOfWork.DepartmentRepository.Add(MappedDep);
                _unitOfWork.Copmlete();
               
                return RedirectToAction(nameof(Index));
            }
            return View(departmentVm);
        }
        public IActionResult Details(int? id, string ViewName = "Details")
        {
            if (id == null)
            {
                return BadRequest();
            }
            var dep = _unitOfWork.DepartmentRepository.GetById(id.Value);
            var mappedDep=_mapper.Map<Department,DepartmentViewModel>(dep);
            if (dep == null)
            {
                return NotFound();
            }
            return View(ViewName, mappedDep);
        }
        public IActionResult Edit(int? id)
        {
            return Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, DepartmentViewModel departmentVm)
        {
            if (id != departmentVm.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    var mappedDep = _mapper.Map<DepartmentViewModel, Department>(departmentVm);
                    _unitOfWork.DepartmentRepository.Update(mappedDep);
                    _unitOfWork.Copmlete();
                   
                    return RedirectToAction(nameof(Index));

                }
                catch (Exception ex)
                {
                
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(departmentVm);

        }

        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, DepartmentViewModel departmentVm)
        {
            if (id != departmentVm.Id)
                return BadRequest();

            try
            {
                var MappedDep = _mapper.Map<DepartmentViewModel, Department>(departmentVm);
                _unitOfWork.DepartmentRepository.Delate(MappedDep);
                _unitOfWork.Copmlete();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(departmentVm);

        }
    }
}

