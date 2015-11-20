using MapperNet.WebDemo.Mappers.Mysql;
using MapperNet.WebDemo.Models.Mysql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MapperNet.WebDemo.Controllers
{
    public class MysqlController : Controller
    {
        public PersonMapper PersonMapper { get; private set; }

        public MysqlController()
        {
            this.PersonMapper = new PersonMapper();
        }

        // GET: Mysql
        public ActionResult Index()
        {
            return View();
        }

        #region Manage Person Entity

        public ActionResult People()
        {
            var people = PersonMapper.Query();
            return View(people);
        }

        public ActionResult CreatePerson()
        {
            var person = new Person();
            return View(person);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePerson(Person person)
        {
            if (ModelState.IsValid)
            {
                var dateDiff = (DateTime.Now - person.DateOfBirth);
                person.Age = new DateTime(dateDiff.Ticks).Year;

                try
                {
                    PersonMapper.Insert(person);
                    return RedirectToAction("People", "Mysql");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            ModelState.AddModelError("", "There is error in your form. Please check it before save");
            return View(new Person());
        }

        public ActionResult PersonDetails(int id)
        {
            var person = PersonMapper.Query().Where(p => p.Id == id).FirstOrDefault();

            if (person == null)
            {
                return RedirectToAction("People", "Mysql");
            }

            return View(person);
        }

        public ActionResult EditPerson(int id)
        {
            var person = PersonMapper.Query(string.Format("SELECT * FROM {0} WHERE person_id=@personId", PersonMapper.TableName), new Dictionary<string, object>()
            {
                { "personId", id }
            }).FirstOrDefault();

            if (person == null)
            {
                return RedirectToAction("People", "Mysql");
            }

            return View(person);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditPerson(Person person)
        {
            if (ModelState.IsValid)
            {
                var dateDiff = (DateTime.Now - person.DateOfBirth);
                person.Age = new DateTime(dateDiff.Ticks).Year;

                try
                {
                    PersonMapper.Update(person);
                    return RedirectToAction("PersonDetails", "Mysql", new { @id = person.Id });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            ModelState.AddModelError("", "There is error in your form. Please check it before save");
            return View(person);
        }

        public ActionResult DeletePerson(int id)
        {
            var person = PersonMapper.Query(string.Format("SELECT * FROM {0} WHERE person_id=@personId", PersonMapper.TableName), new Dictionary<string, object>()
            {
                { "personId", id }
            }).FirstOrDefault();

            if (person == null)
            {
                return RedirectToAction("People", "Mysql");
            }

            return View(person);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult DeletePerson(Person person)
        {
            try
            {
                PersonMapper.Delete(person);
                return RedirectToAction("People", "Mysql");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(person);
            }
        }

        #endregion
    }
}