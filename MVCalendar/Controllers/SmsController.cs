using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MVCalendar.Domains;
using MVCalendar.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace MVCalendar.Controllers
{
    public class SmsController : Twilio.AspNet.Core.TwilioController
    {
        private static readonly NLog.Logger NLogger = NLog.LogManager.GetCurrentClassLogger();
        private readonly IFamilyService _familyService;
        private readonly IEventService _eventService;
        public IConfiguration Configuration { get; set; }
        public SmsController(IConfiguration config, IFamilyService familyService, IEventService eventService)
        {
            Configuration = config;
            _familyService = familyService;
            _eventService = eventService;
        }
        // method receive event ID, retrive event from DB and pass to SmsMessage method
        public async Task SmsEvent(Guid id)
        {
            Event smsEvent = _eventService.GetEvent(id);
            await SmsMessage(smsEvent);
        }

        // method calls to event service method to collect all events pending for reminder, pass them to SmsMessage method
        public async Task SmsReminders()
        {
            var events = await _eventService.CollectEventsReminders();
            foreach (var item in events)
            {
                await SmsMessage(item);
            }
        }

        // method generate sms mesage text, retrive event user, who need to receive SMS and pass it to SendSms method
        private async Task SmsMessage(Event smsEvent)
        {
            CalendarUser userFrom = await _familyService.GetUser(smsEvent.MessageFrom);
            string smsText = smsEvent.Subject + " " + smsEvent.Description + " " + userFrom.Name;

            if (smsEvent.SendToAll)// check if all group members should receive sms message
            {
                var users = await _familyService.GetFamilyUsers(smsEvent.Family.ID);
                foreach (var member in users)
                {
                    SendSms(member, smsText);
                }
            }
            else
            {
                CalendarUser user = await _familyService.GetUser(smsEvent.MesageTo);
                SendSms(user, smsText);
            }
        }

        //method retrive users phone and sends sms by Twilio service
        private ActionResult SendSms(CalendarUser user, string smsTxt)
        {
            string userPhone = user.PhoneNumber;
            string accountSid = Configuration["ACCOUNT_SEED"];
            string authToken = Configuration["AUTH_TOKKEN"];
            TwilioClient.Init(accountSid, authToken);
            var to = new PhoneNumber(userPhone);
            var from = new PhoneNumber("+15756399489");
            var message = MessageResource.Create(
                to: to,
                from: from,
                body: smsTxt
                );
            NLogger.Info($"text message:{smsTxt} send to phone {userPhone} on {DateTime.Now}");
            return Content(message.Sid);
        }
    }
}
