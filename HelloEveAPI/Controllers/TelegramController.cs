using HelloEveAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Telegram.Net.Core;
using Telegram.Net.Core.Auth;
using Telegram.Net.Core.MTProto;
using Telegram.Net.Core.Requests;

namespace HelloEveAPI.Controllers
{
    [RoutePrefix("api/v0/telegram")]
    public class TelegramController : ApiController
    {
        private const string PATH = "C:\\Temp\\HelloEveAPI\\";

        [Route("sendCode")]
        [HttpPost]
        public SendCode_Response SendCode(SendCode_Request req)
        {
            SendCode_Response resp = new SendCode_Response();
            resp.Token = Guid.NewGuid().ToString();

            var client = Task.Run(async () =>
            {
                var tmp = await CreateClient(new FileSessionStore(PATH + resp.Token + ".dat"));
                return tmp;
            });
            var codeRequest = Task.Run(async () => { return await client.Result.SendCode(req.PhoneNumber, VerificationCodeDeliveryType.NumericCodeViaSms); });
            resp.PhoneHash = codeRequest.Result.phoneCodeHash;
            return resp;
        }

        [Route("signIn")]
        [HttpPost]
        public SignIn_Response SignIn(SignIn_Request req)
        {
            SignIn_Response resp = new SignIn_Response();


            var client = Task.Run(async () =>
            {
                var tmp = await CreateClient(new FileSessionStore(PATH + req.Token + ".dat"));
                return tmp;
            });

            var signIn = Task.Run(async () =>
            {
                return await client.Result.SignIn(req.PhoneNumber, req.PhoneCodeHash, req.Code);
            });


            if (client.Result.IsUserAuthorized())
            {
                resp.Successfull = true;
            }

            return resp;
        }

        [Route("sendMessage")]
        [HttpPost]
        public SendMessage_Response SendMessage(SendMessage_Request req)
        {
            SendMessage_Response resp = new SendMessage_Response();
            try
            {

                var client = Task.Run(async () =>
                {
                    var tmp = await CreateClient(new FileSessionStore(PATH + req.Token + ".dat"));
                    return tmp;
                });

                if (!req.UserId.HasValue || req.UserId.Value == 0)
                {
                    var user = Task.Run(async () => { return await ImportAndGetUser(client.Result, req.PhoneNumber); }); ;
                    req.UserId = user.Result.userId;
                }

                var tno = Task.Run(async () => { return await client.Result.SendDirectMessage(req.UserId.Value, req.Message); });
                resp.UserId = req.UserId;
                resp.Successfull = true;
            }
            catch (Exception e)
            {
                resp.Successfull = false;
            }
            return resp;
        }

        private async Task<ImportedContactConstructor> ImportAndGetUser(TelegramClient client, string phoneNumber)
        {
            var contacts = await client.ImportContactByPhoneNumber(phoneNumber, phoneNumber, "Contact");

            return (ImportedContactConstructor)contacts.importedContacts[0];
        }

        private async Task<TelegramClient> CreateClient(ISessionStore store)
        {
            int apiId = 123864;
            string apiHash = "93cc5e4b602e1d61c162b910e1f32c9a";
            var client = new TelegramClient(store, apiId, apiHash, new DeviceInfo("HelloEve Test", "HelloEve Test", "HelloEve Test", "en"));
            await client.Start();

            return client;
        }

    }
}
