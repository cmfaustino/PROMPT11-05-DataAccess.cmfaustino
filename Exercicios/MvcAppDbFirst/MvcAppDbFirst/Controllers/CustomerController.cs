using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcAppDbFirst.Models;

namespace MvcAppDbFirst.Controllers
{   
    public class CustomerController : Controller
    {
        private AdventureWorksLTEntities context = new AdventureWorksLTEntities(); // ALTERACAO: ALTERADO O TIPO E O CONSTRUTOR

        //
        // GET: /Customer/

        public ViewResult Index()
        {
            return View(context.Customer.ToList());
        }

        //
        // GET: /Customer/Details/5

        public ViewResult Details(int id)
        {
            Customer customer = context.Customer.Single(x => x.CustomerID == id);
            return View(customer);
        }

        //
        // GET: /Customer/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Customer/Create

        [HttpPost]
        public ActionResult Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                context.Customer.Add(customer);
                context.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(customer);
        }
        
        //
        // GET: /Customer/Edit/5
 
        public ActionResult Edit(int id)
        {
            Customer customer = context.Customer.Single(x => x.CustomerID == id);
            return View(customer);
        }

        //
        // POST: /Customer/Edit/5

        [HttpPost]
        public ActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                context.Entry(customer).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        //
        // GET: /Customer/Delete/5
 
        public ActionResult Delete(int id)
        {
            Customer customer = context.Customer.Single(x => x.CustomerID == id);
            return View(customer);
        }

        //
        // POST: /Customer/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = context.Customer.Single(x => x.CustomerID == id);
            context.Customer.Remove(customer);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}