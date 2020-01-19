using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MVC_App.Controllers
{
    public class NotificationController : Controller
    {
        public NotificationController()
        {
        }

        [HttpPost]
        [Route("notifications/receive")]
        public void ReceiveNotification([FromBody] string transactionId)
        {

        }
    }
}