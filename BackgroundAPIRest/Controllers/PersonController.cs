//using Business.Main.IbnorcaContext;
using Business.Main.ModuloSample;
using Domain.Main.sample;
using Domain.Main.Wraper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlumbingProps.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackgroundAPIRest.Controllers
{
    [Route("api/Person")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        [HttpGet]
        public string index(string name)
        {
            return "API para la gestion de personas";
        }

        //[HttpGet("GetPersonsTest")]
        //[EnableCors("MyPolicy")]
        //public ResponseQuery<PersonReport> GetPersonsTest(string name)
        //{
        //    Binnacle.ProcessEvent(new Event { category = Event.Category.Information, description = $"Metodo GetPersonsTest llamdo con parametro {name}" });
        //    PersonManager personManager = new PersonManager();
        //    return personManager.GetPersons(name);
        //}

        //[HttpPost("PersonRegisterTest")]
        //public ResponseObject<Person> PersonRegisterTest(Person person)
        //{
        //    PersonManager personManager = new PersonManager();
        //    return personManager.RegisterPerson(person);
        //}

        //[HttpPost("DeletePerson")]
        //public ResponseObject<Person> DeletePerson(Person person)
        //{
        //    PersonManager personManager = new PersonManager();
        //    return personManager.DeletePerson(person);
        //}
    }
}
